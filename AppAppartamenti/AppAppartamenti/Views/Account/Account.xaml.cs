using AppAppartamenti.Api;
using AppAppartamenti.ViewModels;
using AppAppartamentiApiClient;
using DependencyServiceDemos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAppartamenti.Views.Account
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Account : ContentPage
    {
        UserInfoDto viewModel;

        public Account()
        {
            InitializeComponent();

        }


        private async void BtnCambiaPassword_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new Login.CambiaPassword());
            }
            catch (Exception Ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }

        private void BtnInfoPersonali_Clicked(object sender, EventArgs e)
        {
            try
            {
                Navigation.PushAsync(new InformazioniPersonali());
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel == null)
            {
                AccountClient amiciClient = new AccountClient(ApiHelper.GetApiClient());
                UserInfoDto userInfo = await amiciClient.GetCurrentUserInfoAsync();

                viewModel = userInfo;
                BindingContext = viewModel;


                if (viewModel.FotoProfilo != null)
                {
                    imgFotoUtente.Source = ImageSource.FromStream(() => { return new MemoryStream(userInfo.FotoProfilo); });
                }
                else if (viewModel.PhotoUrl != null)
                {
                    imgFotoUtente.Source = ImageSource.FromUri(new Uri(viewModel.PhotoUrl));
                }

            }
        }


        private async void BtnLogOut_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (Api.ApiHelper.GetProvider() == ApiHelper.LoginProvider.Facebook)
                {
                    //Vado alla pagina di logout di facebook
                    Application.Current.MainPage = new NavigationPage(new FacebookLogout());
                }
                else if (Api.ApiHelper.GetProvider() == ApiHelper.LoginProvider.Google)
                {
                    //Vado alla pagina di logout di facebook
                    Application.Current.MainPage = new NavigationPage(new GoogleLogout());
                }
                else
                {
                    //Eseguo il logout
                    AccountClient accountClient = new AccountClient(ApiHelper.GetApiClient());
                    await accountClient.LogoutAsync();

                    //Rimuovo il token e navigo alla home
                    Api.ApiHelper.DeleteToken();
                    Application.Current.MainPage = new Login.Login();
                }
            }
            catch (Exception ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }

        //async void OnPickPhotoButtonClicked(object sender, EventArgs e)
        //{
        //    (sender as Button).IsEnabled = false;

        //    Stream stream = await DependencyService.Get<IPhotoPickerService>().GetImageStreamAsync();
        //    if (stream != null)
        //    {
        //        imgFotoUtente.Source = ImageSource.FromStream(() => stream);
        //    }

        //    (sender as Button).IsEnabled = true;
        //}

        //private void ent_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    try
        //    {
        //        //Controllo che username e password siano valorizzati.
        //        //if (!(String.IsNullOrEmpty(entNome.Text)) && !(String.IsNullOrEmpty(entCognome.Text))
        //        //    && !(String.IsNullOrEmpty(entPassword.Text)) && !(String.IsNullOrEmpty(entConfermaPassword.Text)) && !(String.IsNullOrEmpty(entEmail.Text)))
        //        //{
        //        //    btnRegistrati.IsEnabled = true;
        //        //}
        //        //else
        //        //{
        //        //    btnRegistrati.IsEnabled = false;
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
    }
}
