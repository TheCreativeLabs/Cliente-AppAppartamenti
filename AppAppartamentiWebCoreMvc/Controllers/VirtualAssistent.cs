using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AppAppartamentiWebCoreMvc.Models;
using Microsoft.Extensions.Configuration;
using AppAppartamenti.Api;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using AppAppartamentiWebCoreMvc.AppAppartamentiApiClient;
using static AppAppartamenti.Api.ApiHelper;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using AppAppartamentiWebCoreMvc.Extensions;
using AppAppartamentiWebCoreMvc.Utility;
using System.Drawing;

namespace AppAppartamentiWebCoreMvc.Controllers
{
    [Authorize]
    public class VirtualAssistent : Controller
    {
        IConfiguration _configuration;

        public VirtualAssistent(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> DetailAsync()
        {
            HttpClient httpClient = new HttpClient();
            AccountClient accountClient = new AccountClient(httpClient);
            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var avatars = await accountClient.GetAllAvatarAsync();

            return View(avatars.ToList());
        }

        [HttpPost]
        public async void ChangeAvatarImage(string Id)
        {
            Guid AvatarId = Guid.Empty;

            if(Guid.TryParse(Id, out AvatarId))
            {
                HttpClient httpClient = new HttpClient();
                AccountClient accountClient = new AccountClient(httpClient);
                var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                await accountClient.UpdateAvatarCurrentUserAsync(AvatarId);

                //rimuovo l'oggetto dalla sessione
                HttpContext.Session.Remove(Constants.AvatarInfoKey);
            }
        }

        public async Task<ActionResult> GetAvatar(bool IsForBuy)
        {
            HttpClient httpClient = new HttpClient();
            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            ListaDocumentiDto docs = new ListaDocumentiDto();

            //ottengo le info dalla sessione.
            byte[] avatar = HttpContext.Session.GetObject<byte[]>(Constants.AvatarInfoKey);

            //se non sono in sessione ricarico i dati
            if (avatar == null)
            {
                AccountClient accountClient = new AccountClient(httpClient);

                avatar = await accountClient.GetAvatarCurrentUserAsync();
                HttpContext.Session.SetObject(Constants.AvatarInfoKey, avatar);

              
            }

            ViewData["AvatarImage"] = avatar;

            AnnunciClient adminClient = new AnnunciClient(httpClient);
            if (IsForBuy)
            {
                docs = await adminClient.GetDocumentiAcquistoAsync();
            }
            else
            {
                docs = await adminClient.GetDocumentiVenditaAsync();
            }

            return PartialView("~/Views/VirtualAssistent/_VirtualAssistentPartial.cshtml", docs);
        }
    }
}
