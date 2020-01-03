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
using Newtonsoft.Json;

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

            if(Model.Immagini != null)
            {
                Collection<byte[]> immagini = new Collection<byte[]>();
                foreach (var item in Model.Immagini)
                {
                    immagini.Add(Utility.Utility.Compress(item));
                }

                Model.Immagini = immagini;
            }

            if(Model.ImmaginePlanimetria != null) { 
                Collection<byte[]> planimetrie = new Collection<byte[]>();
                foreach (var item in Model.ImmaginePlanimetria)
                {
                    planimetrie.Add(Utility.Utility.Compress(item));
                }

                Model.ImmaginePlanimetria = planimetrie;
            }

            AnnunciClient annunciClient = new AnnunciClient(httpClient);
            var annuncio = await annunciClient.UpdateAnnuncioAsync(Id, Model);

            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> CreateAsync(Guid? Id)
        {
            HttpClient httpClient = new HttpClient();
            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            AnnunciClient annunciClient = new AnnunciClient(httpClient);
            //-- integrazione per modifica annuncio
            if (Id != null)
            {//caso modifica
                AnnuncioDtoOutput annuncio = await annunciClient.GetAnnuncioByIdAsync((Guid) Id);
                ViewData["Annuncio"] = annuncio;
                List<byte[]> immagini = (await annunciClient.GetImmaginiByIdAnnuncioAsync((Guid)Id)).ToList();
                ViewData["ImmaginiAnnuncio"] = immagini;
            }
            //--fine integrazione

            var classiEnergetiche = await annunciClient.GetListaClasseEnergeticaAsync();
            var tipologieProprieta = await annunciClient.GetListaTipologiaProprietaAsync();
            var tipologiaAnnunci = await annunciClient.GetListaTipologiaAnnunciAsync();
            var tipologiaRiscaldamento = await annunciClient.GetListaTipologiaRiscaldamentoAsync();
            ViewData["ListaTipologieProprieta"] = tipologieProprieta.AsEnumerable();
            ViewData["ListaTipologieAnnunci"] = tipologiaAnnunci.AsEnumerable();
            ViewData["ListaClassiEnergetiche"] = classiEnergetiche.AsEnumerable();
            ViewData["ListaTipologieRiscaldamento"] = tipologiaRiscaldamento.AsEnumerable();


            
            IEnumerable<string> ore = (new List<string>() { "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22" }).AsEnumerable();
            ViewData["ListaOre"] = ore.AsEnumerable();
            IEnumerable<string> minuti = (new List<string>() { "00", "30" }).AsEnumerable();
            ViewData["ListaMinuti"] = minuti.AsEnumerable();

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

        [HttpGet]
        [Authorize]
        public async Task<string> AppuntamentiDisponibili(string IdAnnuncio, string Giorno)
        {
            ICollection<string> orariDisponibili = new List<string>();

            if (!string.IsNullOrEmpty(IdAnnuncio) && !string.IsNullOrEmpty(Giorno))
            {
                //Creo il client e setto il Baerer Token
                HttpClient httpClient = new HttpClient();
                var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                //Ottengo la lista degli orari disponibili
                AgendaClient agendaClient = new AgendaClient(httpClient);
                int day = Int32.Parse(Giorno.Split("-")[2]);
                int month = Int32.Parse(Giorno.Split("-")[1]);
                int year = Int32.Parse(Giorno.Split("-")[0]);
                DateTime gg = new DateTime(year, month, day, 0, 0, 0);
                int i = 1;
                i++;
                orariDisponibili = await agendaClient.GetFasceDisponibiliAnnuncioByGiornoAsync(new Guid(IdAnnuncio), new DateTimeOffset(gg));
            }

            return JsonConvert.SerializeObject(orariDisponibili);
        }

        [HttpGet]
        [Authorize]
        public async Task<string> PrenotaAppuntamento(string IdAnnuncio, string Giorno, string Ora)
        {
            Appuntamento appuntamento = null;

            if (!string.IsNullOrEmpty(IdAnnuncio) && !string.IsNullOrEmpty(Giorno))
            {
                //Creo il client e setto il Baerer Token
                HttpClient httpClient = new HttpClient();
                var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                //Ottengo la lista degli orari disponibili
                AgendaClient agendaClient = new AgendaClient(httpClient);
                int day = Int32.Parse(Giorno.Split("-")[2]);
                int month = Int32.Parse(Giorno.Split("-")[1]);
                int year = Int32.Parse(Giorno.Split("-")[0]);
                //DateTime gg = new DateTime(year, month, day, Int32.Parse(Ora.Split(':')[0]), Int32.Parse(Ora.Split(':')[1]), 0);
                DateTimeOffset giornoEOra = new DateTimeOffset(year, month, day, Int32.Parse(Ora.Split(':')[0]), Int32.Parse(Ora.Split(':')[1]), 0, new TimeSpan(0, 0, 0));
                //DateTimeOffset dtOffsetGg = new DateTimeOffset(gg);

                //TimeZoneInfo utcInfo = TimeZoneInfo.Utc;

                //DateTimeOffset utcTime = TimeZoneInfo.ConvertTime(dtOffsetGg, utcInfo);
                //DateTimeOffset ggUtc = dtOffsetGg
                //    .Subtract(utcTime.Offset)
                //    .ToOffset(utcTime.Offset);

                AppuntamentoDto dto = new AppuntamentoDto()
                {
                    Data = giornoEOra,
                    IdAnnuncio = new Guid(IdAnnuncio)
                };

                appuntamento = await agendaClient.InsertAppuntamentoAsync(dto);
            }

            //return "";

            return appuntamento != null ? JsonConvert.SerializeObject(appuntamento) : "";
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
