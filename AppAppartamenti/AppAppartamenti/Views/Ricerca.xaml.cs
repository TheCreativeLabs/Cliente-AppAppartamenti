﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppAppartamenti.Api;
using AppAppartamenti.ViewModels;
using AppAppartamentiApiClient;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAppartamenti.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Ricerca : ContentPage
    {
        List<TipologiaAnnuncio> listTipologiaAnnuncio = new List<TipologiaAnnuncio>();
        List<TipologiaProprieta> listTipologiaProprieta = new List<TipologiaProprieta>();
        RicercaModel FiltriRicerca;

        public Ricerca()
        {
            InitializeComponent();
        }

        public Ricerca(RicercaModel FiltriRicercaParam)
        {
            InitializeComponent();
            FiltriRicerca = FiltriRicercaParam;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            entRicerca.Focus();

            if (listTipologiaAnnuncio.Any() == false)
            {
                listTipologiaAnnuncio = await ApiHelper.GetListaTipologiaAnnuncio();
            }

            pckTipologiaVendita.ItemsSource = listTipologiaAnnuncio;


            if (listTipologiaProprieta.Any() == false)
            {
                listTipologiaProprieta = await ApiHelper.GetListaTipologiaProprieta();
                listTipologiaProprieta.Insert(0, new TipologiaProprieta() { Id = null,Codice="", Descrizione="Seleziona..."});
            }

            pckTipologiaProprieta.ItemsSource = listTipologiaProprieta;



            if (FiltriRicerca != null)
            {
                pckTipologiaVendita.SelectedItem = listTipologiaAnnuncio.Where(x=> x.Id==FiltriRicerca.TipologiaAnnuncio).FirstOrDefault();
                pckTipologiaProprieta.SelectedItem = listTipologiaProprieta.Where(x=> x.Id==FiltriRicerca.TipologiaProprieta).FirstOrDefault();
                stpBadrooms.Value = FiltriRicerca.NumCamereLetto;
                stpBathroom.Value = FiltriRicerca.NumBagni;
                stpkitchens.Value = FiltriRicerca.NumCucine;
                stpParkingSpace.Value = FiltriRicerca.NumPostiAuto;
                stpGarages.Value = FiltriRicerca.NumGarage;
                chkAscensore.IsChecked = FiltriRicerca.Ascensore.Value;
                chkTerrazzo.IsChecked = FiltriRicerca.Terrazzo.Value;
                chkCantina.IsChecked = FiltriRicerca.Cantina.Value;
                chkCondizionatori.IsChecked = FiltriRicerca.Condizionatori.Value;
                chkGiardino.IsChecked = FiltriRicerca.Giardino.Value;
                chkPiscina.IsChecked = FiltriRicerca.Piscina.Value;
                RangeSlider.LowerValue = FiltriRicerca.MinPrice;
                RangeSlider.UpperValue = FiltriRicerca.MaxPrice;
                RangeSlider2.LowerValue = FiltriRicerca.MinSurface;
                RangeSlider2.UpperValue = FiltriRicerca.MaxSurface;
                lvComuni.SelectedItem = FiltriRicerca.Comune;
                entRicerca.Text = FiltriRicerca.Comune.NomeComune;
            }
            else
            {
                if (pckTipologiaVendita.SelectedItem == null)
                {
                    pckTipologiaVendita.SelectedItem = listTipologiaAnnuncio.FirstOrDefault();
                }
                if (pckTipologiaProprieta.SelectedItem == null)
                {
                    pckTipologiaProprieta.SelectedItem = listTipologiaProprieta.FirstOrDefault();
                }
            }
        }

        private async void btnCancel_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "Ricerca", "");

            await Navigation.PopModalAsync();
        }

        private async void btnRicerca_Clicked(object sender, EventArgs e)
        {
            if(lvComuni.SelectedItem != null)
            {
                RicercaModel ricercaModel = new RicercaModel()
                {
                    Comune = lvComuni.SelectedItem as ComuneDto,
                    MinPrice = (int)RangeSlider.LowerValue,
                    MaxPrice = (int)RangeSlider.UpperValue,
                    TipologiaAnnuncio = ((TipologiaAnnuncio)pckTipologiaVendita.SelectedItem).Id,
                    TipologiaProprieta = ((TipologiaProprieta)pckTipologiaProprieta.SelectedItem).Id,
                    MinSurface = (int)RangeSlider2.LowerValue,
                    MaxSurface = (int)RangeSlider2.UpperValue,
                    NumCamereLetto = (int)stpBadrooms.Value,
                    NumBagni = (int)stpBathroom.Value,
                    NumCucine = (int)stpkitchens.Value,
                    NumGarage = (int)stpGarages.Value,
                    NumPostiAuto = (int)stpParkingSpace.Value,
                    Ascensore = chkAscensore.IsChecked,
                    Cantina = chkCantina.IsChecked,
                    Giardino = chkGiardino.IsChecked,
                    Piscina = chkPiscina.IsChecked,
                    Condizionatori = chkCondizionatori.IsChecked,
                    Terrazzo = chkTerrazzo.IsChecked
                };

                //TODO: capire qual'è l'ultima pagina:
                //Se è ListaAnnunci allora mando un messaggio alla listaannunci, altrimenti allla home
                MessagingCenter.Send(this, "Ricerca", JsonConvert.SerializeObject(ricercaModel));
                await  Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Attenzione", "Selezionare la città o la provincia.", "OK");
            }
        }

        private void EntRicerca_TextChanged(object sender, TextChangedEventArgs e)
        {
            //refresh della lista dei comuni
            var listaComuni = new ListaComuniViewModel(entRicerca.Text);
            listaComuni.LoadItemsCommand.Execute(null);
            lvComuni.ItemsSource = listaComuni.Items;

            stkRicercaAggiuntiva.IsVisible = false;
            lvComuni.IsVisible = true;
        }

        async void LvComuni_Selected(object sender, SelectedItemChangedEventArgs args)
        {
            var comune = args.SelectedItem as ComuneDto;

            if (comune == null)
                return;

            //modifico la textbox del comune inserendo il nome completo del comune
            entRicerca.Text = comune.NomeComune;

            lvComuni.IsVisible = false;
            stkRicercaAggiuntiva.IsVisible = true;
        }

        void StpCamereLetto_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            lblBadroomsCounter.Text = stpBadrooms.Value.ToString();
        }

        void StpStanze_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            lblOtherRoomsCounter.Text = stpOtherRooms.Value.ToString();
        }

        void StpBagni_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            lblBathroomCounter.Text = stpBathroom.Value.ToString();
        }

        void StpCucine_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            lblkitchensCounter.Text = stpkitchens.Value.ToString();
        }

        void StpGarage_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            lblGaragesCounter.Text = stpGarages.Value.ToString();
        }

        void StpParkingSpaces_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            lblParkingSpaceCounter.Text = stpParkingSpace.Value.ToString();
        }
    }
}