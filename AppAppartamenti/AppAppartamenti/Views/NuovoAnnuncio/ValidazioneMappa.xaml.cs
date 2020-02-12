using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace AppAppartamenti.Views.NuovoAnnuncio
{
    public partial class ValidazioneMappa : ContentPage
    {
        string indirizzo;
        string comune;
        Position Position;
        public ValidazioneMappa(Position position,string Comune, string Indirizzo)
        {
            InitializeComponent();
            Position = position;
            comune = Comune;
            indirizzo = Indirizzo;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //ottengo la posizione dell'indirizzo.
            List<Position> postionList = new List<Position>();

            try
            {
                postionList.Add(Position);
            }
            catch (Exception ex)
            {
            }

            if (postionList.Count != 0)
            {
                //Se la mappa ha già dei Pin presenti li cancello.
                if (map.Pins.Count > 0) map.Pins.RemoveAt(0);

                //Ottengo la posizione e la mostro nella mappa.
                var position = postionList.FirstOrDefault<Position>();
                map.Pins.Add(new Pin() { Position = position, Label = "Indirizzo" });
                map.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromMiles(0.1)));
                map.IsVisible = true;
            }
        }

        private async void BtnBack_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PopModalAsync();
            }
            catch (Exception Ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }

        private async void BtnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void BtnProcedi_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "Valida", "Mappa validata");
            await Navigation.PopModalAsync();
        }
    }
}
