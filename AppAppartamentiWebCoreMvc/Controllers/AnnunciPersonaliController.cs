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
using AppAppartamentiWebCoreMvc.AppAppartamentiApiClient;
using System.Collections.ObjectModel;

namespace AppAppartamentiWebCoreMvc.Controllers
{
    [Authorize]
    public class AnnunciPersonaliController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public AnnunciPersonaliController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> ListAsync()
        {
            HttpClient httpClient = new HttpClient();
            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            AnnunciClient annunciClient = new AnnunciClient(httpClient);
            var annunci = await annunciClient.GetAnnunciByUserAsync(1,50);

            return View(annunci.AsEnumerable());
        }

        public async Task<IActionResult> EditAsync(Guid Id)
        {
            HttpClient httpClient = new HttpClient();
            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            AnnunciClient annunciClient = new AnnunciClient(httpClient);
            var annuncio = await annunciClient.GetAnnuncioByIdAsync(Id);

            var classiEnergetiche = await annunciClient.GetListaClasseEnergeticaAsync();
            var tipologieProprieta = await annunciClient.GetListaTipologiaProprietaAsync();
            var tipologiaAnnunci = await annunciClient.GetListaTipologiaAnnunciAsync();
            var tipologiaRiscaldamento = await annunciClient.GetListaTipologiaRiscaldamentoAsync();
            ViewData["ListaTipologieProprieta"] = tipologieProprieta.AsEnumerable();
            ViewData["ListaTipologieAnnunci"] = tipologiaAnnunci.AsEnumerable();
            ViewData["ListaClassiEnergetiche"] = classiEnergetiche.AsEnumerable();
            ViewData["ListaTipologieRiscaldamento"] = tipologiaRiscaldamento.AsEnumerable();
            ViewData["Annuncio"] = annuncio;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(AnnuncioDtoInput Model, Guid Id)
        {
            HttpClient httpClient = new HttpClient();
            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            AnnunciClient annunciClient = new AnnunciClient(httpClient);
            var annuncio = await annunciClient.UpdateAnnuncioAsync(Id, Model);

            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> CreateAsync()
        {
            HttpClient httpClient = new HttpClient();
            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            AnnunciClient annunciClient = new AnnunciClient(httpClient);

            var classiEnergetiche = await annunciClient.GetListaClasseEnergeticaAsync();
            var tipologieProprieta = await annunciClient.GetListaTipologiaProprietaAsync();
            var tipologiaAnnunci = await annunciClient.GetListaTipologiaAnnunciAsync();
            var tipologiaRiscaldamento = await annunciClient.GetListaTipologiaRiscaldamentoAsync();
            ViewData["ListaTipologieProprieta"] = tipologieProprieta.AsEnumerable();
            ViewData["ListaTipologieAnnunci"] = tipologiaAnnunci.AsEnumerable();
            ViewData["ListaClassiEnergetiche"] = classiEnergetiche.AsEnumerable();
            ViewData["ListaTipologieRiscaldamento"] = tipologiaRiscaldamento.AsEnumerable();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(AnnuncioDtoInput Model)
        {
            if (!ModelState.IsValid)
            {
                RedirectToPage("/");
            }

            HttpClient httpClient = new HttpClient();

            Collection<byte[]> immagini = new Collection<byte[]>();
            foreach (var item in Model.Immagini)
            {
                immagini.Add(Utility.Utility.Compress(item));
            }

            Model.Immagini = immagini;

            Collection<byte[]> planimetrie = new Collection<byte[]>();
            foreach (var item in Model.ImmaginePlanimetria)
            {
                planimetrie.Add(Utility.Utility.Compress(item));
            }

            Model.ImmaginePlanimetria = planimetrie;
            
            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            AnnunciClient annunciClient = new AnnunciClient(httpClient);
            await annunciClient.InsertAnnuncioAsync(Model);

            return View("../Home/Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
