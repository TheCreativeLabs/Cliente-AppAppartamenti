﻿using System;
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

        public async Task<IActionResult> ListAsync()
        {
            HttpClient httpClient = new HttpClient();

            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            AppAppartamentiApiClient.AnnunciClient annunciClient = new AppAppartamentiApiClient.AnnunciClient(httpClient);
            var annunci = await annunciClient.GetAnnunciAsync();

            return View(annunci.AsEnumerable());
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


            return View(annuncio);
        }

        [HttpGet]
        public async Task<IActionResult> CreateAsync()
        {
            HttpClient httpClient = new HttpClient();
            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);


            AnnunciClient annunciClient = new AnnunciClient(httpClient);
            var classiEnergetiche =await  annunciClient.GetListaClasseEnergeticaAsync();
            var tipologieProprieta =await annunciClient.GetListaTipologiaProprietaAsync();
            var tipologiaAnnunci  = await annunciClient.GetListaTipologiaAnnunciAsync();
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
            HttpClient httpClient = new HttpClient();
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