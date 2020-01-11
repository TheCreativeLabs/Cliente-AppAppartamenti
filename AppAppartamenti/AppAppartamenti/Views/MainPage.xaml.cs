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
        TabPageViewModel viewModel;

        public MainPage()
        {
            InitializeComponent();
        }

        public async void RefreshBadge()
        {
            MessaggiClient messaggiClient = new MessaggiClient(await Api.ApiHelper.GetApiClient());
            var newMsg = await messaggiClient.GetChatMessagesToReadAsync();

            Device.BeginInvokeOnMainThread(() =>
            {
                viewModel = new TabPageViewModel();
                viewModel.NewMessages = newMsg;
                BindingContext = viewModel;
            });
        }
    }
}