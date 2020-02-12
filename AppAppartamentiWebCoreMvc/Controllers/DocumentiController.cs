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
using AppAppartamentiWebCoreMvc.Extensions;
using AppAppartamentiWebCoreMvc.Utility;
using static AppAppartamentiWebCoreMvc.Utility.Utility;

namespace AppAppartamentiWebCoreMvc.Controllers
{
    public class DocumentiController : Controller
    {
        

        private readonly ILogger<HomeController> _logger;

        public DocumentiController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        
        public async Task<FileResult> DetailAsync(Guid Id)
        {

            HttpClient httpClient = new HttpClient();
            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            AppAppartamentiApiClient.AnnunciClient annunciClient = new AppAppartamentiApiClient.AnnunciClient(httpClient);
            var documento = await annunciClient.GetDownloadDocumentoPdfAsync(Id);

            if(documento.Documento != null)
            {
                return File(documento.Documento, "application/pdf");
            }
            return null;

            //return View(documento);
        }

    }
}
