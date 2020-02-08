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
    public class AdminController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public AdminController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> ListToApproveAsync()
        {
            HttpClient httpClient = new HttpClient();
            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            AdminClient adminController = new AdminClient(httpClient);
            var annunci = await adminController.GetAnnunciDaApprovareAsync(1,1000);

            return View(annunci.AsEnumerable());
        }

        public async Task<IActionResult> ListReportedAsync()
        {
            HttpClient httpClient = new HttpClient();
            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            AdminClient adminController = new AdminClient(httpClient);
            var annunci = await adminController.GetListaAnnunciSegnalatiAsync(1, 1000);
            return View(annunci.AsEnumerable());
        }

        public async Task<IActionResult> ApproveAdAsync(Guid Id)
        {
            HttpClient httpClient = new HttpClient();
            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            AdminClient adminController = new AdminClient(httpClient);
            await adminController.ApprovaAnnuncioAsync(Id);
            return RedirectToAction("ListToApproveAsync");
        }

        public async Task<IActionResult> DisapproveAdAsync(Guid Id)
        {
            HttpClient httpClient = new HttpClient();
            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            AdminClient adminController = new AdminClient(httpClient);
            await adminController.DisapprovaAnnuncioAsync(Id);
            return RedirectToAction("ListToApproveAsync");
        }

        public async Task<IActionResult> DisapproveReportedAdAsync(Guid Id)
        {
            HttpClient httpClient = new HttpClient();
            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            AdminClient adminController = new AdminClient(httpClient);
            await adminController.DisapprovaAnnuncioAsync(Id);
            return RedirectToAction("ListReportedAsync");
        }

        public async Task<IActionResult> ListSellDocumentAsync()
        {
            HttpClient httpClient = new HttpClient();
            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            AnnunciClient annunciClient = new AnnunciClient(httpClient);
            var docs = await annunciClient.GetDocumentiVenditaAsync();

            return View(docs);
        }

        public async Task<IActionResult> DetailAsync(Guid Id,bool Reported)
        {
            HttpClient httpClient = new HttpClient();
            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            AnnunciClient annunciClient = new AnnunciClient(httpClient);
            var annuncio = await annunciClient.GetAnnuncioByIdAsync(Id);

            var classiEnergetiche = await annunciClient.GetListaClasseEnergeticaAsync();
            ViewData["ListaClassiEnergetiche"] = classiEnergetiche.AsEnumerable();

            AccountClient accountClient = new AccountClient(httpClient);
            UserInfoDto userInfo = await accountClient.GetUserInfoAsync(annuncio.IdUtente.Value);

            ViewData["Reported"] = Reported;

            return View(annuncio);
        }

        public async Task<IActionResult> ListBuyDocumentAsync()
        {
            HttpClient httpClient = new HttpClient();
            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            AnnunciClient annunciClient = new AnnunciClient(httpClient);
            var docs = await annunciClient.GetDocumentiAcquistoAsync();

            return View(docs);
        }

        public async Task<ActionResult> DeleteBuyDocument(Guid Id)
        {
            HttpClient httpClient = new HttpClient();
            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            AdminClient adminController = new AdminClient(httpClient);
            await adminController.DeleteDocumentoAsync(Id);

            return RedirectToAction("ListBuyDocumentAsync");
        }

        public async Task<ActionResult> DeleteSellDocument(Guid Id)
        {
            HttpClient httpClient = new HttpClient();
            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            AdminClient adminController = new AdminClient(httpClient);
            await adminController.DeleteDocumentoAsync(Id);
            return RedirectToAction("ListSellDocumentAsync");
        }

        [HttpGet]
        public async Task<IActionResult> BuyDocumentInsertAsync()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SellDocumentInsertAsync()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BuyDocumentInsertAsync(DocumentoDto DocumentoDtoInput)
        {
            HttpClient httpClient = new HttpClient();
            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            AdminClient adminController = new AdminClient(httpClient);
            await adminController.InsertDocumentoAsync(DocumentoDtoInput);
            return RedirectToAction("ListBuyDocumentAsync");
        }

        [HttpPost]
        public async Task<IActionResult> SellDocumentInsertAsync(DocumentoDto DocumentoDtoInput)
        {
            HttpClient httpClient = new HttpClient();
            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            AdminClient adminController = new AdminClient(httpClient);
            await adminController.InsertDocumentoAsync(DocumentoDtoInput);
            return RedirectToAction("ListSellDocumentAsync");
        }


    }
}
