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
using AppAppartamenti.Views.Messaggi;

namespace AppAppartamenti
{
    public partial class App : Application
    {

        public static object ParentWindow { get; set; }

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
                MainPage = new MainPage();
                ((MainPage)MainPage).RefreshBadge();
                await Task.Run(async () => await ApiHelper.GetListaMessaggi(true));
                await Task.Run(async () => await ApiHelper.GetListaAnnunciRecenti(true));
                await Task.Run(async () => await ApiHelper.GetUserInfo(true));
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
                CatchPush(e);
            };

            AppCenter.Start("ios=ad8c0d22-4f1c-44e4-b960-8874faf1f7f1;" +
                             "android=89d4b161-2305-4c49-9b68-8c386a835f4a;",
                             typeof(Push));
        }

        public async void CatchPush(PushNotificationReceivedEventArgs message)
        {
            //Refresh del badge e lista messaggi
            if(await ApiHelper.GetToken() != null) { 
                await ApiHelper.GetListaMessaggi(true);
                ((MainPage)MainPage).RefreshBadge();
            }
        }

        private async void getId()
        {
            var i = await AppCenter.GetInstallIdAsync();
            await Current.MainPage.DisplayAlert("ID", i.ToString(), "OK");
        }

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
