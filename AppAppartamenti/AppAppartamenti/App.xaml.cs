using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AppAppartamenti.Services;
using AppAppartamenti.Views;
using AppAppartamenti.Views.Login;
//using Microsoft.AppCenter;
//using Microsoft.AppCenter.Push;

namespace AppAppartamenti
{
    public partial class App : Application
    {
       

        public App()
        {
            InitializeComponent();
            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTgyMTA5QDMxMzcyZTMzMmUzME9QcS9ocHo4K29nbnpOUGc5OTRVMVNhOTZ1Y2pJS0ZTSXh2emxyTmJCM009");

            //DependencyService.Register<MockDataStore>();            //DependencyService.Register<MockDataStore>();
            MainPage = new NavigationPage(new Login());
            //var mainPage = new Login();
            //var rootPage = new NavigationPage(mainPage);
            //Navigation
        }

        protected override void OnStart()
        {

            //if (!AppCenter.Configured)
            //{
            //    Push.PushNotificationReceived += (sender, e) =>
            //    {
            //        Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            //        {
            //            Current.MainPage.DisplayAlert(e.Title, e.Message, "OK");
            //        });
            //    };
            //}

            //AppCenter.Start("ios=e5d73c0e-f124-4149-b83a-33bdbb6b46bc;" +
            //                 "android=89d4b161-2305-4c49-9b68-8c386a835f4a;",
            //                 typeof(Push));


            //AppCenter.GetInstallIdAsync().ContinueWith(installId =>
            //{
            //    System.Diagnostics.Debug.WriteLine("****************************************************");
            //    System.Diagnostics.Debug.WriteLine($"INSTALLATIONID={installId.Result}");
            //    System.Diagnostics.Debug.WriteLine("****************************************************");
            //});
            // Handle when your app starts
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
