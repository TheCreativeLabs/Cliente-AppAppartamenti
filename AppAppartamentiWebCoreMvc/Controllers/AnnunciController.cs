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
using AppAppartamentiWebCoreMvc.AppAppartamentiApiClient;
using AppAppartamentiWebCoreMvc.Extensions;
using AppAppartamentiWebCoreMvc.Utility;
using static AppAppartamentiWebCoreMvc.Utility.Utility;

namespace AppAppartamentiWebCoreMvc.Controllers
{
    [Authorize]
    public class AnnunciController : Controller
    {
        

        private readonly ILogger<HomeController> _logger;
        private readonly int pageSize = 5;
        //ICollection<AnnunciDtoOutput> listaAnnunci;

        public AnnunciController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult ListAsync()
        {
            return View();
        }

        public async Task<ActionResult> RefreshListAsync(int ListPage, int? Order)
        {

            OrderBy? orderBy = null;
            if(Order != null)
            {
                 orderBy = (OrderBy)Order;
            }

            HttpClient httpClient = new HttpClient();
            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            AnnunciClient annunciClient = new AppAppartamentiApiClient.AnnunciClient(httpClient);
            var FilterModel = HttpContext.Session.GetObject<FilterModalViewModel>(Constants.FilterModalKey);

            var annunci = await annunciClient.GetAnnunciAsync(ListPage, pageSize, FilterModel.IdTipologiaAnnuncio,FilterModel.IdTipologiaProprieta,FilterModel.CodiceComune,
                FilterModel.PrezzoMin,FilterModel.PrezzoMax,FilterModel.DimensioneMin, FilterModel.DimensioneMax,FilterModel.NumeroCamereLetto,
                FilterModel.NumeroBagni, FilterModel.NumeroCucine, FilterModel.NumeroPostiAuto, FilterModel.NumeroGarage, FilterModel.NumeroAltreStanze, FilterModel.Giardino,null,FilterModel.Cantina,
                FilterModel.Piscina,FilterModel.Ascensore, FilterModel.Condizionatori,null,null,null, null); //todo penultimo parametro condizionatori

            ICollection<AnnunciDtoOutput> listaAnnunci;
            if (ListPage == 1)
            {
                listaAnnunci = annunci;
            } else
            {
                listaAnnunci = HttpContext.Session.GetObject<ICollection<AnnunciDtoOutput>>(Constants.ListaAnnunciKey);
                foreach (AnnunciDtoOutput a in annunci)
                {
                    listaAnnunci.Add(a);
                }
            }
            HttpContext.Session.SetObject(Constants.ListaAnnunciKey, listaAnnunci);

            //if(listaAnnunci == null)
            //{
            //    listaAnnunci = annunci;
            //} else
            //{
            //    foreach(AnnunciDtoOutput a in annunci)
            //    {
            //        listaAnnunci.Add(a);
            //    }
            //}

            ViewData["CurrentListPage"] = ListPage;

            return PartialView("_AnnunciPartial", listaAnnunci);
        }

        public async Task<IActionResult> DetailAsync(Guid Id, bool Preferiti)
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
            ViewData["Preferiti"] = Preferiti;
            return View(annuncio);
        }

        public async Task<IActionResult> DetailImmaginiAsync(Guid Id)
        {
            ViewData["Id"] = Id;
            return View();
        }

        public async Task<List<string>> ImmaginiAnnuncioAsync(Guid IdAnnuncio) {
            HttpClient httpClient = new HttpClient();
            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            AppAppartamentiApiClient.AnnunciClient annunciClient = new AppAppartamentiApiClient.AnnunciClient(httpClient);
            List<byte[]> immaginiAnnuncio = (await annunciClient.GetImmaginiByIdAnnuncioAsync(IdAnnuncio)).ToList();

            List<string> imagesSrc = new List<string>();
            foreach(var imm in immaginiAnnuncio)
            {
                var base64 = Convert.ToBase64String(imm);
                var imgSrc = String.Format("data:image/gif;base64,{0}", base64);
                imagesSrc.Add(imgSrc);
            }

            return imagesSrc;
        }

        public async Task<ActionResult> RecentAds()
        {
            HttpClient httpClient = new HttpClient();
            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            AnnunciClient annunciClient = new AppAppartamentiApiClient.AnnunciClient(httpClient);
            var recentAds= await annunciClient.GetRicercheRecentiCurrentAsync();
            return PartialView("~/Views/Annunci/_RecentAdPartial.cshtml", recentAds);
        }

        public async Task<ActionResult> SignalAdReason()
        {
            HttpClient httpClient = new HttpClient();
            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            AnnunciClient annunciClient = new AppAppartamentiApiClient.AnnunciClient(httpClient);
            var list = await annunciClient.GetListaMotiviSegnalazioneAsync();
            return PartialView("~/Views/Annunci/_SegnalaPartial.cshtml", list);
        }

        public async void ReportAd(Guid Id,Guid ReportReasonId,string ReportMessage)
        {
            HttpClient httpClient = new HttpClient();
            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            AnnunciClient annunciClient = new AppAppartamentiApiClient.AnnunciClient(httpClient);

            SegnalazioneDtoInput segnalazioneDtoInput = new SegnalazioneDtoInput()
            {
                IdAnnuncio = Id,
                IdMotivoSegnalazione = ReportReasonId,
                TestoSegnalazione = ReportMessage
            };

           await annunciClient.InsertSegnalazioneAsync(segnalazioneDtoInput);
        }
    }
}
