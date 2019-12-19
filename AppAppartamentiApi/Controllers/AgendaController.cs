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
        [ResponseType(typeof(AppuntamentoDto))]
        public async Task<IHttpActionResult> InsertAnnuncio(AppuntamentoDto AppuntamentoDto)
        {
            //Controllo se il modello è valido
            if (AppuntamentoDto == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = Guid.NewGuid();

            //Creo l'Appuntamento
            Appuntamento appuntamento = new Appuntamento()
            {
                Id = id,
                IdRichiedente = new Guid(User.Identity.GetUserId()),
                IdDestinatario = AppuntamentoDto.IdDestinatario,
                IdAnnuncio = AppuntamentoDto.IdAnnuncio
            };

            await dbDataContext.SaveChangesAsync();
            
            return Ok(appuntamento);
        }

        [HttpGet]
        [Route("DisponibilitaByGiorno")]
        [ResponseType(typeof(string))]
        public async Task<string> GetFasceDisponibiliAnnuncioByGiorno(Guid IdAnnuncio, DateTime Giorno)
        {
            if (IdAnnuncio == null || Giorno == null)
            {
                //fixme return BadRequest(ModelState);
            }

            FasceOrarie fasce = await dbDataContext.FasceOrarie.Where(x => x.IdAnnuncio == IdAnnuncio).FirstOrDefaultAsync();
            TimeSpan start = new TimeSpan(0, 1, 0);
            DateTime GiornoInizio = new DateTime(Giorno.Year, Giorno.Month, Giorno.Day);
            GiornoInizio = GiornoInizio.Date + start; //giorno alle 00:01

            TimeSpan end = new TimeSpan(23, 59, 0);
            DateTime GiornoFine = new DateTime(Giorno.Year, Giorno.Month, Giorno.Day);
            GiornoFine = GiornoFine.Date + end;

            ICollection <Appuntamento> appuntamentiEsistenti = await dbDataContext.Appuntamento
                                 .Where(x => x.IdAnnuncio == IdAnnuncio && x.Data >= GiornoInizio && x.Data <= GiornoFine).ToListAsync();

            DayOfWeek dayOfWeek = Giorno.DayOfWeek;
            string fascia = getFasciaByDayOfWeek(fasce, dayOfWeek);

            string durataAppuntamentoMin = Properties.Settings.Default.MinutiDurataAppuntamento;

            return "";

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
