using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using AppAppartamenti.ViewModels;
using AppAppartamentiApiClient;
using Xamarin.Forms;

namespace AppAppartamenti.Views.Messaggi
{
    public partial class NuovoMessaggio : ContentPage
    {
        MessaggiViewModel viewModel;

        public NuovoMessaggio(ChatListDtoOutput ChatInfo)
        {
            InitializeComponent();

            BindingContext = viewModel = new MessaggiViewModel();
            viewModel.IdChatParam = ChatInfo.IdChat.Value;
        }

        public NuovoMessaggio(Guid IdAnnuncio)
        {
            InitializeComponent();

            BindingContext = viewModel = new MessaggiViewModel();
            viewModel.IdAnnuncioParam = IdAnnuncio;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}
