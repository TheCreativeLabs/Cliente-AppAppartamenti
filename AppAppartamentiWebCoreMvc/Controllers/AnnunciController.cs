using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AppAppartamentiWebCoreMvc.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using AppAppartamentiApiClient;
using AppAppartamentiWebCoreMvc.Extensions;
using AppAppartamentiWebCoreMvc.Utility;

namespace AppAppartamentiWebCoreMvc.Controllers
{
    [Authorize]
    public class AnnunciController : Controller
    {
        

        private readonly ILogger<HomeController> _logger;

        public AnnunciController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult ListAsync()
        {
            return View();
        }

        public async Task<ActionResult> RefreshListAsync(int ListPage)
        {
            HttpClient httpClient = new HttpClient();
            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            AnnunciClient annunciClient = new AppAppartamentiApiClient.AnnunciClient(httpClient);
            var FilterModel = HttpContext.Session.GetObject<FilterModalViewModel>(Constants.FilterModalKey);

            var annunci = await annunciClient.GetAnnunciAsync(ListPage,5,FilterModel.IdTipologiaAnnuncio,FilterModel.IdTipologiaProprieta,FilterModel.CodiceComune,
                FilterModel.PrezzoMin,FilterModel.PrezzoMax,FilterModel.DimensioneMin, FilterModel.DimensioneMax,FilterModel.NumeroCamereLetto,
                FilterModel.NumeroBagni, FilterModel.NumeroCucine, FilterModel.NumeroPostiAuto, FilterModel.NumeroGarage,FilterModel.Giardino,null,FilterModel.Cantina,
                FilterModel.Piscina,FilterModel.Ascensore,null);

            ViewData["ListPage"] = ListPage;

            return PartialView("_AnnunciPartial", annunci);
        }

        public async Task<IActionResult> DetailAsync(Guid Id)
        {
            HttpClient httpClient = new HttpClient();
            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            AppAppartamentiApiClient.AnnunciClient annunciClient = new AppAppartamentiApiClient.AnnunciClient(httpClient);
            var annuncio = await annunciClient.GetAnnuncioByIdAsync(Id);

            var classiEnergetiche = await annunciClient.GetListaClasseEnergeticaAsync();
            ViewData["ListaClassiEnergetiche"] = classiEnergetiche.AsEnumerable();

            AccountClient accountClient = new AccountClient(httpClient);
            UserInfoDto userInfo = await accountClient.GetUserInfoAsync(annuncio.IdUtente.Value);
            ViewData["UserInfo"] = userInfo;

            return View(annuncio);
        }

        
    }
}
