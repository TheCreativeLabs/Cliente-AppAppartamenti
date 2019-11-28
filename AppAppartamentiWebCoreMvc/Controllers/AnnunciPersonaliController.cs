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
            //HttpClient httpClient = new HttpClient();
            ////var accessToken = HttpContext.Session.GetString("ACCESS_TOKEN");
            //var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            //httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            //AppAppartamentiApiClient.AnnunciClient annunciClient = new AppAppartamentiApiClient.AnnunciClient(httpClient);
            //var annunci = await annunciClient.GetAnnunciByUserAsync();

            return View();// annunci.AsEnumerable());
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
