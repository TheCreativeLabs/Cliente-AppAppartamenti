using AppAppartamenti.Api;
using AppAppartamenti.Views;
using AppAppartamentiApiClient;
using AppAppartamenti.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAppartamenti.Views.Account
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FacebookLogin : ContentPage
    {
        String ApiRequest;

        public FacebookLogin()
        {
            InitializeComponent();

            //ottengo l'url da chiamare per l'autenticazione su Facebook
            AccountClient accountClient = new AccountClient(new System.Net.Http.HttpClient());
            ExternalLoginViewModel externalLoginViewModel = accountClient.GetExternalLoginsAsync("/", true).Result.ToList()[1];
            string apiRequest = $"{AppSetting.ApiEndpoint.Replace(".com/",".com")}{externalLoginViewModel.Url}";
            apiRequest = apiRequest.Replace("www.", "");
            ApiRequest = apiRequest;
            //mostro l'url nella pagina
            var webView = new WebView
            {
                Source = apiRequest,
                HeightRequest = 1
            };

            webView.Navigated += WebViewOnNavigated;

            Content = webView;
        }



        private async void WebViewOnNavigated(object sender, WebNavigatedEventArgs e)
        {
            var accessToken = ExtractAccessTokenFromUrl(e.Url);
            
            if (accessToken != "")
            {
                //Salva il token nelle properties
                Application.Current.Properties[Api.ApiHelper.AccessTokenKey] = accessToken;

                //Salvo il nelle properties che l'utente ha fatto accesso con Facebook
                Api.ApiHelper.SetProvider(ApiHelper.LoginProvider.Facebook);

                //creo il client e setto il Baerer Token
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                //Ottengo le info sull'utente connesso per verificare se è già registrato
                AccountClient accountClient = new AccountClient(httpClient);
                UserInfoViewModel userInfoViewModel = await accountClient.GetUserInfoAsync();

                //TODO: Controllare che la risposta del server sia OK

                //Se l'utente non è registrato allora lo registro
                if (userInfoViewModel != null && userInfoViewModel.HasRegistered.HasValue && userInfoViewModel.HasRegistered.Value == false)
                {
                    RegisterExternalBindingModel registerExternalBindingModel = new RegisterExternalBindingModel()
                    {
                        Email = userInfoViewModel.Email
                    };

                    //registro l'utente
                    await accountClient.RegisterExternalAsync(registerExternalBindingModel);

                    var webView = new WebView
                    {
                        Source = ApiRequest,
                        HeightRequest = 1
                    };

                    webView.Navigated += WebViewOnNavigated;

                    Content = webView;
                }
                else if (userInfoViewModel == null || userInfoViewModel.HasRegistered.HasValue == false) // ci sono stati degli errori
                {
                    //TODO mostrare pagina di errore
                }
                else if(userInfoViewModel.HasRegistered.HasValue && userInfoViewModel.HasRegistered.Value == true && userInfoViewModel.LoginProvider.ToUpper() == "FACEBOOK") //utente già registrato con FB: accede
                {
                    //Application.Current.MainPage = new NavigationPage( new MainPage());
                    Application.Current.MainPage = new MainPage();
                }
                else //l'utente è già registrato ma NON con facebook: è registrato con Google o con la mail, quindi non può accedere
                {
                    await DisplayAlert("Attenzione", "Esiste già un account registrato con la tua mail.", "OK");
                    await Navigation.PopAsync();
                }
            }
        }

        /// <summary>
        /// Estrae il token dall'url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string ExtractAccessTokenFromUrl(string url)
        {
            if (url.Contains("access_token") && url.Contains("&expires_in="))
            {
                var at = url.Replace($"{AppSetting.ApiEndpoint}#access_token=", "");

                if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                {
                    at = url.Replace($"{AppSetting.ApiEndpoint}#access_token=", "");
                }

                var accessToken = at.Remove(at.IndexOf("&token_type=bearer&expires_in="));

                return accessToken;
            }

            return string.Empty;
        }
    }
}