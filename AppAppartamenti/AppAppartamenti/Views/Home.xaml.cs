using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppAppartamenti.Api;
using AppAppartamenti.Notify;
using AppAppartamenti.ViewModels;
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
        public Home()
        {
            InitializeComponent();

            //notificationManager = DependencyService.Get<INotificationManager>();
            //notificationManager.NotificationReceived += (sender, eventArgs) =>
            //{
            //    var evtData = (NotificationEventArgs)eventArgs;
            //    ShowNotification(evtData.Title, evtData.Message);
            //};
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var isBusy = false;

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
        }

        private async void entRicerca_Focused(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Ricerca());
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