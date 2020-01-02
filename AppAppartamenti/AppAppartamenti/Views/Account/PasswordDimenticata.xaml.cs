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
        }

    private async void btnProsegui_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(entEmail.Text) || !Regex.IsMatch(entEmail.Text, Utility.Utility.EmailRegex))
                {
                    //entEmail.BackgroundColor = Color.FromRgb(255, 175, 173);
                    //lblValidatorEntEmail.IsVisible = true;
                    await DisplayAlert("Attenzione", "Inserisci la tua email per recuperare la password", "OK");
                }
                else
                {
                    HttpClient httpClient = new HttpClient();
                    AccountClient accountClient = new AccountClient(httpClient);
                    await accountClient.RestorePasswordAsync(entEmail.Text);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}