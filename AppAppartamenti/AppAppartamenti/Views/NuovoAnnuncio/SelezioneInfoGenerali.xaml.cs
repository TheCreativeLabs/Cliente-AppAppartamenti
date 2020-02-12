using System;
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
    public partial class SelezioneInfoGenerali : ContentPage
    {
        AnnuncioDtoInput annuncio;
        AnnuncioDetailViewModel dtoToModify;

        public SelezioneInfoGenerali(AnnuncioDtoInput Annuncio, AnnuncioDetailViewModel dtoToModify)
        {
            this.dtoToModify = dtoToModify;
            InitializeComponent();
            lbl_nuovoAnnuncio.IsVisible = dtoToModify == null;
            lbl_modificaAnnuncio.IsVisible = dtoToModify != null;

            if (this.dtoToModify != null)
            {
                if (entSuperficie.Text == null)
                {
                    entSuperficie.Text = dtoToModify.Item.Superficie.ToString();
                }

                stpCamereLetto.Value = Convert.ToDouble(dtoToModify.Item.NumeroCameraLetto);
                lblCamereLettoCount.Text = dtoToModify.Item.NumeroCameraLetto.ToString();


                stpStanze.Value = Convert.ToDouble(dtoToModify.Item.NumeroAltreStanze);
                lblStanzeCount.Text = dtoToModify.Item.NumeroAltreStanze.ToString();


                stpBagni.Value = Convert.ToDouble(dtoToModify.Item.NumeroBagni);
                lblBagniCount.Text = dtoToModify.Item.NumeroBagni.ToString();

                stpCucine.Value = Convert.ToDouble(dtoToModify.Item.NumeroCucine);
                lblCucineCount.Text = dtoToModify.Item.NumeroCucine.ToString();

                stpGarage.Value = Convert.ToDouble(dtoToModify.Item.NumeroGarage);
                lblGarageCount.Text = dtoToModify.Item.NumeroGarage.ToString();

                stpParkingSpaces.Value = Convert.ToDouble(dtoToModify.Item.NumeroPostiAuto);
                lblParkingSpacesCount.Text = dtoToModify.Item.NumeroPostiAuto.ToString();

                //if(annuncio.Cantina == null)
                //{
                chkCantina.IsChecked = dtoToModify.Item.Cantina != null ? (bool)dtoToModify.Item.Cantina : false;
                //}
                //if (chkPiscina.IsChecked == null)
                //{
                chkPiscina.IsChecked = dtoToModify.Item.Piscina != null ? (bool)dtoToModify.Item.Piscina : false;
                //}
                //if (chkAscensore.IsChecked== null)
                //{
                chkAscensore.IsChecked = dtoToModify.Item.Ascensore != null ? (bool)dtoToModify.Item.Ascensore : false;
                //}
                //if (chkTerrazzo.IsChecked==null)
                //{
                chkTerrazzo.IsChecked = dtoToModify.Item.Balcone != null ? (bool)dtoToModify.Item.Balcone : false;
                //}
                //if (chkGiardino.IsChecked==null)
                //{
                chkGiardino.IsChecked = dtoToModify.Item.Giardino != null ? (bool)dtoToModify.Item.Giardino : false;
                //}
                //if (chkCondizionatori.IsChecked==null)
                //{
                chkCondizionatori.IsChecked = dtoToModify.Item.Condizionatori != null ? (bool)dtoToModify.Item.Condizionatori : false;
                //}
                //if (chkNoArchitecturalBarriers.IsChecked==null)
                //{
                chkNoArchitecturalBarriers.IsChecked = dtoToModify.Item.SenzaBarriereArchitettoniche != null ? (bool)dtoToModify.Item.SenzaBarriereArchitettoniche : false;
                //}
                //if (chkStairlifts.IsChecked==null)
                //{
                chkStairlifts.IsChecked = dtoToModify.Item.Montascale != null ? (bool)dtoToModify.Item.Montascale : false;
                //}
                //if (chkNoStepsInProperty.IsChecked==null)
                //{
                chkNoStepsInProperty.IsChecked = dtoToModify.Item.SenzaGradiniInternoProprieta != null ? (bool)dtoToModify.Item.SenzaGradiniInternoProprieta : false;
                //}

            }

            annuncio = Annuncio;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
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

        private async void BtnInfoGeneraliProcedi_Clicked(object sender, EventArgs e)
        {
            if (!(String.IsNullOrEmpty(entSuperficie.Text)))
            {
                annuncio.Superficie = double.Parse(entSuperficie.Text);
                annuncio.NumeroAltreStanze = int.Parse(stpStanze.Value.ToString());
                annuncio.NumeroBagni = int.Parse(stpBagni.Value.ToString());
                annuncio.NumeroCameraLetto = int.Parse(stpCamereLetto.Value.ToString());
                annuncio.NumeroCucine = int.Parse(stpCucine.Value.ToString());
                annuncio.NumeroGarage = int.Parse(stpGarage.Value.ToString());
                annuncio.NumeroPostiAuto = int.Parse(stpParkingSpaces.Value.ToString());
                annuncio.Cantina = chkCantina.IsChecked;
                annuncio.Piscina = chkPiscina.IsChecked;
                annuncio.Ascensore = chkAscensore.IsChecked;
                annuncio.Balcone = chkTerrazzo.IsChecked;
                annuncio.Giardino = chkGiardino.IsChecked;
                annuncio.Condizionatori = chkCondizionatori.IsChecked;
                annuncio.SenzaBarriereArchitettoniche = chkNoArchitecturalBarriers.IsChecked;
                annuncio.Montascale = chkStairlifts.IsChecked;
                annuncio.SenzaGradiniInternoProprieta = chkNoStepsInProperty.IsChecked;

                await Navigation.PushAsync(new SelezionePrezzo(annuncio, dtoToModify));
            }
            else
            {
                await DisplayAlert("Campo obbligatorio", "Inserire la superficie dell'immobile", "Ok");
            }

         
        }

        #region Stepper

            void StpCamereLetto_ValueChanged(object sender, ValueChangedEventArgs e)
            {
                lblCamereLettoCount.Text = stpCamereLetto.Value.ToString();
            }

            void StpStanze_ValueChanged(object sender, ValueChangedEventArgs e)
            {
                lblStanzeCount.Text = stpStanze.Value.ToString();
            }

            void StpBagni_ValueChanged(object sender, ValueChangedEventArgs e)
            {
                lblBagniCount.Text = stpBagni.Value.ToString();
            }

            void StpCucine_ValueChanged(object sender, ValueChangedEventArgs e)
            {
                lblCucineCount.Text = stpCucine.Value.ToString();
            }

            void StpGarage_ValueChanged(object sender, ValueChangedEventArgs e)
            {
                lblGarageCount.Text = stpGarage.Value.ToString();
            }

            void StpParkingSpaces_ValueChanged(object sender, ValueChangedEventArgs e)
            {
                lblParkingSpacesCount.Text = stpParkingSpaces.Value.ToString();
            }

        #endregion
    }
}