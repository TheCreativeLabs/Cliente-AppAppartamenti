﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppAppartamenti.ViewModels;
using AppAppartamentiApiClient;
using DependencyServiceDemos;
using RestSharp.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace AppAppartamenti.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelezioneLuogo : ContentPage
    {
        AnnuncioDtoInput annuncio;

        public SelezioneLuogo(AnnuncioDtoInput Annuncio)
        {
            InitializeComponent();

            annuncio = Annuncio;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                ((NavigationPage)this.Parent).BarBackgroundColor = Color.White;
                ((NavigationPage)this.Parent).BarTextColor = Color.Black;
                NavigationPage.SetHasNavigationBar(this, true);
            }
            catch (Exception ex)
            {

            }

            slBody.IsVisible = true;
        }

        private async void BtnBack_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PopAsync();
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

        private async void BtnIndirizzoProcedi_Clicked(object sender, EventArgs e)
        {
            annuncio.Indirizzo = entIndirizzo.Text;

            await setMapLocation();

            mapsPopup.IsVisible = true;
        }


        private async Task setMapLocation()
        {
            //ottengo la posizione dell'indirizzo.
            List<Position> postionList = new List<Position>();

            try
            {
                postionList = (await (new Geocoder()).GetPositionsForAddressAsync($"{entIndirizzo.Text} , {entCercaComune.Text}")).ToList();
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

            //btnIndirizzoProcedi.IsVisible = true;
        }

        private void EntCercaComune_TextChanged(object sender, TextChangedEventArgs e)
        {
            //refresh della lista dei comuni
            var listaComuni = new ListaComuniViewModel(entCercaComune.Text);
            listaComuni.LoadItemsCommand.Execute(null);
            lvComuni.ItemsSource = listaComuni.Items;
        }

        async void LvComuni_Selected(object sender, SelectedItemChangedEventArgs args)
        {
            Comuni comune = args.SelectedItem as Comuni;

            if (comune == null || comune.CodiceComune == null)
                return;

            //modifico l'annuncio
            annuncio.CodiceComune = comune.CodiceComune.Value;

            //modifico la textbox del comune inserendo il nome completo del comune
            entCercaComune.Text = comune.NomeComune;

            //nascondo la lista dei comuni e mostro la entry dell'indirizzo
            lvComuni.IsVisible = false;
            entIndirizzo.IsVisible = true;

            //mostro il bottone procedi
            //btnIndirizzoProcedi.IsVisible = true;

            // Manually deselect item.
            lvComuni.SelectedItem = null;

        }

        private void EntIndirizzo_textChanged(object sender, TextChangedEventArgs e)
        {
            btnIndirizzoProcedi.IsEnabled = true;
        }

        private void ButtonPopUpNo(object sender, EventArgs e)
        {
            mapsPopup.IsVisible = false;
        }

        private async void ButtonPopUpSi(object sender, EventArgs e)
        {
            mapsPopup.IsVisible = false;
            await Navigation.PushAsync(new SelezioneInfoGenerali(annuncio));
        }

    }
}