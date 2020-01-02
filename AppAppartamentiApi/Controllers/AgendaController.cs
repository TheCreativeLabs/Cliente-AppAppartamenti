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
    [RoutePrefix("api/Agenda")]
    public class AgendaController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;

        private DbDataContext dbDataContext = new DbDataContext();


        public AgendaController()
        {
        }

        public AgendaController(ApplicationUserManager userManager,
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
        [Route("AppuntamentoCreate", Name = "AppuntamentoCreate")]
        [ResponseType(typeof(Appuntamento))]
        public async Task<IHttpActionResult> InsertAppuntamento(AppuntamentoDto AppuntamentoDto)
        {
            //Controllo se il modello è valido
            if (AppuntamentoDto == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //FIXME VERIFICARE CHE L'EVENTO NON ESISTA già

            Guid idCurrent = new Guid(User.Identity.GetUserId());

            var id = Guid.NewGuid();
            var idDestinatario = AppuntamentoDto.IdDestinatario;
            if(idDestinatario == null)
            {
                idDestinatario = await dbDataContext.Annuncio.Where(x => x.Id == AppuntamentoDto.IdAnnuncio).Select(x => x.IdUtente).FirstOrDefaultAsync();
            }

            //Creo l'Appuntamento
            Appuntamento appuntamento = new Appuntamento()
            {
                Id = id,
                IdRichiedente = idCurrent,
                IdDestinatario = (Guid) idDestinatario,
                IdAnnuncio = AppuntamentoDto.IdAnnuncio,
                Data = AppuntamentoDto.Data,
                Confermato = false
            };

            dbDataContext.Appuntamento.Add(appuntamento);

            CodaNotifiche notifica = new CodaNotifiche() { 
                Id = Guid.NewGuid(),
                IdDestinatario = (Guid)idDestinatario,
                IdRichiedente = idCurrent,
                Sent = false,
                Title = "Richiesta di appuntamento",
                Message = "Hai una richiesta per un nuovo appuntamento. Entra nell'app per accettare!"
            };

            dbDataContext.CodaNotifiche.Add(notifica);

            await dbDataContext.SaveChangesAsync();
            
            return Ok(appuntamento);
        }

        [HttpGet]
        [Route("DisponibilitaByGiorno")]
        [ResponseType(typeof(List<string>))]
        public async Task<List<string>> GetFasceDisponibiliAnnuncioByGiorno(Guid IdAnnuncio, DateTime Giorno)
        {
            if (IdAnnuncio == null || Giorno == null)
            {
                //fixme return BadRequest(ModelState);
            }
            List<string> orariDisponibili = new List<string>();

            FasceOrarie fasceAnnuncio = await dbDataContext.FasceOrarie.Where(x => x.IdAnnuncio == IdAnnuncio).FirstOrDefaultAsync();

            if (fasceAnnuncio != null) { 
                TimeSpan startTime = new TimeSpan(0, 1, 0);
                DateTime GiornoInizio = new DateTime(Giorno.Year, Giorno.Month, Giorno.Day);
                GiornoInizio = GiornoInizio.Date + startTime; //giorno alle 00:01

                TimeSpan endTime = new TimeSpan(23, 59, 0);
                DateTime GiornoFine = new DateTime(Giorno.Year, Giorno.Month, Giorno.Day);
                GiornoFine = GiornoFine.Date + endTime; //giorno alle 23:59

                ICollection <Appuntamento> appuntamentiEsistenti = await dbDataContext.Appuntamento
                                     .Where(x => x.IdAnnuncio == IdAnnuncio && x.Data >= GiornoInizio && x.Data <= GiornoFine).ToListAsync();

                DayOfWeek dayOfWeek = Giorno.DayOfWeek;
                string fasceOfDay = getFasciaByDayOfWeek(fasceAnnuncio, dayOfWeek);

                if (fasceOfDay == null)
                {
                    return orariDisponibili;
                }

                string durataAppuntamentoMin = Properties.Settings.Default.MinutiDurataAppuntamento;

                string[] fasce = fasceOfDay.Split(';');
                fasce = fasce.Take(fasce.Count() - 1).ToArray();

                foreach(string fascia in fasce)
                {
                    //la divido in intervalli di durataAppuntamentoMin
                    List<string> singoliIntervalli = new List<string>();
                    string orarioStart = fascia.Split('-')[0];
                    string orarioEnd = fascia.Split('-')[1];
                    //FIXME gestire eccezione o usare try parse
                    TimeSpan start = new TimeSpan(Int32.Parse(orarioStart.Split(':')[0]), Int32.Parse(orarioStart.Split(':')[1]), 0);
                    TimeSpan end = new TimeSpan(Int32.Parse(orarioEnd.Split(':')[0]), Int32.Parse(orarioEnd.Split(':')[1]), 0);
                    TimeSpan incremento = new TimeSpan(0, Int32.Parse(durataAppuntamentoMin), 0);


                    TimeSpan inizioAppuntamento = start;
                    TimeSpan fineAppuntamento = start + incremento;
                    //List<string> ore = new List<string>();
                    while (inizioAppuntamento != end)
                    {

                        //se  l'intervallo non è già impegnato , lo aggiungo agli orari disponibili
                        //passo all'intervallo successivo
                        Appuntamento appuntamento = appuntamentiEsistenti.Where(x => x.Data.Hour == inizioAppuntamento.Hours && x.Data.Minute == inizioAppuntamento.Minutes).FirstOrDefault();
                        if (appuntamento == null)
                        {
                            orariDisponibili.Add(
                                                    new DateTime(inizioAppuntamento.Ticks).ToString("HH:mm")
                                                    + "-" + 
                                                    new DateTime(fineAppuntamento.Ticks).ToString("HH:mm")
                                                );
                        }
                        inizioAppuntamento = inizioAppuntamento + incremento;
                        fineAppuntamento = fineAppuntamento + incremento;


                        //    i = i.Add(incremento);
                        //    //string ora = i.Hours + ":" + i.Minutes;
                        //    string ora = new DateTime(i.Ticks).ToString("HH:mm");
                        //    ore.Add(ora);
                    }

                }
            }

            return orariDisponibili;

        }

        [HttpGet]
        [Route("AgendaCurrentByGiorno")]
        [ResponseType(typeof(List<AppuntamentoDtoOutput>))]
        public async Task<List<AppuntamentoDtoOutput>> GetAgendaCurrentByGiorno(DateTime Giorno)
        {
            if (Giorno == null)
            {
                throw new Exception("Giorno agenda obbligatorio");
            }

            Guid currentUserId = new Guid(User.Identity.GetUserId());

            TimeSpan startTime = new TimeSpan(0, 1, 0);
            DateTime GiornoInizio = new DateTime(Giorno.Year, Giorno.Month, Giorno.Day);
            GiornoInizio = GiornoInizio.Date + startTime; //giorno alle 00:01

            TimeSpan endTime = new TimeSpan(23, 59, 0);
            DateTime GiornoFine = new DateTime(Giorno.Year, Giorno.Month, Giorno.Day);
            GiornoFine = GiornoFine.Date + endTime; //giorno alle 23:59

            List<AppuntamentoDtoOutput> appuntamenti = await dbDataContext.Appuntamento
                                                .Include(x => x.Annuncio)
                                                .Where(x => x.Data >= GiornoInizio && x.Data <= GiornoFine && (x.IdDestinatario == currentUserId || ( x.IdRichiedente == currentUserId && x.Confermato == true)))
                                                .Select(appuntamento => new AppuntamentoDtoOutput()
                                                {
                                                    IdAppuntamento = appuntamento.Id,
                                                    IdAnnuncio = appuntamento.IdAnnuncio,
                                                    IdDestinatario = appuntamento.IdDestinatario,
                                                    IdRichiedente = appuntamento.IdRichiedente,
                                                    Data = appuntamento.Data,
                                                    CodiceComune = appuntamento.Annuncio.ComuneCodice != null ? appuntamento.Annuncio.ComuneCodice : null,
                                                    Indirizzo = appuntamento.Annuncio.Indirizzo,
                                                    Confermato = appuntamento.Confermato
                                                })
                                                .ToListAsync();

            foreach (AppuntamentoDtoOutput appuntamento in appuntamenti)
            {
                appuntamento.NomeComune = await dbDataContext.Comuni.Where(x => x.CodiceComune == appuntamento.CodiceComune).Select(x => x.NomeComune).FirstOrDefaultAsync(); ;
                Guid idPersonToMeet;

                if (currentUserId == appuntamento.IdDestinatario) //allora devo restituire le info del richiedente
                {
                    idPersonToMeet = appuntamento.IdRichiedente;
                }
                else  //allora devo restituire le info del destinatario
                {
                    idPersonToMeet = appuntamento.IdDestinatario;
                }
                appuntamento.NameAndSurnamePersonToMeet = await dbDataContext.UserInfo
                                                        .Where(x => x.IdAspNetUser == idPersonToMeet)
                                                        .Select(x => x.Nome + " " + x.Cognome).FirstOrDefaultAsync();
            }

            return appuntamenti;

        }

        private class BaseInfoUser
        {
            public Guid IdUser { get; set; }
            public string NomeCognome { get; set; }
            public byte[] Immagine  { get; set; }
        }

        //GET /api/Agenda/AppuntamentoById?IdAppuntamento=7
        [HttpGet]
        [Route("AppuntamentoById")]
        [ResponseType(typeof(AppuntamentoDettaglioDtoOutput))]
        public async Task<IHttpActionResult> GetAppuntamentoById(Guid IdAppuntamento)
        {
            if (IdAppuntamento == null)
            {
                //fixme return BadRequest(ModelState);
            }

            Guid currentUserId = new Guid(User.Identity.GetUserId());

            AppuntamentoDettaglioDtoOutput appuntamento = await dbDataContext.Appuntamento
                                        .Where(x => x.Id == IdAppuntamento)
                                        .Select(app => new AppuntamentoDettaglioDtoOutput() { 
                                        IdAppuntamento = app.Id,
                                        IdAnnuncio = app.IdAnnuncio,
                                        IdDestinatario = app.IdDestinatario,
                                        IdRichiedente = app.IdRichiedente,
                                        Confermato = app.Confermato,
                                        Data = app.Data,
                                        FromMe = (app.IdRichiedente == currentUserId ? true : false)
                                        })
                                        .FirstOrDefaultAsync();

            if(appuntamento == null)
            {
                return NotFound();
            }


            Annuncio ann = await dbDataContext.Annuncio
                                .Include(x => x.Comuni)
                                .Include(x => x.TipologiaAnnuncio)
                                .Include(x => x.TipologiaProprieta)
                                .Where(x => x.Id == appuntamento.IdAnnuncio)
                                .FirstOrDefaultAsync();

            if (ann == null)
            {
                return NotFound();
            }

            appuntamento.CoordinateGeograficheAnnuncio = ann.CoordinateGeografiche;

            AnnunciDtoOutput annuncio = new AnnunciDtoOutput()
            {
                Id = ann.Id,
                IdUtente = ann.IdUtente,
                DataCreazione = ann.DataCreazione,
                DataModifica = ann.DataModifica,
                CodiceComune = ann.ComuneCodice,
                NomeComune = ann.Comuni.NomeComune,
                Indirizzo = ann.Indirizzo,
                Prezzo = ann.Prezzo,
                Superficie = ann.Superficie,
                Descrizione = ann.Descrizione,
                TipologiaAnnuncio = ann.TipologiaAnnuncio.Descrizione,
                TipologiaProprieta = ann.TipologiaProprieta.Descrizione,
                Completato = ann.Completato,
                Cancellato = ann.Cancellato
            };

            ImmagineAnnuncio immagine = await dbDataContext.ImmagineAnnuncio
                                                .Where(x => x.IdAnnuncio == annuncio.Id).FirstOrDefaultAsync();

            if(immagine != null)
            {
                annuncio.ImmaginePrincipale = immagine.Immagine;
            }

            appuntamento.InfoAnnuncio = annuncio;

            //----------------ATTENZIONE: FIXME PRESTAZIONI-----------------
            //NameAndSurnamePersonToMeet vengono già prese in AgendaCurrentByGiorno
            //quindi in realtà nel momento in cui viene richiesto il dettaglio, l'informazione NameAndSurnamePersonToMeet
            //è già presente nell'oggetto della lista appuntamenti e potremmo non rifare la query su UserInfo

            Guid idPersonToMeet;

            if (currentUserId == appuntamento.IdDestinatario) //allora devo restituire le info del richiedente
            {
                idPersonToMeet = appuntamento.IdRichiedente;
            }
            else  //allora devo restituire le info del destinatario
            {
                idPersonToMeet = appuntamento.IdDestinatario;
            }
            BaseInfoUser infoUser = await dbDataContext.UserInfo
                                                    .Where(x => x.IdAspNetUser == idPersonToMeet)
                                                    .Select(x => new BaseInfoUser() { 
                                                        IdUser = idPersonToMeet,
                                                        NomeCognome = x.Nome + " " + x.Cognome,
                                                        Immagine = x.FotoProfilo
                                                    }).FirstOrDefaultAsync();

            appuntamento.NameAndSurnamePersonToMeet = infoUser.NomeCognome;
            appuntamento.ImagePersonToMeet= infoUser.Immagine;
            appuntamento.IdPersonToMeet = infoUser.IdUser;





            return Ok(appuntamento);

        }

        [HttpPut]
        [Route("AppuntamentoConferma/{IdAppuntamento:Guid}")]
        [ResponseType(typeof(Appuntamento))]
        public async Task<IHttpActionResult> ConfermaAppuntamento([FromUri]Guid IdAppuntamento)
        {
            //Controllo che i parametri siano valorizzati
            if (!ModelState.IsValid || (IdAppuntamento == null || IdAppuntamento == Guid.Empty))
            {
                return BadRequest(ModelState);
            }

            //Cerco l'appuntamento
            Appuntamento appuntamento = await dbDataContext.Appuntamento
                                        .Where(x => x.Id == IdAppuntamento).FirstOrDefaultAsync();
            if (appuntamento == null)
            {
                return NotFound();
            }

            //Modifico l'appuntamento
            appuntamento.Confermato = true;



            CodaNotifiche notifica = new CodaNotifiche()
            {
                Id = Guid.NewGuid(),
                IdDestinatario = appuntamento.IdRichiedente, //devo dire al richiedente dell'appuntamento che è stato accettato: il richiedente appuntamento è il destinatario della notifica
                IdRichiedente = appuntamento.IdDestinatario,
                Sent = false,
                Title = "Conferma di appuntamento",
                Message = "Il tuo appuntamento è stato confermato! Entra nell'app per visualizzare i dettagli."
            };

            dbDataContext.CodaNotifiche.Add(notifica);


            try
            {
                //Salvo le modifiche sul DB.
                await dbDataContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok(appuntamento); 
        }

        //DELETE: api/Agenda/AppuntamentoDelete/5
        [HttpDelete]
        [Route("AppuntamentoDelete/{id}")]
        public async Task<IHttpActionResult> DeleteAppuntamento(Guid Id)
        {
            Appuntamento appuntamento = await dbDataContext.Appuntamento
                                                .FirstOrDefaultAsync(x => x.Id == Id);
            if (appuntamento == null)
            {
                return NotFound();
            }

            Guid destinatarioAppuntamento = appuntamento.IdDestinatario;
            Guid richiedenteAppuntamento = appuntamento.IdRichiedente;
            dbDataContext.Appuntamento.Remove(appuntamento);

            try
            {
                //Salvo le modifiche sul DB.
                await dbDataContext.SaveChangesAsync();

                Guid idCurrent = new Guid(User.Identity.GetUserId());

                //deve ricevere la notifica chi SUBISCE l'eliminazione, il richiedente della notifica è il current
                Guid destinatarioNotifica = (idCurrent == destinatarioAppuntamento) ? richiedenteAppuntamento : destinatarioAppuntamento;
                CodaNotifiche notifica = new CodaNotifiche()
                {
                    Id = Guid.NewGuid(),
                    IdDestinatario = appuntamento.IdRichiedente, //devo dire al richiedente dell'appuntamento che è stato accettato: il richiedente appuntamento è il destinatario della notifica
                    IdRichiedente = idCurrent,
                    Sent = false,
                    Title = "Appuntamento eliminato",
                    Message = "Il tuo appuntamento è stato eliminato. Entra nell'app per visualizzare i dettagli."
                };

                dbDataContext.CodaNotifiche.Add(notifica);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok();
        }

        private string getFasciaByDayOfWeek(FasceOrarie fasce, DayOfWeek dayOfWeek)
        {
            string fascia = null;
            if(fasce != null) { 
                switch (dayOfWeek)
                {
                    case DayOfWeek.Monday:
                        fascia = fasce.Monday;
                        break;
                    case DayOfWeek.Tuesday:
                        fascia = fasce.Tuesday;
                        break;
                    case DayOfWeek.Wednesday:
                        fascia = fasce.Wednesday;
                        break;
                    case DayOfWeek.Thursday:
                        fascia = fasce.Thursday;
                        break;
                    case DayOfWeek.Friday:
                        fascia = fasce.Friday;
                        break;
                    case DayOfWeek.Saturday:
                        fascia = fasce.Saturday;
                        break;
                    case DayOfWeek.Sunday:
                        fascia = fasce.Sunday;
                        break;
                };
            }
            return fascia;
        }
    }
}
