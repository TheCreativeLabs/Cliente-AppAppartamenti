using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AppAppartamenti.Services;
using AppAppartamenti.Views;
using AppAppartamenti.Views.Login;

namespace AppAppartamenti
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            //DependencyService.Register<MockDataStore>();            //DependencyService.Register<MockDataStore>();
            MainPage = new NavigationPage(new Login());
            //var mainPage = new Login();
            //var rootPage = new NavigationPage(mainPage);
            //Navigation
        }

        protected override void OnStart()
        {
            // Handle when your app starts
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
