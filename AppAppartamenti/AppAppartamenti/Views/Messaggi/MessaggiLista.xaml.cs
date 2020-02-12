using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using AppAppartamenti.ViewModels;
using AppAppartamentiApiClient;
using Xamarin.Forms;

namespace AppAppartamenti.Views.Messaggi
{
    public partial class MessaggiLista : ContentPage
    {
        ChatListViewModel viewModel;

        public MessaggiLista()
        {
            InitializeComponent();

            BindingContext = viewModel = new ChatListViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewModel.LoadItemsCommand.Execute(null);
        }

        protected override void OnDisappearing()
        {
            base.OnAppearing();
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
