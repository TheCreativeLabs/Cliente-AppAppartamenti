﻿using System;
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
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace AppAppartamentiWebCoreMvc.Controllers
{
    public class LoginController : Controller
    {
        IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public const string ApiEndpoint = "https://appappartamentiapi.appregaliapitest.com/";

        [HttpPost]
        [Authorize]
        public async Task<string> GetUserInfoAsync()
        {
            HttpClient httpClient = new HttpClient();
            var accessToken = User.Claims.Where(x => x.Type == "token").Select(x => x.Value).FirstOrDefault();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            //ottengo l'url da chiamare per l'autenticazione su Facebook
            AccountClient accountClient = new AccountClient(httpClient);

            UserInfoDto userInfoDto = await accountClient.GetCurrentUserInfoAsync();

            return JsonConvert.SerializeObject(userInfoDto);
        }

        [HttpPost]
        public string GetFacebookExternalLogin()
        {
            //ottengo l'url da chiamare per l'autenticazione su Facebook
            AccountClient accountClient = new AccountClient(new System.Net.Http.HttpClient());

            ExternalLoginViewModel externalLoginViewModel = accountClient.GetExternalLoginsAsync("https://localhost:5001/Home/FacebookLogin", true).Result.ToList()[1];

            string apiRequest = $"{ApiEndpoint.Replace(".com/", ".com")}{externalLoginViewModel.Url}";
            apiRequest = apiRequest.Replace("www.", "");


            HttpContext.Session.SetString("ApiUrl", apiRequest);
            return apiRequest;
        }


        [HttpPost]
        public async Task<string> FacebookLoginResultAsync(string token)
        {
            //creo il client e setto il Baerer Token
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            //Ottengo le info sull'utente connesso per verificare se è già registrato
            AccountClient accountClient = new AccountClient(httpClient);
            UserInfoViewModel userInfoViewModel = await accountClient.GetUserInfoAsync();

            if (userInfoViewModel.HasRegistered.HasValue && userInfoViewModel.HasRegistered.Value == false)
            {
                RegisterExternalBindingModel registerExternalBindingModel = new RegisterExternalBindingModel()
                {
                    Email = userInfoViewModel.Email
                };

                //registro l'utente
                await accountClient.RegisterExternalAsync(registerExternalBindingModel);

                return JsonConvert.SerializeObject(HttpContext.Session.GetString("ApiUrl"));
            }
            else
            {
                var claims = new List<Claim>
                {
                    new Claim("user", userInfoViewModel.Email),
                    new Claim("token", token),
                    new Claim("provider","Facebook")
                };

                await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", "user", "role")));

                return JsonConvert.SerializeObject("");
            }
        }

        [HttpPost]
        public string GetGoogleExternalLogin()
        {
            //ottengo l'url da chiamare per l'autenticazione su Facebook
            AccountClient accountClient = new AccountClient(new System.Net.Http.HttpClient());

            ExternalLoginViewModel externalLoginViewModel = accountClient.GetExternalLoginsAsync("https://localhost:5001/Home/FacebookLogin", true).Result.ToList()[0];

            string apiRequest = $"{ApiEndpoint.Replace(".com/", ".com")}{externalLoginViewModel.Url}";
            apiRequest = apiRequest.Replace("www.", "");


            HttpContext.Session.SetString("ApiUrl", apiRequest);
            return apiRequest;
        }


        [HttpPost]
        public async Task<string> GoogleLoginResultAsync(string token)
        {
            //creo il client e setto il Baerer Token
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            //Ottengo le info sull'utente connesso per verificare se è già registrato
            AccountClient accountClient = new AccountClient(httpClient);
            UserInfoViewModel userInfoViewModel = await accountClient.GetUserInfoAsync();

            if (userInfoViewModel.HasRegistered.HasValue && userInfoViewModel.HasRegistered.Value == false)
            {
                RegisterExternalBindingModel registerExternalBindingModel = new RegisterExternalBindingModel()
                {
                    Email = userInfoViewModel.Email
                };

                //registro l'utente
                await accountClient.RegisterExternalAsync(registerExternalBindingModel);

                return JsonConvert.SerializeObject(HttpContext.Session.GetString("ApiUrl"));
            }
            else
            {
                var claims = new List<Claim>
                {
                    new Claim("user", userInfoViewModel.Email),
                    new Claim("token", token),
                    new Claim("provider","Google")
                };

                await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", "user", "role")));

                return JsonConvert.SerializeObject("");
            }
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