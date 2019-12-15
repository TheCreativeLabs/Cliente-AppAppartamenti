using AppAppartamenti.Api;
using AppAppartamenti.ViewModels;
using AppAppartamentiApiClient;
using DependencyServiceDemos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAppartamenti.Views.Account
{
    public class MenuItem
    {
        public Page  RedirectPage { get; set; }
        public string DisplayName { get; set; }
        public string Icona { get; set; }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Account : ContentPage
    {
        UserInfoDto viewModel;

        public Account()
        {
            InitializeComponent();

            ObservableCollection<MenuItem> items = new ObservableCollection<MenuItem>();
            items.Add(new MenuItem() { DisplayName = "Informazioni personali", Icona = "\uf007", RedirectPage =new InformazioniPersonali() });
            items.Add(new MenuItem() { DisplayName = "Cambia password", Icona = "\uf084", RedirectPage = new Login.CambiaPassword() });
            items.Add(new MenuItem() { DisplayName = "Contattaci", Icona = "\uf658", RedirectPage = null });
            items.Add(new MenuItem() { DisplayName = "Notifiche", Icona = "\uf0f3", RedirectPage = null });
            items.Add(new MenuItem() { DisplayName = "Privacy", Icona = "\uf505", RedirectPage = null });
            items.Add(new MenuItem() { DisplayName = "Logout", Icona = "\uf2f5", RedirectPage = null});

            listView.ItemsSource = items;
        }

        async void OnItemTapped(object obj,ItemTappedEventArgs e)
        {
            var item = e.Item as MenuItem;

            if (item == null)
                return;

            listView.SelectedItem = null;

            if(item.RedirectPage == null)
            {
                string action = await DisplayActionSheet("Continuare?","Cancel", "Log out");

                if(action == "Log out")
                {
                    await LogOut();
                }
            }
            else
            {
                await Navigation.PushAsync(item.RedirectPage);
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

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


        private async Task LogOut()
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
    }
}
