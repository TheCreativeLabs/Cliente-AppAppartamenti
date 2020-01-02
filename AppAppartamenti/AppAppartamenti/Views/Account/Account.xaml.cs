using AppAppartamenti.Api;
using AppAppartamenti.Notify;
using AppAppartamenti.ViewModels;
using AppAppartamenti.Views.Messaggi;
using AppAppartamentiApiClient;
using DependencyServiceDemos;
//using Foundation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAppartamenti.Views.Account
{
    public class MenuItem
    {
        public int Id { get; set; }
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
            items.Add(new MenuItem() { Id=0, DisplayName = "Informazioni personali", Icona = "\uf007", RedirectPage =new InformazioniPersonali() });
            items.Add(new MenuItem() { Id=1, DisplayName = "Cambia password", Icona = "\uf084", RedirectPage = new Login.CambiaPassword() });
            items.Add(new MenuItem() { Id = 2, DisplayName = "I miei annunci", Icona = "\uf08d", RedirectPage = new MieiAnnunci() });
            items.Add(new MenuItem() { Id=3, DisplayName = "Contattaci", Icona = "\uf658", RedirectPage = null });
            items.Add(new MenuItem() { Id=4, DisplayName = "Condividi l'app", Icona = "\uf14d", RedirectPage = null });
            items.Add(new MenuItem() { Id=5, DisplayName = "Notifiche", Icona = "\uf0f3", RedirectPage = new Notification.NotificationSetting() });
            items.Add(new MenuItem() { Id=6, DisplayName = "Privacy", Icona = "\uf505", RedirectPage = new GeneralCondition.PrivacyPolicy() });
            items.Add(new MenuItem() { Id=7, DisplayName = "Logout", Icona = "\uf2f5", RedirectPage = null});


            listView.ItemsSource = items;
        }

        async void OnItemTapped(object obj,ItemTappedEventArgs e)
        {
            var item = e.Item as MenuItem;

            if (item == null)
                return;

            listView.SelectedItem = null;

            if(item.Id == 7)
            {
                string action = await DisplayActionSheet("Continuare?","Cancel", "Log out");

                if(action == "Log out")
                {
                    await LogOut();
                }
            }else if (item.Id == 4)
            {
                await ShareUri();
            }
            else if (item.Id == 3)
            {
                await ContactUs();
            }
            else
            {
                await Navigation.PushAsync(item.RedirectPage);
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            UserInfoDto userInfo = await ApiHelper.GetUserInfo();
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

        public async Task ShareUri()
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Uri = AppSetting.SiteApp,
                Title = "Condividi il link"
            });
        }

        public async Task ContactUs()
        {
            try
            {
                var to = new List<string>();
                to.Add(AppSetting.EmailApp);
                var message = new EmailMessage
                {
                    Subject = "",
                    Body = "",
                    To = to,
                    //Cc = ccRecipients,
                    //Bcc = bccRecipients
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                // Email is not supported on this device
            }
            catch (Exception ex)
            {
                // Some other exception occurred
            }
        }

        private async Task LogOut()
        {
            try
            {
                DependencyService.Get<IClearCookies>().ClearAllCookies();

                //if (Api.ApiHelper.GetProvider() == ApiHelper.LoginProvider.Facebook)
                //{
                //    //Vado alla pagina di logout di facebook
                //    Application.Current.MainPage = new NavigationPage(new FacebookLogout());
                //}
                //else if (Api.ApiHelper.GetProvider() == ApiHelper.LoginProvider.Google)
                //{
                //    //Vado alla pagina di logout di facebook
                //    Application.Current.MainPage = new NavigationPage(new GoogleLogout());
                //}
                //else
                //{
                //Eseguo il logout
                AccountClient accountClient = new AccountClient(await ApiHelper.GetApiClient());
                await accountClient.LogoutAsync();

                //Rimuovo il token e navigo alla home
                Api.ApiHelper.RemoveSettings();
                //Api.ApiHelper.DeleteToken();
                //Api.ApiHelper.RemoveProvider();
                Application.Current.MainPage = new NavigationPage(new Login.Login());
                //}
            }
            catch (Exception ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }
    }
}
