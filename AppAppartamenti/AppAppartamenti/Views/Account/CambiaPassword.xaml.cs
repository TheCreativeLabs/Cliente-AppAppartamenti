using AppAppartamenti.Utility;
using AppAppartamentiApiClient;
using AppAppartamenti.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AppAppartamenti.Views;

namespace AppAppartamenti.Views.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CambiaPassword : ContentPage
    {
        public CambiaPassword()
        {
            InitializeComponent();
        }

        private async void btnProcedi_Clicked(object sender, EventArgs e)
        {
            try
            {
                bool formIsValid = true;

                //Controllo validità della vecchia password.
                if (String.IsNullOrEmpty(entVecchiaPassword.Text))
                {
                    formIsValid = false;

                    await DisplayAlert("Attenzione", "La vecchia password è obbligatoria", "OK");
                }

                //Controllo validità della nuova password.
                if (String.IsNullOrEmpty(entNuovaPassword.Text) || !Regex.IsMatch(entNuovaPassword.Text, Utility.Utility.PasswordRegex))
                {
                    formIsValid = false;

                    await DisplayAlert("Attenzione", "Le nuova password deve avere almeno una lettera maiuscola, un numero e un carattere speciale.", "OK");

                }

                //Controllo che le due password siano uguali.
                if (String.IsNullOrEmpty(entConfermaPassword.Text) || !(entNuovaPassword.Text == entConfermaPassword.Text))
                {
                    formIsValid = false;

                    await DisplayAlert("Attenzione", "Le due password non corrispondono", "OK");
                    //lblValidatorEntConfermaPassword.IsVisible = true;
                    //entConfermaPassword.BackgroundColor = Color.FromRgb(255, 175, 173);
                }

                //Se la form è valida proseguo con la registrazione.
                if (formIsValid)
                {
                    HttpClient httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await ApiHelper.GetToken());
                    AccountClient accountClient = new AccountClient(httpClient);

                    ChangePasswordBindingModel changePasswordBindingModel = new ChangePasswordBindingModel()
                    {
                        OldPassword = entVecchiaPassword.Text,
                        NewPassword = entNuovaPassword.Text,
                        ConfirmPassword = entConfermaPassword.Text
                    };

                    await accountClient.ChangePasswordAsync(changePasswordBindingModel);
                }
            }
            catch (ApiException Ex)
            {
                //Se sono qui non ho l'accesso, quindi la password è sbagliata.
                await DisplayAlert("Attenzione", "La password inserita non è valida.", "OK");
            }
            catch (Exception)
            {
                await Navigation.PushAsync(new ErrorPage());
            }
        }

        private async void ent_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                btnProcedi.IsEnabled = false;

                //Controllo che username e password siano valorizzati, se lo sono abilito il pulsante.
                if (!(String.IsNullOrEmpty(entVecchiaPassword.Text)) && !(String.IsNullOrEmpty(entNuovaPassword.Text)) && !(String.IsNullOrEmpty(entConfermaPassword.Text)))
                {
                    btnProcedi.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}