using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AppAppartamentiWebCoreMvc.Models;
using System.Net.Http;
using AppAppartamentiWebCoreMvc.AppAppartamentiApiClient;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using AppAppartamentiWebCoreMvc.Extensions;
using AppAppartamentiWebCoreMvc.Utility;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;

namespace AppAppartamentiWebCoreMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult FacebookLogin()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<ActionResult> FilterModalAsync()
        {
            HttpClient httpClient = new HttpClient();
            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            //Ottengo la lista delle tipologie proprietà
            AnnunciClient annunciClient = new AnnunciClient(httpClient);
            var listTipologiaProprieta = await annunciClient.GetListaTipologiaProprietaAsync();
            var listTipologiaAnnuncio = await annunciClient.GetListaTipologiaAnnunciAsync();

            ViewData["ListaTipologiaProprieta"] = listTipologiaProprieta;
            ViewData["ListaTipologiaAnnuncio"] = listTipologiaAnnuncio;

            var FilterModel = HttpContext.Session.GetObject<FilterModalViewModel>(Constants.FilterModalKey);

            return PartialView("_FilterModal",FilterModel);
        }

        [HttpPost]
        [Authorize]
        public string SaveSearchFilter(FilterModalViewModel Model)
        {
            HttpContext.Session.SetObject(Constants.FilterModalKey, Model);

            return JsonConvert.SerializeObject("OK");
        }

        [HttpPost]
        [Authorize]
        public async Task<string> ListaComuni(string NomeComune)
        {
            ICollection<ComuneDto> comuni = null;

            if (!string.IsNullOrEmpty(NomeComune))
            {
                //Creo il client e setto il Baerer Token
                HttpClient httpClient = new HttpClient();
                var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                //Ottengo la lista del comune
                AnnunciClient annunciClient = new AnnunciClient(httpClient);
                comuni = await annunciClient.GetListaComuniAsync(NomeComune);
            }

            return JsonConvert.SerializeObject(comuni);
        }

        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
    }
}
