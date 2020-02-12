using System;
using System.Collections.Generic;
using AppAppartamenti.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppAppartamenti.ContentViews
{
    public partial class AcceptConditionContentView : ContentView
    {
        public AcceptConditionContentView()
        {
            InitializeComponent();
        }

        private async void TapGestureRecognizer_lblCondizioniGenerali(object sender, EventArgs e)
        {
            try
            {
                await Browser.OpenAsync(AppSetting.GeneralConditionUrl, new BrowserLaunchOptions
                {
                    LaunchMode = BrowserLaunchMode.SystemPreferred,
                    TitleMode = BrowserTitleMode.Show
                });
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
                await Browser.OpenAsync(AppSetting.PrivacyUrl, new BrowserLaunchOptions
                {
                    LaunchMode = BrowserLaunchMode.SystemPreferred,
                    TitleMode = BrowserTitleMode.Show
                });
            }
            catch (Exception ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }
    }
}
