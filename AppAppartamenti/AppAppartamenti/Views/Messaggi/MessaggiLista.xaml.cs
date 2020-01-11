using System;
using System.Collections.Generic;
using System.Timers;
using AppAppartamenti.ViewModels;
using AppAppartamentiApiClient;
using Xamarin.Forms;

namespace AppAppartamenti.Views.Messaggi
{
    public partial class MessaggiLista : ContentPage
    {
        ChatListViewModel viewModel;
        Timer tmrExecutor = new Timer();

        public MessaggiLista()
        {
            InitializeComponent();

            BindingContext = viewModel = new ChatListViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //((MainPage)this.Parent.Parent).StopTimer();
            viewModel.LoadItemsCommand.Execute(null);

            //tmrExecutor.Elapsed += new ElapsedEventHandler(tmrExecutor_Elapsed);
            //tmrExecutor.Interval = 30000;
            //tmrExecutor.Enabled = true;
            //tmrExecutor.Start();
        }

        protected override void OnDisappearing()
        {
            base.OnAppearing();

            tmrExecutor.Stop();

            //((MainPage)this.Parent.Parent).StartTimer();
        }

        private async void tmrExecutor_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                viewModel.LoadItemsCommand.Execute(null);
            });
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as ChatListDtoOutput;

            if (item == null)
                return;

            await viewModel.UpdateItems(item.IdChat.Value);

            await Navigation.PushAsync(new NuovoMessaggio(item));

            // Manually deselect item.
            LvChat.SelectedItem = null;
        }
    }
}
