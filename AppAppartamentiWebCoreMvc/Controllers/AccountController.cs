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
    public class AccountController : Controller
    {
        IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<string> GetUserInfoAsync()
        {
            //ottengo le info sull'utente
            UserInfoDto userInfoDto =await GetCurrentUserInfoAsync();

            return JsonConvert.SerializeObject(userInfoDto);
        }

        public async Task<IActionResult> DetailAsync()
        {
            //ottengo le info sull'utente
            UserInfoDto userInfoDto = await GetCurrentUserInfoAsync();

            return View(userInfoDto);
        }


        public async Task<IActionResult> EditAsync()
        {
            //ottengo le info sull'utente
            UserInfoDto userInfoDto = await GetCurrentUserInfoAsync();
            ViewData["UserInfo"] = userInfoDto;

            return View();
        }

        private async Task<UserInfoDto> GetCurrentUserInfoAsync()
        {
            //ottengo le info dalla sessione.
            UserInfoDto userInfoDto = HttpContext.Session.GetObject<UserInfoDto>(Constants.UserInfoKey);

            //se non sono in sessione ricarico i dati
            if (userInfoDto == null)
            {
                HttpClient httpClient = new HttpClient();
                var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                AccountClient accountClient = new AccountClient(httpClient);

                userInfoDto = await accountClient.GetCurrentUserInfoAsync();
                HttpContext.Session.SetObject(Constants.UserInfoKey, userInfoDto);
            }

            return userInfoDto;
        }

        [HttpPost]
        public IActionResult EditAsync(UpdateUserBindingModel Model)
        {
            if (Model != null)
            {
                try
                {
                    Model.BirthName = Model.Name;

                    Model.ImmagineProfilo = Utility.Utility.Compress(Model.ImmagineProfilo);

                    HttpClient httpClient = new HttpClient();
                    var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                    AccountClient accountClient = new AccountClient(httpClient);
                    accountClient.UpdateUserAsync(Model);

                    HttpContext.Session.Remove(Constants.UserInfoKey);
                }
                catch (Exception ex)
                {
                    ViewBag["Risultato"] = ex;
                }
            }

            return RedirectToAction("Detail");
        }

        public IActionResult EditPasswordAsync()
        {
           return View();
        }

        [HttpPost]
        public IActionResult EditPasswordAsync(ChangePasswordBindingModel Model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            try
            {
                HttpClient httpClient = new HttpClient();
                var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                AccountClient accountClient = new AccountClient(httpClient);
                accountClient.ChangePasswordAsync(Model);
            }
            catch (Exception ex)
            {
                ViewBag["Risultato"] = ex;
            }

            return RedirectToAction("Detail");
        }
    }
}
