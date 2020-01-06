using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Timers;
using AppAppartamenti.ViewModels;
using AppAppartamentiApiClient;
using Xamarin.Forms;
using AppAppartamenti.Behaviors;
using System.Linq;
using AppAppartamenti.Api;

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

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if(viewModel.IdChatParam.HasValue) { 
                ApiHelper.UpdateChatList(viewModel.IdChatParam.Value);
                ((MainPage)this.Parent.Parent).RefreshBadge(0);
            }
            else
            {
                ApiHelper.GetListaAnnunciRecenti(true);
            }

            if (viewModel.Items.Count == 0)
                await viewModel.LoadItems();

            if (viewModel.Items.Any())
                lvMessages.ScrollTo(viewModel.Items[viewModel.Items.Count - 1], ScrollToPosition.End, false);

            tmrExecutor.Elapsed += new ElapsedEventHandler(tmrExecutor_Elapsed);
            tmrExecutor.Interval = 30000;
            tmrExecutor.Enabled = true;
            tmrExecutor.Start();
        }

        protected override void OnDisappearing()
        {
            base.OnAppearing();

            tmrExecutor.Stop();
        }

        private async void tmrExecutor_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await viewModel.ReloadItems();

                if (viewModel.Items.Any())
                    lvMessages.ScrollTo(viewModel.Items[viewModel.Items.Count - 1], ScrollToPosition.End, false);
            });
        }

        private async void Send_Clicked(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(chatTextInput.Text))
            {
                var msg = chatTextInput.Text;
                chatTextInput.Text = "";

                viewModel.AddNewMessage.Execute(msg);

                MessaggiClient messaggiClient = new MessaggiClient(await Api.ApiHelper.GetApiClient());
                await messaggiClient.InsertMessaggioAsync(viewModel.IdChat, viewModel.IdUser, msg);

                lvMessages.ScrollTo(viewModel.Items[viewModel.Items.Count - 1], ScrollToPosition.End, false);
            }
        }
    }
}
