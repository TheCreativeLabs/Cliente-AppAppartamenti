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
    public class MessaggiController : Controller
    {
        IConfiguration _configuration;

        public MessaggiController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> DetailAsync()
        {
            return View();
        }

        public async Task<IActionResult> ListAsync()
        {
            HttpClient httpClient = new HttpClient();
            MessaggiClient messaggiClient = new MessaggiClient(httpClient);
            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            var list = await messaggiClient.GetChatListAsync();
            return View(list.ToList());
        }

        public async Task<ActionResult> GetChatDetail(Guid IdChat)
        {
            HttpClient httpClient = new HttpClient();
            MessaggiClient messaggiClient = new MessaggiClient(httpClient);
            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var chat = await messaggiClient.GetChatAsync(IdChat,null,null);

            return PartialView("_ChatDetailPartial", chat);
        }

        [HttpPost]
        public async void Send(Guid IdChat, Guid IdPersonToChat, string Message)
        {
            HttpClient httpClient = new HttpClient();
            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            MessaggiClient messaggiClient = new MessaggiClient(httpClient);

            await messaggiClient.InsertMessaggioAsync(IdChat, IdPersonToChat, Message);
        }
    }
}
