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

namespace AppAppartamentiApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Notifiche")]
    public class NotificheController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;

        private DbDataContext dbDataContext = new DbDataContext();


        public NotificheController()
        {
        }

        public NotificheController(ApplicationUserManager userManager,
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

        // PUT: api/Evento/EventoUpdate/1
        [HttpPut]
        [Route("UpdateInfoNotification")]
        [ResponseType(typeof(NotificationInfoDto))]
        public async Task<IHttpActionResult> UpdateInfoNotification([FromBody]NotificationInfoDto NotificationInfoDto)
        {
            //Controllo che i parametri siano valorizzati
            if (NotificationInfoDto == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Guid idCurrent = new Guid(User.Identity.GetUserId());



            //Cerco l'annuncio
            UserInfo userInfo = await dbDataContext.UserInfo
                                        .Where(x => x.IdAspNetUser == idCurrent).FirstOrDefaultAsync();
            if (userInfo == null)
            {
                return NotFound();
            }

            userInfo.OsVersion = NotificationInfoDto.OsVersion;
            userInfo.InstallationId = NotificationInfoDto.InstallationId;

            try
            {
                //Salvo le modifiche sul DB.
                await dbDataContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok(NotificationInfoDto);
        }
    }
}
