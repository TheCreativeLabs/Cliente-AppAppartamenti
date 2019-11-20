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
using AppAppartamentiApiClient;

namespace AppAppartamentiWebCoreMvc.Controllers
{
    public class LoginController : Controller
    {
        IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> AccediAsync(string Email, string Password)
        {
            var token = await ApiHelper.SetTokenAsync(_configuration.GetValue<string>("MySetting:ApiEndpoint"), Email, Password);

            var claims = new List<Claim>
            {
                new Claim("user", Email),
            };

            await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", "user", "role")));

            return Redirect("/");
        }

        [HttpPost]
        public async Task<IActionResult> SignUpAsync(string Nome, string Cognome, DateTime DataNascita, string Email, string Password, string ConfermaPassword)
        {
            AccountClient accountClient = new AccountClient(new System.Net.Http.HttpClient());

            RegisterUserBindingModel registerUserBindingModel = new RegisterUserBindingModel()
            {
                Name = Nome,
                BirthName = Nome,
                Surname = Cognome,
                DataNascita = DataNascita,
                Password = Password,
                ConfirmPassword = ConfermaPassword,
                ImmagineProfilo = null,
                Email = Email
            };

            await accountClient.RegisterAsync(registerUserBindingModel);

            return Redirect("/");
        }

        public async Task<IActionResult> LogOutAsync()
        {
            await HttpContext.SignOutAsync();

            return Redirect("/");
        }

    }
}
