﻿
using AppAppartamentiApiClient;
using AppAppartamenti.Api;
using AppAppartamenti.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AppAppartamenti;

namespace AppAppartamenti.Views.Account
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GoogleLogout : ContentPage
    {
        public GoogleLogout()
        {
            InitializeComponent();

            //var apiRequest =
            //    $"https://www.facebook.com/dialog/oauth?client_id={AppSetting.ClientId}&display=popup&response_type=token&redirect_uri={AppSetting.ApiEndpoint}";

            //var webView = new WebView
            //{
            //    Source = apiRequest,
            //    HeightRequest = 1
            //};

            //webView.Navigated += WebViewOnNavigated;

            //Content = webView;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            AccountClient accountClient = new AccountClient(await ApiHelper.GetApiClient());

            //Rimuovo il token
            //Api.ApiHelper.DeleteToken();
            //Api.ApiHelper.RemoveProvider();
            Api.ApiHelper.RemoveSettings();
            //Api.ApiHelper.RemoveProvider();
            //eseguo il logout.
            await accountClient.LogoutAsync();

            //torno alla pagina di login.
            Application.Current.MainPage = new NavigationPage(new Login.Login());
        }

        private async void WebViewOnNavigated(object sender, WebNavigatedEventArgs e)
        {
            string accessToken = ExtractAccessTokenFromUrl(e.Url);

            if (accessToken != "")
            {
                AccountClient accountClient = new AccountClient(await ApiHelper.GetApiClient());

                ManageInfoViewModel manageInfoViewModel = await accountClient.GetManageInfoAsync("/", true);

                if (manageInfoViewModel != null && manageInfoViewModel.Logins.Count > 0)
                {
                    //ottengo l'id dell'utente di facebook.
                    string idFacebookUser = ((UserLoginInfoViewModel)manageInfoViewModel.Logins.First()).ProviderKey;

                    var requestUrl =
                       $"https://graph.facebook.com/v2.7/{idFacebookUser}/permissions/email?&access_token={accessToken}";

                    //Elimino i permessi di facebook
                    HttpClient httpFacebookClient = new HttpClient();
                    HttpResponseMessage httpResponseMessage = await httpFacebookClient.DeleteAsync(requestUrl);

                    //Rimuovo il token 
                    Api.ApiHelper.DeleteToken();
                    Api.ApiHelper.RemoveProvider();

                    if (httpResponseMessage != null && httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        //eseguo il logout.
                        await accountClient.LogoutAsync();

                        //torno alla pagina di login.
                        Application.Current.MainPage = new NavigationPage(new Login.Login());
                    }
                }
            }
        }


        /// <summary>
        /// Metodo che estrae l'access token necessario per il logout di facebook.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string ExtractAccessTokenFromUrl(string url)
        {
            if (url.Contains("access_token") && url.Contains("&expires_in="))
            {
                var at = url.Replace($"{AppSetting.ApiEndpoint}?#access_token=", "");

                var accessToken = at.Remove(at.IndexOf("&data_access_expiration_time="));

                return accessToken;
            }

            return string.Empty;
        }
    }
}