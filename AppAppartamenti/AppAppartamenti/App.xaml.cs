using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AppAppartamenti.Services;
using AppAppartamenti.Views;
using AppAppartamenti.Views.Login;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Push;
using AppAppartamenti.Api;
using System.Threading.Tasks;

namespace AppAppartamenti
{
    public partial class App : Application
    {
       

        public App()
        {
            InitializeComponent();
            //Register Syncfusion license
            
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTgyMTA5QDMxMzcyZTMzMmUzME9QcS9ocHo4K29nbnpOUGc5OTRVMVNhOTZ1Y2pJS0ZTSXh2emxyTmJCM009");
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTg2MTU3QDMxMzcyZTM0MmUzMEhjUEJsQW5VeG56QUlSUTBEcU8rcG5Ld2hTQ3lPcTk4MUpubnNWRnNIbDg9");

            SetMainPage();


        }

        public async void SetMainPage()
        {
            if (await ApiHelper.GetToken() != null)
            {
                Task.Run(async () => { await ApiHelper.GetUserInfo(); });
                Task.Run(async () => { await ApiHelper.GetListaTipologiaProprieta(); });
                Task.Run(async () => { await ApiHelper.GetListaTipologiaAnnuncio(); });
                
                MainPage = new MainPage();
            }
            else
            {
                MainPage = new NavigationPage(new Login());
            }
        }

        protected override void OnStart()
        {

            Push.PushNotificationReceived += (sender, e) =>
            {
                // Add the notification message and title to the message
                var summary = $"Push notification received:" +
                                    $"\n\tNotification title: {e.Title}" +
                                    $"\n\tMessage: {e.Message}";

                // If there is custom data associated with the notification,
                // print the entries
                if (e.CustomData != null)
                {
                    summary += "\n\tCustom data:\n";
                    foreach (var key in e.CustomData.Keys)
                    {
                        summary += $"\t\t{key} : {e.CustomData[key]}\n";
                    }
                }

                // Send the notification summary to debug output
                System.Diagnostics.Debug.WriteLine(summary);
            };

            AppCenter.Start("ios=e5d73c0e-f124-4149-b83a-33bdbb6b46bc;" +
                             "android=89d4b161-2305-4c49-9b68-8c386a835f4a;",
                             typeof(Push));

            //AppCenter.Start("e5d73c0e-f124-4149-b83a-33bdbb6b46bc;", typeof(Push));

            //AppCenter.SetUserId("SIMONEB");

            //if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS)
            //{
            //    //Start AppCenter Push notification with iOS app secret
            //    AppCenter.Start("e5d73c0e-f124-4149-b83a-33bdbb6b46bc", typeof(Push));
            //}
            //else if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android)
            //{
            //    //Start AppCenter Push notification with Android app secret
            //    AppCenter.Start("89d4b161-2305-4c49-9b68-8c386a835f4a", typeof(Push));
            //}

            



            // Handle when your app starts
        }

        private async void getId()
        {
            var i = await AppCenter.GetInstallIdAsync();
            await Current.MainPage.DisplayAlert("ID", i.ToString(), "OK");
        }

        //private void OnPushNotificationRecieved(object sender, PushNotificationReceivedEventArgs e)
        //{
        //    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
        //    {
        //        Current.MainPage.DisplayAlert(e.Title, e.Message, "OK");
        //    });
        //}

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
