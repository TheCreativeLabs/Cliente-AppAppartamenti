using AppAppartamentiApiClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;
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
        public const string UserInfoKey = "UserInfo_Key";
        public const string ListaTipologiaAnnuncioKey = "ListaTipologiaAnnuncio_Key";
        public const string ListaTipologiaProprietaKey = "ListaTipologiaProprieta_Key";
        public const string NotificationStatusKey = "NotificationStatus_Key";
        public const string ListaComuniKey = "ListaComuni_Key";
        public const string ListaAnnunciRecentiProprietaKey = "ListaAnnunciRecentiProprieta_Key";
        public const string ListaChatKey = "ListaChat_Key";
        public const string AndroidRegistrationToken = "AndroidRegistrationToken_Key";

        public class BearerToken
        {
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }

            [JsonProperty("token_type")]
            public string TokenType { get; set; }

            [JsonProperty("expires_in")]
            public string ExpiresIn { get; set; }

            [JsonProperty("refresh_token")]
            public string RefreshToken { get; set; }

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

                    SetToken(jsonResponse);
                }
                else
                {
                    //Se arrivo qui allora email e password sono sbagliati.
                    throw new ApplicationException("Password o Email errati.");
                }
            }
        }


        public static async Task<string> GetRefreshToken(string RefreshToken)
        {
            using (var httpClient = new HttpClient())
            {
                Uri Endpoint = new Uri($"{AppSetting.ApiEndpoint}Token");

                var tokenRequest =
                    new List<KeyValuePair<string, string>>
                        {
                        new KeyValuePair<string, string>("grant_type", "refresh_token"),
                        new KeyValuePair<string, string>("refresh_token", RefreshToken)
                        };

                HttpContent encodedRequest = new FormUrlEncodedContent(tokenRequest);

                HttpResponseMessage response = await httpClient.PostAsync(Endpoint, encodedRequest);

                if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string jsonResponse = response.Content.ReadAsStringAsync().Result;

                    SetToken(jsonResponse);

                    BearerToken token = JsonConvert.DeserializeObject<BearerToken>(jsonResponse);

                    return token.AccessToken;
                }
                else
                {
                    //Se arrivo qui allora email e password sono sbagliati.
                    throw new ApplicationException("Password o Email errati.");
                }
            }
        }


        public static void SetToken(string AccessToken)
        {
            Preferences.Set(AccessTokenKey, AccessToken);
        }

        // Ottiene il Token
        public async static Task<string> GetToken()
        {
            string accessToken = null;

            var bearerToken = Preferences.Get(AccessTokenKey,null);

            LoginProvider provider = GetProvider();

            if (Api.ApiHelper.LoginProvider.Facebook.Equals(provider) || Api.ApiHelper.LoginProvider.Google.Equals(provider)) //provider facebook o google: non serve fare refresh token. ho direttamente il token in bearerToken
            {
                accessToken = bearerToken;
            }
            else
            { //registrazione con mail: se necessario si fa refreshToken
                if (bearerToken != null && !string.IsNullOrEmpty(bearerToken))
                {
                    BearerToken token = JsonConvert.DeserializeObject<BearerToken>(bearerToken);

                    var expireDate = DateTime.Parse(token.Expires);
                    //DateTime.ParseExact(
                    //            token.Expires,
                    //            "ddd MMM dd yyyy HH:mm:ss 'GMT'",
                    //            CultureInfo.InvariantCulture);
                    if (expireDate < DateTime.Now)
                    {
                        accessToken = await GetRefreshToken(token.RefreshToken);
                    }
                    else
                    {
                        accessToken = token.AccessToken;
                    }
                }
            }


            return accessToken;
        }

        /// <summary>
        /// Rimuove il token.
        /// </summary>
        /// <returns></returns>
        public static async void DeleteToken()
        {
            Preferences.Remove(AccessTokenKey);
        }

        /// <summary>
        /// Salva nella cache se l'utente si è loggato con facebook, google o email.
        /// </summary>
        /// <param name="Provider"></param>
        public static async void SetProvider(LoginProvider Provider)
        {
            Preferences.Set(ProviderKey, Provider.ToString());

        }

        /// <summary>
        /// Restituisce se l'utente si è loggato con facebook,google o email.
        /// </summary>
        /// <returns></returns>
        public static LoginProvider GetProvider()
        {
            LoginProvider provider = LoginProvider.Email;

            if (Preferences.Get(ProviderKey, null) != null)
            {
                Enum.TryParse(Preferences.Get(ProviderKey, null), out provider);
            }

            return provider;
        }



        /// <summary>
        /// Rimuove il provider.
        /// </summary>
        /// <returns></returns>
        public static async void RemoveProvider()
        {

                Preferences.Remove(ProviderKey);

            }


            /// <summary>
            /// Restituisce il Client da usare per le chiamate all'api
            /// </summary>
            /// <returns></returns>
            public  static async Task<HttpClient> GetApiClient()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await Api.ApiHelper.GetToken());
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

        public static async Task<UserInfoDto> GetUserInfo()
        {
            UserInfoDto userInfo = null;

            if(Preferences.Get(UserInfoKey, null) != null)
            {
                userInfo = JsonConvert.DeserializeObject<UserInfoDto>(Preferences.Get(UserInfoKey, null));
            }

            if (userInfo == null)
            {
                AccountClient amiciClient = new AccountClient(await ApiHelper.GetApiClient());
                userInfo = await amiciClient.GetCurrentUserInfoAsync();
                Preferences.Set(UserInfoKey, JsonConvert.SerializeObject(userInfo));
            }

            return userInfo;
        }

        public static void RemoveUserInfo()
        {
            Preferences.Remove(UserInfoKey);

        }

        public static void SetNotificationStatus(bool IsEnabled)
        {
           Preferences.Set(NotificationStatusKey, IsEnabled.ToString());
        }

        public static async Task<bool> GetNotificationStatus()
        {
            bool notificationStatus = false;

            var notification = Preferences.Get(NotificationStatusKey, null);

            if(notification != null)
            {
                notificationStatus = bool.Parse(notification);
            }

            return notificationStatus;
        }

        public static void RemoveNotificationStatus()
        {
            Preferences.Remove(NotificationStatusKey);
        }

        /// <summary>
        /// Rimuove tutte le setting
        /// </summary>
        public static void RemoveSettings()
        {
            Preferences.Clear();
        }


        public static async Task<List<TipologiaAnnuncio>> GetListaTipologiaAnnuncio()
        {
            List<TipologiaAnnuncio> listTipologiaAnnuncio = new List<TipologiaAnnuncio>();

            if (Preferences.Get(ListaTipologiaAnnuncioKey, null) != null)
            {
                listTipologiaAnnuncio = JsonConvert.DeserializeObject<List<TipologiaAnnuncio>>(Preferences.Get(ListaTipologiaAnnuncioKey, null));
            }

            if (listTipologiaAnnuncio.Any() == false)
            {
                AnnunciClient annunciClient = new AnnunciClient(await ApiHelper.GetApiClient());
                listTipologiaAnnuncio = (await annunciClient.GetListaTipologiaAnnunciAsync()).ToList();
                Preferences.Set(ListaTipologiaAnnuncioKey, JsonConvert.SerializeObject(listTipologiaAnnuncio));
            }

            return listTipologiaAnnuncio;
        }

        public static async Task<List<TipologiaProprieta>> GetListaTipologiaProprieta()
        {
            List<TipologiaProprieta> listTipologiaProprieta = new List<TipologiaProprieta>();

            if (Preferences.Get(ListaTipologiaProprietaKey, null) != null)
            {
                listTipologiaProprieta = JsonConvert.DeserializeObject<List<TipologiaProprieta>>(Preferences.Get(ListaTipologiaProprietaKey, null));
            }

            if (listTipologiaProprieta.Any() == false)
            {
                AnnunciClient annunciClient = new AnnunciClient(await ApiHelper.GetApiClient());
                listTipologiaProprieta = (await annunciClient.GetListaTipologiaProprietaAsync()).ToList();
                Preferences.Set(ListaTipologiaProprietaKey, JsonConvert.SerializeObject(listTipologiaProprieta));
            }

            return listTipologiaProprieta;
        }

        public static async Task<List<AnnunciDtoOutput>> GetListaAnnunciRecenti(bool RefreshData)
        {
            List<AnnunciDtoOutput> listaAnnunci = new List<AnnunciDtoOutput>();


            if (!RefreshData && Preferences.Get(ListaAnnunciRecentiProprietaKey, null) != null)
            {
                listaAnnunci = JsonConvert.DeserializeObject<List<AnnunciDtoOutput>>(Preferences.Get(ListaAnnunciRecentiProprietaKey, null));
            }

            if (listaAnnunci.Any() == false)
            {
                AnnunciClient annunciClient = new AnnunciClient(await Api.ApiHelper.GetApiClient());
                listaAnnunci = (await annunciClient.GetRicercheRecentiCurrentAsync()).ToList();
                Preferences.Set(ListaAnnunciRecentiProprietaKey, JsonConvert.SerializeObject(listaAnnunci));
            }

            return listaAnnunci;
        }

        public static async Task<List<ChatListDtoOutput>> GetListaMessaggi(bool RefreshData)
        {
            List<ChatListDtoOutput> listaChat = new List<ChatListDtoOutput>();

            if (!RefreshData && Preferences.Get(ListaChatKey, null) != null)
            {
                listaChat = JsonConvert.DeserializeObject<List<ChatListDtoOutput>>(Preferences.Get(ListaChatKey, null));
            }

            if (listaChat.Any() == false)
            {
                MessaggiClient messaggiClient = new MessaggiClient(await Api.ApiHelper.GetApiClient());
                listaChat = (await messaggiClient.GetChatListAsync()).ToList();
                Preferences.Set(ListaChatKey, JsonConvert.SerializeObject(listaChat));
            }

            return listaChat;
        }


        public static async Task<ICollection<ComuneDto>> GetListaComuni(string ComuneDesc)
        {
            ICollection<ComuneDto> listaComuni = new Collection<ComuneDto>();

            if (!String.IsNullOrEmpty(ComuneDesc))
            {
                if (ComuneDesc.Length < 3)
                {
                    Preferences.Remove(ListaComuniKey);
                }
                else if(ComuneDesc.Length == 3)
                {
                    AnnunciClient annunciClient = new AnnunciClient(await Api.ApiHelper.GetApiClient());
                    var lista = await annunciClient.GetListaComuniAsync(ComuneDesc);
                    if (lista.Any())
                    {
                        Preferences.Set(ListaComuniKey, JsonConvert.SerializeObject(lista));
                    }
                }
                else
                {
                  if( Preferences.Get(ListaComuniKey, null) != null)
                    {

                        listaComuni = await FiltraLista(ComuneDesc);
                    }
                }
            }
            else{
                Preferences.Remove(ListaComuniKey);
            }

            return listaComuni;
        }

        private static Task<List<ComuneDto>> FiltraLista(string ComuneDesc)
        {
            var comuni = JsonConvert.DeserializeObject<ICollection<ComuneDto>>(Preferences.Get(ListaComuniKey, null));

            var list = comuni.Where(x => x.NomeComune.ToUpper().StartsWith(ComuneDesc.ToUpper())).ToList();
            
            return Task.FromResult<List<ComuneDto>>(list);
        }

        public static void SetFirebaseToken(string Token)
        {
            Preferences.Set(AndroidRegistrationToken, Token);
        }

        // Ottiene il Token
        public async static Task<string> GetFirebaseToken()
        {
            string Token = Preferences.Get(AndroidRegistrationToken, null);

            return Token;
        }

        /// <summary>
        /// Rimuove il token.
        /// </summary>
        /// <returns></returns>
        public static async void DeleteFirebaseToken()
        {
            Preferences.Remove(AndroidRegistrationToken);
        }

    }





}
