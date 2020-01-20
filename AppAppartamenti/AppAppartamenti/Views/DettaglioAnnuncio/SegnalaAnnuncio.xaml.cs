using System;
using System.Collections.Generic;
using AppAppartamenti.Api;
using AppAppartamenti.ViewModels;
using AppAppartamentiApiClient;
using Xamarin.Forms;

namespace AppAppartamenti.Views
{ 
    public partial class SegnalaAnnuncio : ContentPage
    {
        Guid IdAnnuncio;
        MotiviSegnalazioneViewModel viewModel;
        public SegnalaAnnuncio(Guid IdAnnuncioParam)
        {
            InitializeComponent();
            IdAnnuncio = IdAnnuncioParam;
            BindingContext = viewModel = new MotiviSegnalazioneViewModel();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            viewModel.LoadItemsCommand.Execute(null);
        }

        private async void BtnSegnala_Clicked(object sender, EventArgs e)
        {
            if (rbList.SelectedItem != null)
            {
                MotivoSegnalazione motivoSegnalazione = viewModel.translationsMap[(string)rbList.SelectedItem];
                Guid idMotivo = motivoSegnalazione.Id.Value;

                AnnunciClient adminClient = new AnnunciClient(await ApiHelper.GetApiClient());
                SegnalazioneDtoInput segnalazione = new SegnalazioneDtoInput()
                {
                    IdAnnuncio = IdAnnuncio,
                    IdMotivoSegnalazione = idMotivo,
                    TestoSegnalazione = edtSegnalazione.Text
                };

                await adminClient.InsertSegnalazioneAsync(segnalazione);

                await DisplayAlert("Segnalazione avvenuta", "Segnalazione avvenuta con successo.", "Ok");

                await Navigation.PopAsync();
            }
        }
    }
}
