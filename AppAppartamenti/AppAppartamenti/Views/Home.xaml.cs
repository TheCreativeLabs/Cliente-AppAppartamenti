using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppAppartamenti.Api;
using AppAppartamenti.Notify;
using AppAppartamenti.ViewModels;
using AppAppartamentiApiClient;
using Newtonsoft.Json;
using Plugin.Media;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAppartamenti.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : ContentPage
    {
        //INotificationManager notificationManager;
        //int notificationNumber = 0;

        RicercaModel RicercaModel;
        AnnunciRecentiViewModel viewModel;

        public Home()
        {
            InitializeComponent();

            BindingContext = viewModel = new AnnunciRecentiViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<Ricerca, string>(this, "Ricerca", async (sender, arg) =>
            {
                RicercaModel = null;
                if (!string.IsNullOrEmpty(arg))
                {
                    RicercaModel = JsonConvert.DeserializeObject<RicercaModel>(arg);
                }
            });

            if(RicercaModel != null)
            {
                await Navigation.PushAsync(new ListaAnnunci(RicercaModel));
                RicercaModel = null;
            }

            if(!viewModel.Items.Any())
                viewModel.LoadItemsCommand.Execute(null);

            ((MainPage)this.Parent.Parent).viewModel.ReloadItemsCommand.Execute(null);
        }

        private async void entRicerca_Focused(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Ricerca());
        }

        private async void btnNuovoAnnuncio_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new SelezioneProprieta(null))); //è un nuovo annuncio, non devo passare l'annuncio da modificare
        }

        async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (!e.CurrentSelection.Any())
                return;

            var item = e.CurrentSelection.First() as AnnunciDtoOutput;

            if (item.Id != null && item.Id != Guid.Empty)
            {
                await Navigation.PushAsync(new DettaglioAnnuncio(item, false));
            }

            // Manually deselect item.
            cvRecenti.SelectedItem = null;
        }

        //async void BtnAdd_Clicked(object sender, EventArgs e)
        //{
        //    notificationNumber++;
        //    string title = $"Local Notification #{notificationNumber}";
        //    string message = $"You have now received {notificationNumber} notifications!";
        //    notificationManager.ScheduleNotification(title, message, await ApiHelper.GetNotificationStatus());
        //}

        //async void OnScheduleClick(object sender, EventArgs e)
        //{
        //    notificationNumber++;
        //    string title = $"Local Notification #{notificationNumber}";
        //    string message = $"You have now received {notificationNumber} notifications!";
        //    notificationManager.ScheduleNotification(title, message, await ApiHelper.GetNotificationStatus());
        //}

        //void ShowNotification(string title, string message)
        //{
        //    Device.BeginInvokeOnMainThread(async () =>
        //    {
        //           await DisplayAlert("Notifica",$"Notification Received:\nTitle: {title}\nMessage: {message}","OK");
        //    });
        //}
    }
}