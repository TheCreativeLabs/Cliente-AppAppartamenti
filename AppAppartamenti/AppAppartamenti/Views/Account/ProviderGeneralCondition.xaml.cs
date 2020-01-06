using System;
using System.Collections.Generic;
using System.Net.Http;
using AppAppartamenti.Api;
using AppAppartamenti.Views.GeneralCondition;
using AppAppartamentiApiClient;
using Xamarin.Forms;

namespace AppAppartamenti.Views.Account
{
    public partial class ProviderGeneralCondition : ContentPage
    {
        UserInfoViewModel UserInfo;
        string ApiRequest;
        string Token;

        public ProviderGeneralCondition(UserInfoViewModel UserInfoParam, string ApiRequestParam,string accessToken)
        {
            InitializeComponent();
            UserInfo = UserInfoParam;
            ApiRequest = ApiRequestParam;
            Token = accessToken;
        }

        private async void btnRegistrati_Clicked(object sender, EventArgs e)
        {
            if (!chkCondition.IsChecked)
            {
                await DisplayAlert("Attenzione", "E' necessario accettare le condizioni di utilizzo.", "OK");
                return;
            }

            RegisterExternalBindingModel registerExternalBindingModel = new RegisterExternalBindingModel()
            {
                Email = UserInfo.Email
            };

            //creo il client e setto il Baerer Token
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            AccountClient accountClient = new AccountClient(httpClient);
            await accountClient.RegisterExternalAsync(registerExternalBindingModel);

            var webView = new WebView
            {
                Source = ApiRequest,
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
                Api.ApiHelper.SetToken(accessToken);

                //Salvo il nelle properties che l'utente ha fatto accesso con Facebook
                Api.ApiHelper.SetProvider(ApiHelper.LoginProvider.Facebook);

                //creo il client e setto il Baerer Token
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                //Ottengo le info sull'utente connesso per verificare se è già registrato
                AccountClient accountClient = new AccountClient(httpClient);
                UserInfoViewModel userInfoViewModel = await accountClient.GetUserInfoAsync();

                //TODO: Controllare che la risposta del server sia OK
                try
                {
                    Application.Current.MainPage = new EnableNotification();
                }
                catch
                {
                    await DisplayAlert("errore", "errore", "cancel");
                }
            }
        }

        private string ExtractAccessTokenFromUrl(string url)
        {
            if (url.Contains("access_token") && url.Contains("&expires_in="))
            {
                var at = url.Replace($"{AppSetting.ApiEndpoint}FacebookLoading#access_token=", "");

                if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                {
                    at = url.Replace($"{AppSetting.ApiEndpoint}FacebookLoading#access_token=", "");
                }

                var accessToken = at.Remove(at.IndexOf("&token_type=bearer&expires_in="));

                return accessToken;
            }

            return string.Empty;
        }

        private async void TapGestureRecognizer_lblCondizioniGenerali(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushModalAsync(new GeneralCondition.GeneralCondition());
            }
            catch (Exception ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }

        private async void TapGestureRecognizer_lblPrivacyPolici(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushModalAsync(new PrivacyPolicy());
            }
            catch (Exception ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }
    }
}
