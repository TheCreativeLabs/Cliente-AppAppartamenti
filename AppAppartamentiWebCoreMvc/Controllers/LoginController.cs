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
using static AppAppartamenti.Api.ApiHelper;
using Microsoft.AspNetCore.Http;
using System.Net.Http;

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
            BearerToken bearerToken = await ApiHelper.SetTokenAsync(_configuration.GetValue<string>("MySetting:ApiEndpoint"), Email, Password);

            if (bearerToken != null && !string.IsNullOrEmpty(bearerToken.AccessToken))
            {

                var claims = new List<Claim>
                {
                    new Claim("user", Email),
                    new Claim("token", bearerToken.AccessToken)
                };

                await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", "user", "role")));
            }

            return Redirect("/");
        }

        [HttpPost]
        public async Task<IActionResult> SignUpAsync(RegisterUserBindingModel Model)
        {
            AccountClient accountClient = new AccountClient(new System.Net.Http.HttpClient());

            Model.BirthName = Model.Name;

            await accountClient.RegisterAsync(Model);

            return Redirect("/");
        }

        public async Task<IActionResult> LogOutAsync()
        {
            await HttpContext.SignOutAsync();

            return Redirect("/");
        }
    }
}
