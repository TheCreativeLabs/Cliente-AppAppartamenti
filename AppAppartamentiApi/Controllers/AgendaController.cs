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
        public async Task<IHttpActionResult> InsertAnnuncio(AppuntamentoDto AppuntamentoDto)
        {
            //Controllo se il modello è valido
            if (AppuntamentoDto == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //FIXME VERIFICARE CHE L'EVENTO NON ESISTA già

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
                IdRichiedente = new Guid(User.Identity.GetUserId()),
                IdDestinatario = (Guid) idDestinatario,
                IdAnnuncio = AppuntamentoDto.IdAnnuncio,
                Data = AppuntamentoDto.Data
            };

            dbDataContext.Appuntamento.Add(appuntamento);

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

            FasceOrarie fasceAnnuncio = await dbDataContext.FasceOrarie.Where(x => x.IdAnnuncio == IdAnnuncio).FirstOrDefaultAsync();
            TimeSpan startTime = new TimeSpan(0, 1, 0);
            DateTime GiornoInizio = new DateTime(Giorno.Year, Giorno.Month, Giorno.Day);
            GiornoInizio = GiornoInizio.Date + startTime; //giorno alle 00:01

            TimeSpan endTime = new TimeSpan(23, 59, 0);
            DateTime GiornoFine = new DateTime(Giorno.Year, Giorno.Month, Giorno.Day);
            GiornoFine = GiornoFine.Date + endTime;

            ICollection <Appuntamento> appuntamentiEsistenti = await dbDataContext.Appuntamento
                                 .Where(x => x.IdAnnuncio == IdAnnuncio && x.Data >= GiornoInizio && x.Data <= GiornoFine).ToListAsync();

            DayOfWeek dayOfWeek = Giorno.DayOfWeek;
            string fasceOfDay = getFasciaByDayOfWeek(fasceAnnuncio, dayOfWeek);

            string durataAppuntamentoMin = Properties.Settings.Default.MinutiDurataAppuntamento;

            string[] fasce = fasceOfDay.Split(';');
            fasce = fasce.Take(fasce.Count() - 1).ToArray();
            List<string> orariDisponibili = new List<string>();

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

            return orariDisponibili;

        }

        private string getFasciaByDayOfWeek(FasceOrarie fasce, DayOfWeek dayOfWeek)
        {
            string fascia = null;
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
            return fascia;
        }
    }
}
