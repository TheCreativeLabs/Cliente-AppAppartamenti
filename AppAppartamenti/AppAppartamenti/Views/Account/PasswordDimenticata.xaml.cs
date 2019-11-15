using AppAppartamenti.Utility;
using AppAppartamentiApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAppartamenti.Views.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PasswordDimenticata : ContentPage
    {
        public PasswordDimenticata()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                ((NavigationPage)this.Parent).BarBackgroundColor = Color.White;
                ((NavigationPage)this.Parent).BarTextColor = Color.Black;
                NavigationPage.SetHasNavigationBar(this, true);
            }
            catch (Exception ex)
            {

            }

            stkBody.IsVisible = true;
        }

    private async void btnProsegui_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(entEmail.Text) || !Regex.IsMatch(entEmail.Text, Utility.Utility.EmailRegex))
                {
                    entEmail.BackgroundColor = Color.FromRgb(255, 175, 173);
                    lblValidatorEntEmail.IsVisible = true;
                }
                else
                {
                    HttpClient httpClient = new HttpClient();
                    AccountClient accountClient = new AccountClient(httpClient);
                    await accountClient.RestorePasswordAsync(entEmail.Text);

                    stkFormRestore.IsVisible = false;
                    stkRestoreAvvenuto.IsVisible = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void btnLogin_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new Login());
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void entEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Controllo che username e password siano valorizzati.
            if (!(String.IsNullOrEmpty(entEmail.Text)))
            {
                btnProsegui.IsEnabled = true;
            }
            else
            {
                btnProsegui.IsEnabled = false;
            }
        }
    }
}