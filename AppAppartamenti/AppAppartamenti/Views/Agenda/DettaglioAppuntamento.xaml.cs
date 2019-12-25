using System;
using System.Collections.Generic;
using AppAppartamenti.Views.Messaggi;
using AppAppartamentiApiClient;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace AppAppartamenti.Views
{
    public partial class DettaglioAppuntamento : ContentPage
    {
        Guid IdAppuntamento;

        AppuntamentoDettaglioDtoOutput viewModel;

        public DettaglioAppuntamento(AppuntamentoDtoOutput appuntamento)
        {
            InitializeComponent();

            IdAppuntamento = appuntamento.IdAppuntamento;

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            
            AgendaClient agendaClient = new AgendaClient(Api.ApiHelper.GetApiClient());
            var appuntamento =  await agendaClient.GetAppuntamentoByIdAsync(IdAppuntamento);
            BindingContext = viewModel = viewModel = appuntamento;


            if (!String.IsNullOrEmpty(viewModel.CoordinateGeograficheAnnuncio))
            {
                var pos = viewModel.CoordinateGeograficheAnnuncio.Split(';');
                var lat = pos[0];
                var lon = pos[1];

                Pin pin = new Pin
                {
                    Label = viewModel.InfoAnnuncio.Indirizzo,
                    Address = $"{viewModel.InfoAnnuncio.Indirizzo},{viewModel.InfoAnnuncio.NomeComune}",
                    Type = PinType.Generic,
                    Position = new Position(Double.Parse(lat), Double.Parse(lon))
                };

                map.Pins.Add(pin);
                map.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Position, Distance.FromMiles(0.1)));
            }

        }


        private async void BtnMessaggio_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NuovoMessaggio());
        }

        private async void BtnDelete_Clicked(object sender, EventArgs e)
        {
            var btn = sender as Button;
            btn.IsEnabled = false;


            bool answer = await DisplayAlert("Annullare l'appuntamento", "Procedendo l'appuntamento verrà cancellato definitamente.", "Si", "No");
            if (answer)
            {
                AgendaClient agendaClient = new AgendaClient(Api.ApiHelper.GetApiClient());
                await agendaClient.DeleteAppuntamentoAsync(IdAppuntamento);
                await Navigation.PopAsync();
            }

            btn.IsEnabled = true;

        }
    }
}
