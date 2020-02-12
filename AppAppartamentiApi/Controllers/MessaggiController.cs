using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using AppAppartamentiApi.Models;
using AppAppartamentiApi.Providers;
using AppAppartamentiApi.Results;
using System.Linq;
using System.Web.Http.Description;
using System.Data.Entity;
using System.Web.Http.Results;
using System.Net;
using System.Data.Entity.Infrastructure;
using AppAppartamentiApi.Dto;

namespace AppAppartamentiApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Messaggi")]
    public class MessaggiController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;

        private DbDataContext dbDataContext = new DbDataContext();


        public MessaggiController()
        {
        }

        public MessaggiController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // POST: api/Agenda/AppuntamentoCreate
        [HttpPost]
        [Route("MessaggioCreate", Name = "MessaggioCreate")]
        public async Task<IHttpActionResult> InsertMessaggio(Guid IdChat,Guid IdDestinatario, String Messaggio)
        {
            var idAspNetUser = new Guid(User.Identity.GetUserId());

            AnnuncioMessaggi annuncioMessaggi = new AnnuncioMessaggi()
            {
                Id = Guid.NewGuid(),
                IdChat = IdChat,
                IdDestinatario = IdDestinatario,
                Messaggio = Messaggio,
                IdMittente = idAspNetUser,
                DataInserimento = DateTime.Now,
                DataLettura = null
            };

            dbDataContext.AnnuncioMessaggi.Add(annuncioMessaggi);

            CodaNotifiche codaNotifiche = new CodaNotifiche()
            {
                Id = Guid.NewGuid(),
                IdDestinatario = IdDestinatario,
                IdRichiedente = idAspNetUser,
                Title = "Nuovo messaggio",
                Message = $"Hai ricevuto un nuovo messaggio",
            };
            dbDataContext.CodaNotifiche.Add(codaNotifiche);
            await dbDataContext.SaveChangesAsync();

            return Ok();

        }


        [HttpGet ]
        [Route("ChatListOttieni")]
        [ResponseType(typeof(List<ChatListDtoOutput>))]
        public async Task<List<ChatListDtoOutput>> GetChatListAsync()
        {
            var idAspNetUser = new Guid(User.Identity.GetUserId());

            var listaChat = await dbDataContext.
                    Chat
                    .Include(x => x.AnnuncioMessaggi)
                     .Join(dbDataContext.UserInfo, // the source table of the inner join
                                     chat => chat.IdUserMittente,        // Select the primary key (the first part of the "on" clause in an sql "join" statement)
                                     userInfo => userInfo.IdAspNetUser,   // Select the foreign key (the second part of the "on" clause)
                                     (chat, userInfo) => new { Chat = chat, UserInfo = userInfo }) // selection
                     .Join(dbDataContext.UserInfo, // the source table of the inner join
                                     chat2 => chat2.Chat.IdUserDestinatario,        // Select the primary key (the first part of the "on" clause in an sql "join" statement)
                                     userInfo => userInfo.IdAspNetUser,   // Select the foreign key (the second part of the "on" clause)
                                     (chat2, userInfo) => new { Chat = chat2, UserInfo2 = userInfo })
                    .Where(x => (x.Chat.Chat.IdUserDestinatario == idAspNetUser || x.Chat.Chat.IdUserMittente == idAspNetUser))
                    .Select(x => new ChatListDtoOutput()
                    {
                        IdAnnuncio = x.Chat.Chat.IdAnnuncio,
                        IdChat = x.Chat.Chat.Id,
                        IdUser = (x.Chat.Chat.IdUserDestinatario == idAspNetUser ? x.Chat.Chat.IdUserMittente : x.Chat.Chat.IdUserDestinatario),
                        Nome = (x.Chat.Chat.IdUserDestinatario == idAspNetUser ? x.Chat.UserInfo.Nome : x.UserInfo2.Nome),
                        Cognome = (x.Chat.Chat.IdUserDestinatario == idAspNetUser ? x.Chat.UserInfo.Cognome : x.UserInfo2.Cognome),
                        DataCreazione = x.Chat.Chat.DataCreazione,
                        FotoUtente = (x.Chat.Chat.IdUserDestinatario == idAspNetUser ? x.Chat.UserInfo.FotoProfilo : x.UserInfo2.FotoProfilo),
                        NumberMsgToRead = x.Chat.Chat.AnnuncioMessaggi.Where(y => y.DataLettura == null && y.IdDestinatario == idAspNetUser).Count()
                    }).ToListAsync();

            return listaChat;
        }

        [HttpGet]
        [Route("MessaggiDaLeggere")]
        [ResponseType(typeof(int))]
        public async Task<int> GetChatMessagesToRead()
        {
            var idAspNetUser = new Guid(User.Identity.GetUserId());

            var numNewMessage =  dbDataContext.AnnuncioMessaggi
                      .Where(x => x.IdDestinatario == idAspNetUser && !x.DataLettura.HasValue).Count();

            return numNewMessage;
        }

        [HttpPost]
        [Route("ChatOttieni")]
        [ResponseType(typeof(ChatDtoOutput))]
        public async Task<ChatDtoOutput> GetChatAsync(Guid? IdChat = null, Guid? IdAnnuncio = null, Guid? IdUser = null)
        {
            var idAspNetUser = new Guid(User.Identity.GetUserId());

            ChatDtoOutput chat  = null; 

            if (IdChat.HasValue)
            {
                    var chatInfo = dbDataContext
                      .Chat
                      .Include(x => x.AnnuncioMessaggi)
                      .Where(x => x.Id == IdChat.Value).First();

                var msg = chatInfo.AnnuncioMessaggi.Where(x => !x.DataLettura.HasValue && x.IdDestinatario == idAspNetUser).ToList();
                foreach (var item in msg)
                {
                    item.DataLettura = DateTime.Now;
                }
                await dbDataContext.SaveChangesAsync();

                chat = new ChatDtoOutput()
                {
                    IdChat = chatInfo.Id,
                    IdAnnuncio = chatInfo.IdAnnuncio,
                    IdUser = (chatInfo.IdUserMittente == idAspNetUser ? chatInfo.IdUserDestinatario : chatInfo.IdUserMittente),
                    Messaggi = chatInfo.AnnuncioMessaggi.Select(y => new MessaggioDto()
                    {
                        IdDestinatario = y.IdDestinatario,
                        IdMittente = y.IdMittente,
                        Messaggio = y.Messaggio,
                        DataInserimento = y.DataInserimento,
                        DataLettura = y.DataLettura,
                        FromMe = (y.IdMittente == idAspNetUser ? true : false)
                    }).OrderBy(y => y.DataInserimento).ToList()
                };

            }
            else if(IdAnnuncio.HasValue)
            {
                //Controllo se c'è già una chat
                var listChatInfo = dbDataContext
                    .Chat
                    .Include(x => x.AnnuncioMessaggi)
                    .Where(x => x.IdAnnuncio == IdAnnuncio.Value && (x.IdUserDestinatario == IdUser.Value || x.IdUserMittente == IdUser.Value) && (x.IdUserDestinatario == idAspNetUser || x.IdUserMittente == idAspNetUser))
                    .ToList();

                if (listChatInfo.Any()) {
                    var chatInfo = listChatInfo.First();

                    var msg = chatInfo.AnnuncioMessaggi.Where(x => !x.DataLettura.HasValue && x.IdDestinatario == idAspNetUser).ToList();
                    foreach (var item in msg)
                    {
                        item.DataLettura = DateTime.Now;
                    }
                    await dbDataContext.SaveChangesAsync();

                    chat = new ChatDtoOutput()
                    {
                        IdChat = chatInfo.Id,
                        IdAnnuncio = chatInfo.IdAnnuncio,
                        IdUser = (chatInfo.IdUserMittente == idAspNetUser ? chatInfo.IdUserDestinatario : chatInfo.IdUserMittente),
                        Messaggi = chatInfo.AnnuncioMessaggi.Select(y => new MessaggioDto()
                        {
                            IdDestinatario = y.IdDestinatario,
                            IdMittente = y.IdMittente,
                            Messaggio = y.Messaggio,
                            DataInserimento = y.DataInserimento,
                            DataLettura = y.DataLettura,
                            FromMe = (y.IdMittente == idAspNetUser ? true : false)
                        }).OrderBy(y => y.DataInserimento).ToList()
                    };
                }
                else
                {
                    //Altrimenti la creo
                    Chat newChat = new Chat()
                    {
                        Id = Guid.NewGuid(),
                        IdAnnuncio = IdAnnuncio.Value,
                        IdUserMittente = idAspNetUser,
                        IdUserDestinatario = IdUser.Value,
                        DataCreazione = DateTime.Now,
                    };
                    dbDataContext.Chat.Add(newChat);
                    await dbDataContext.SaveChangesAsync();

                    chat = new ChatDtoOutput()
                    {
                        IdChat = newChat.Id,
                        IdAnnuncio = newChat.IdAnnuncio,
                        Messaggi = new List<MessaggioDto>()
                    };
                }
            }




            // OTTENGO LA CHAT PER IDCHAT
            // OTTENGO I MESSAGGI DELLA CHAT


            //SE LA CHAT E' VUOTA LA CREO

            return chat;
        }
    }
}
