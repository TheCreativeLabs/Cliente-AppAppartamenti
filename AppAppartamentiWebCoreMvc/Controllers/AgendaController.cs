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
    public class AgendaController : Controller
    {
        IConfiguration _configuration;

        public AgendaController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> DetailAsync()
        {
            return View();
        }

        public async Task<ActionResult> GetAppointmentList(DateTime SelectedDate)
        {
            HttpClient httpClient = new HttpClient();
            AgendaClient agendaClient = new AgendaClient(httpClient);
            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var listaAppuntamenti =  await agendaClient.GetAgendaCurrentByGiornoAsync(SelectedDate);

            return PartialView("_AppuntamentiPartial", listaAppuntamenti.ToList());
        }

        public async Task<ActionResult> GetAppointmentDetail(Guid SelectedAppointment)
        {
            HttpClient httpClient = new HttpClient();
            AgendaClient agendaClient = new AgendaClient(httpClient);
            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var appuntamento = await agendaClient.GetAppuntamentoByIdAsync(SelectedAppointment);

            return PartialView("_AppuntamentiDetailPartial", appuntamento);
        }

        public async void Delete(Guid Id)
        {
            HttpClient httpClient = new HttpClient();
            AgendaClient agendaClient = new AgendaClient(httpClient);
            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            await agendaClient.DeleteAppuntamentoAsync(Id);
        }

        public async void Accept(Guid Id)
        {
            HttpClient httpClient = new HttpClient();
            AgendaClient agendaClient = new AgendaClient(httpClient);
            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            await agendaClient.ConfermaAppuntamentoAsync(Id);
        }

        [HttpGet]
        public async Task<ActionResult> AppuntamentiDisponibili(string IdAnnuncio, string Giorno)
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
                orariDisponibili = await agendaClient.GetFasceDisponibiliAnnuncioByGiornoAsync(new Guid(IdAnnuncio), new DateTimeOffset(gg));
            }

            return PartialView("~/Views/Annunci/_FasceOrariePartial.cshtml", orariDisponibili);
        }
    }
}
