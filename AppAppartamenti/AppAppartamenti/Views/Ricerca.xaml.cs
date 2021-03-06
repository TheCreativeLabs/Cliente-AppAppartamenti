﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using AppAppartamenti.Api;
using AppAppartamenti.ViewModels;
using AppAppartamentiApiClient;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.RangeSlider.Forms;

namespace AppAppartamenti.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Ricerca : ContentPage
    {
        List<TipologiaAnnuncio> listTipologiaAnnuncio = new List<TipologiaAnnuncio>();
        List<TipologiaProprieta> listTipologiaProprieta = new List<TipologiaProprieta>();
        List<TipologiaProprieta> listTipologiaProprietaVendita = new List<TipologiaProprieta>();
        TipologiaProprieta room;
        TipologiaProprieta postoLetto;
        RicercaModel FiltriRicerca;
        bool IsRicaricamento;
        ListaComuniViewModel viewModel;

        public Ricerca()
        {
            InitializeComponent();

        }

        public Ricerca(RicercaModel FiltriRicercaParam)
        {
            InitializeComponent();
            FiltriRicerca = FiltriRicercaParam;
            IsRicaricamento = true;

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            RangeSlider2.Effects.Add(Effect.Resolve("EffectsSlider.RangeSliderEffect"));
            RangeSliderAffitto.Effects.Add(Effect.Resolve("EffectsSlider.RangeSliderEffect"));
            RangeSliderVendita.Effects.Add(Effect.Resolve("EffectsSlider.RangeSliderEffect"));


            BindingContext = viewModel = new ListaComuniViewModel();

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

            //pckTipologiaProprieta.ItemsSource = listTipologiaProprieta

            listTipologiaProprietaVendita = listTipologiaProprieta;
            room = listTipologiaProprietaVendita.FirstOrDefault(x => x.Codice == "ROOM");
            postoLetto = listTipologiaProprietaVendita.FirstOrDefault(x => x.Codice == "POSTO_LETTO");
            listTipologiaProprietaVendita.Remove(room);
            listTipologiaProprietaVendita.Remove(postoLetto);


            pckTipologiaProprieta.ItemsSource = listTipologiaProprietaVendita;

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
                chkNoArchitecturalBarriers.IsChecked = FiltriRicerca.SenzaBarriereArchitettoniche.Value;
                chkStairlifts.IsChecked = FiltriRicerca.Montascale.Value;
                chkNoStepsInProperty.IsChecked = FiltriRicerca.SenzaGradiniInternoProprieta.Value;

                if (pckTipologiaVendita.SelectedIndex == 0)
                {
                    RangeSliderVendita.LowerValue = FiltriRicerca.MinPrice;
                    RangeSliderVendita.UpperValue = FiltriRicerca.MaxPrice;
                }
                else
                {
                    RangeSliderAffitto.LowerValue = FiltriRicerca.MinPrice;
                    RangeSliderAffitto.UpperValue = FiltriRicerca.MaxPrice;
                }

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
                var prezzoMin = 0;
                var prezzoMax = 0;
                if(pckTipologiaVendita.SelectedIndex == 0)
                {
                    prezzoMin = (int)RangeSliderVendita.LowerValue;
                    prezzoMax = (int)RangeSliderVendita.UpperValue;
                }
                else
                {
                    prezzoMin = (int)RangeSliderAffitto.LowerValue;
                    prezzoMax = (int)RangeSliderAffitto.UpperValue;
                }

                RicercaModel ricercaModel = new RicercaModel()
                {
                    Comune = lvComuni.SelectedItem as ComuneDto,
                    MinPrice = prezzoMin,
                    MaxPrice = prezzoMax,
                    TipologiaAnnuncio = ((TipologiaAnnuncio)pckTipologiaVendita.SelectedItem).Id,
                    TipologiaProprieta = ((TipologiaProprieta)pckTipologiaProprieta.SelectedItem).Id,
                    MinSurface = (int)RangeSlider2.LowerValue,
                    MaxSurface = (int)RangeSlider2.UpperValue,
                    NumCamereLetto = (int)stpBadrooms.Value,
                    NumBagni = (int)stpBathroom.Value,
                    NumCucine = (int)stpkitchens.Value,
                    NumGarage = (int)stpGarages.Value,
                    NumAltreStanze = (int)stpOtherRooms.Value,
                    NumPostiAuto = (int)stpParkingSpace.Value,
                    Ascensore = chkAscensore.IsChecked,
                    Cantina = chkCantina.IsChecked,
                    Giardino = chkGiardino.IsChecked,
                    Piscina = chkPiscina.IsChecked,
                    Condizionatori = chkCondizionatori.IsChecked,
                    Terrazzo = chkTerrazzo.IsChecked,
                    SenzaBarriereArchitettoniche = chkNoArchitecturalBarriers.IsChecked,
                    Montascale = chkStairlifts.IsChecked,
                    SenzaGradiniInternoProprieta = chkNoStepsInProperty.IsChecked
                };


                if (IsRicaricamento)
                {
                    //Se è ListaAnnunci allora mando un messaggio alla listaannunci, altrimenti allla home
                    MessagingCenter.Send(this, "Ricarica", JsonConvert.SerializeObject(ricercaModel));
                }
                else
                {
                    //Se è ListaAnnunci allora mando un messaggio alla listaannunci, altrimenti allla home
                    MessagingCenter.Send(this, "Ricerca", JsonConvert.SerializeObject(ricercaModel));
                }

                //TODO: capire qual'è l'ultima pagina:
                await  Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Attenzione", "Selezionare la città o la provincia.", "OK");
            }
        }

        public void TipologiaVenditaSelectedIndexChanged(object sender, EventArgs e)
        {
            //Method call every time when picker selection changed.
            var picker = (Picker)sender;

            if(picker.SelectedIndex == 0) {
                stkPrezzoVendita.IsVisible = true;
                stkPrezzoAffitto.IsVisible = false;

                //pckTipologiaProprieta.ItemsSource.Remove(room);
                //pckTipologiaProprieta.ItemsSource.Remove(postoLetto);
                //pckTipologiaProprieta.ItemsSource.Clear();
                //pckTipologiaProprieta.ItemsSource = listTipologiaProprietaVendita; 
                //pckTipologiaProprieta.SelectedItem = listTipologiaProprieta.FirstOrDefault();
            }
            else
            {
                stkPrezzoVendita.IsVisible = false;
                stkPrezzoAffitto.IsVisible = true;

                //pckTipologiaProprieta.ItemsSource.Add(room);
                //fixme doesn't work
                //pckTipologiaProprieta.ItemsSource.Clear();
                //pckTipologiaProprieta.ItemsSource = listTipologiaProprieta;
                //pckTipologiaProprieta.SelectedItem = listTipologiaProprieta.FirstOrDefault();
                //pckTipologiaProprieta.ItemsSource.Add(postoLetto);

            }
        }

        private async void EntRicerca_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(entRicerca.Text.Length > 3)
            {
                stkRicercaAggiuntiva.IsVisible = false;
                lvComuni.IsVisible = true;
                viewModel.NomeComune = entRicerca.Text;
                viewModel.LoadItemsCommand.Execute(null);
            }
            else
            {
                lvComuni.IsVisible = false;
            }
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

        void Sup_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if(!string.IsNullOrEmpty(entSupMin.Text))
                RangeSlider2.LowerValue = float.Parse(entSupMin.Text);

            if (!string.IsNullOrEmpty(entSupMax.Text))
                RangeSlider2.UpperValue = float.Parse(entSupMax.Text);
        }

        void RangeSlider2_LowerValueChanged(System.Object sender, System.EventArgs e)
        {
            entSupMin.Text = ((RangeSlider)sender).LowerValue.ToString();
        }

        void RangeSlider2_UpperValueChanged(System.Object sender, System.EventArgs e)
        {
            entSupMax.Text = ((RangeSlider)sender).UpperValue.ToString();
        }


        void Aff_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(entAffittoMin.Text)) { 
                entAffittoMin.Text = string.Format("{0:N0}", Decimal.Parse(entAffittoMin.Text));

            RangeSliderAffitto.LowerValue = float.Parse(entAffittoMin.Text);
            }
            if (!string.IsNullOrEmpty(entAffittoMax.Text))
            {
                entAffittoMax.Text = string.Format("{0:N0}", Decimal.Parse(entAffittoMax.Text));
                RangeSliderAffitto.UpperValue = float.Parse(entAffittoMax.Text);
            }
        }

        void RangeSliderAffitto_LowerValueChanged(System.Object sender, System.EventArgs e)
        {
            entAffittoMin.Text = ((RangeSlider)sender).LowerValue.ToString();
        }

        void RangeSliderAffitto_UpperValueChanged(System.Object sender, System.EventArgs e)
        {
            entAffittoMax.Text = ((RangeSlider)sender).UpperValue.ToString();
        }

        void Ven_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(entVenditaMin.Text))
            {
                entVenditaMin.Text = string.Format("{0:N0}", Decimal.Parse(entVenditaMin.Text));
                RangeSliderVendita.LowerValue = float.Parse(entVenditaMin.Text);
            }

            if (!string.IsNullOrEmpty(entVenditaMax.Text))
            {
                entVenditaMax.Text = string.Format("{0:N0}", Decimal.Parse(entVenditaMax.Text));
                RangeSliderVendita.UpperValue = float.Parse(entVenditaMax.Text);
            }
        }

        void RangeSliderVendita_LowerValueChanged(System.Object sender, System.EventArgs e)
        {
            entVenditaMin.Text = string.Format("{0:N0}", Decimal.Parse(((RangeSlider)sender).LowerValue.ToString()));
        }

        void RangeSliderVendita_UpperValueChanged(System.Object sender, System.EventArgs e)
        {
            entVenditaMax.Text = string.Format("{0:N0}", Decimal.Parse(((RangeSlider)sender).UpperValue.ToString()));
        }
    }
}