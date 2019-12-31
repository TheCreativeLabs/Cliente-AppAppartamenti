using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Timers;
using AppAppartamenti.ViewModels;
using AppAppartamentiApiClient;
using Xamarin.Forms;

namespace AppAppartamenti.Views.Messaggi
{
    public partial class NuovoMessaggio : ContentPage
    {
        MessaggiViewModel viewModel;

        Timer tmrExecutor = new Timer();


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

           if(viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);

            tmrExecutor.Elapsed += new ElapsedEventHandler(tmrExecutor_Elapsed); // adding Event
            tmrExecutor.Interval = 30000; // Set your time here 
            tmrExecutor.Enabled = true;
            tmrExecutor.Start();

            if(this.Parent.Parent.Parent.GetType() == typeof(MainPage))
                ((MainPage)this.Parent.Parent.Parent).viewModel.ReloadItemsCommand.Execute(null);
        }

        private void tmrExecutor_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            viewModel.ReloadItemsCommand.Execute(null);
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
