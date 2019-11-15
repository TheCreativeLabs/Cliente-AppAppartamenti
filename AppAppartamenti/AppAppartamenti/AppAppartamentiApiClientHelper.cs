using AppAppartamentiApiClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppAppartamenti.Api
{
    public static class ApiHelper
    {
        public enum LoginProvider
        {
            Facebook = 0,
            Google = 1,
            Email = 2
        }

        public const string AccessTokenKey = "Access_Token";
        public const string ProviderKey = "Provider_Key";

        public class BearerToken
        {
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }

            [JsonProperty("token_type")]
            public string TokenType { get; set; }

            [JsonProperty("expires_in")]
            public string ExpiresIn { get; set; }

            [JsonProperty("userName")]
            public string UserName { get; set; }

            [JsonProperty(".issued")]
            public string Issued { get; set; }

            [JsonProperty(".expires")]
            public string Expires { get; set; }
        }

        /// <summary>
        /// Setta il token
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public static async Task SetTokenAsync(string Username, string Password)
        {
            using (var httpClient = new HttpClient())
            {
                Uri Endpoint = new Uri($"{AppSetting.ApiEndpoint}Token");

                var tokenRequest =
                    new List<KeyValuePair<string, string>>
                        {
                        new KeyValuePair<string, string>("grant_type", "password"),
                        new KeyValuePair<string, string>("username", Username),
                        new KeyValuePair<string, string>("password", Password)
                        };

                HttpContent encodedRequest = new FormUrlEncodedContent(tokenRequest);

                HttpResponseMessage response = await httpClient.PostAsync(Endpoint, encodedRequest);

                if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string jsonResponse = response.Content.ReadAsStringAsync().Result;
                    BearerToken token = JsonConvert.DeserializeObject<BearerToken>(jsonResponse);

                    Application.Current.Properties[AccessTokenKey] = token.AccessToken;
                }
                else
                {
                    //Se arrivo qui allora email e password sono sbagliati.
                    throw new ApplicationException("Password o Email errati.");
                }
            }
        }

        // Ottiene il Token
        public static string GetToken()
        {
            string accessToken = null;

            if (Application.Current.Properties.ContainsKey(AccessTokenKey))
            {
                accessToken = Application.Current.Properties[AccessTokenKey].ToString();

                //TODO: controllare la validità del token.
            }

            return accessToken;
        }

        /// <summary>
        /// Rimuove il token.
        /// </summary>
        /// <returns></returns>
        public static string DeleteToken()
        {
            string accessToken = null;

            if (Application.Current.Properties.ContainsKey(AccessTokenKey))
            {


                Application.Current.Properties.Remove(AccessTokenKey);
            }

            return accessToken;
        }

        /// <summary>
        /// Salva nella cache se l'utente si è loggato con facebook, google o email.
        /// </summary>
        /// <param name="Provider"></param>
        public static void SetProvider(LoginProvider Provider)
        {
            Application.Current.Properties[ProviderKey] = Provider;
        }

        /// <summary>
        /// Restituisce se l'utente si è loggato con facebook,google o email.
        /// </summary>
        /// <returns></returns>
        public static LoginProvider GetProvider()
        {
            LoginProvider provider = LoginProvider.Email;

            if (Application.Current.Properties.ContainsKey(ProviderKey))
            {
                provider = (LoginProvider)Application.Current.Properties[ProviderKey];
            }

            return provider;
        }



        /// <summary>
        /// Rimuove il provider.
        /// </summary>
        /// <returns></returns>
        public static string RemoveProvider()
        {
            string accessToken = null;

            if (Application.Current.Properties.ContainsKey(ProviderKey))
            {
                Application.Current.Properties.Remove(ProviderKey);
            }

            return accessToken;
        }

        /// <summary>
        /// Restituisce il Client da usare per le chiamate all'api
        /// </summary>
        /// <returns></returns>
        public static HttpClient GetApiClient()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Api.ApiHelper.GetToken());
            return httpClient;
        }

        /// <summary>
        /// Metodo per registrarsi
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Password"></param>
        /// <param name="ConfermaPassword"></param>
        /// <returns></returns>
        public static async Task RegisterAsync(string Email, string Password, string ConfermaPassword)
        {
            using (var httpClient = new HttpClient())
            {
                AccountClient accountClient = new AccountClient(httpClient);

                //Creo il modello dei dati per la registrazione
                RegisterUserBindingModel registerBindingModel = new RegisterUserBindingModel()
                {
                    Email = Email,
                    Password = Password,
                    ConfirmPassword = ConfermaPassword

                };

                try
                {
                    await accountClient.RegisterAsync(registerBindingModel);
                }
                catch (ApiException ex)
                {
                    throw new Exception("Si è verificato un errore" + ex.StatusCode);
                }
            }
        }
    }

}
