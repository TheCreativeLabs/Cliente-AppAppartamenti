using System;
using System.Collections.Generic;
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

            //if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as ChatListDtoOutput;

            if (item == null)
                return;
          
            await Navigation.PushAsync(new NuovoMessaggio(item));

            // Manually deselect item.
            LvChat.SelectedItem = null;
        }
    }
}
