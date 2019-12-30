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
            this.Title = $"{ChatInfo.Nome} {ChatInfo.Cognome}";
        }

        public NuovoMessaggio(Guid IdAnnuncio, Guid IdPersonToMeet)
        {
            InitializeComponent();

            BindingContext = viewModel = new MessaggiViewModel();
            viewModel.IdAnnuncioParam = IdAnnuncio;
            viewModel.IdUserToChat = IdPersonToMeet;
            this.Title = $"Nuovo messaggio";

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<Ricerca, string>(this, "AggiornaMsg", async (sender, arg) =>
            {
                if (!string.IsNullOrEmpty(arg))
                {
                   viewModel.LoadItemsCommand.Execute(null);
                }
            });

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        private async void Send_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(chatTextInput.Text))
            {
                MessaggiClient messaggiClient = new MessaggiClient(await Api.ApiHelper.GetApiClient());
                await messaggiClient.InsertMessaggioAsync(viewModel.IdChat, viewModel.IdUser, chatTextInput.Text);

                viewModel.LoadItemsCommand.Execute(null);

                chatTextInput.Text = "";
            }
        }
    }
}
