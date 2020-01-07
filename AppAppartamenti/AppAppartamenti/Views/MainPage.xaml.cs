using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using AppAppartamenti.Api;
using AppAppartamenti.ViewModels;
using AppAppartamentiApiClient;
using Plugin.Badge.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace AppAppartamenti.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : Xamarin.Forms.TabbedPage
    {
        Timer tmrExecutor = new Timer();
        TabPageViewModel viewModel;
        public MainPage()
        {
            InitializeComponent();
            viewModel = new TabPageViewModel();
            BindingContext = viewModel;

            GetBindingContext();

            tmrExecutor.Elapsed += new ElapsedEventHandler(tmrExecutor_Elapsed);
            tmrExecutor.Interval = 30000;
            tmrExecutor.Enabled = true;
        }

        private async void GetBindingContext()
        {
            MessaggiClient messaggiClient = new MessaggiClient(await Api.ApiHelper.GetApiClient());
            var newMsg = await messaggiClient.GetChatMessagesToReadAsync();
            viewModel.NewMessages = newMsg;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            tmrExecutor.Start();
        }

        protected async override void OnDisappearing()
        {
            base.OnDisappearing();
            tmrExecutor.Stop();
        }

        private async void tmrExecutor_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            MessaggiClient messaggiClient = new MessaggiClient(await Api.ApiHelper.GetApiClient());
            var newMsg = await messaggiClient.GetChatMessagesToReadAsync();

            if (newMsg > 0)
            {
                ApiHelper.GetListaMessaggi(true);
            }

            RefreshBadge(newMsg);
        }

        public void RefreshBadge(int newMsg)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                viewModel = new TabPageViewModel();
                viewModel.NewMessages = newMsg;
                BindingContext = viewModel;
            });
        }

        public void StopTimer()
        {
            tmrExecutor.Stop();
        }

        public void StartTimer()
        {
            tmrExecutor.Start();
        }

        private Task DisplayAlert(string v1, string v2)
        {
            throw new NotImplementedException();
        }
    }
}