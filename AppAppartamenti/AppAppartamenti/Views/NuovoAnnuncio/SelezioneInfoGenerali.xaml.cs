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

        public SelezioneInfoGenerali(AnnuncioDtoInput Annuncio)
        {
            InitializeComponent();

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
            }

            annuncio.NumeroAltreStanze = int.Parse(stpStanze.Value.ToString());
            annuncio.NumeroBagni = int.Parse(stpBagni.Value.ToString());
            annuncio.NumeroCameraLetto = int.Parse(stpCamereLetto.Value.ToString());
            annuncio.NumeroCucine = int.Parse(stpCucine.Value.ToString());
            annuncio.NumeroGarage = int.Parse(stpGarage.Value.ToString());
            annuncio.Cantina = chkCantina.IsChecked;
            annuncio.Piscina = chkPiscina.IsChecked;
            annuncio.Ascensore = chkAscensore.IsChecked;
            annuncio.Balcone = chkTerrazzo.IsChecked;
            annuncio.Giardino = chkGiardino.IsChecked;
            annuncio.Condizionatori = chkCondizionatori.IsChecked;

            await Navigation.PushAsync(new SelezionePrezzo(annuncio));
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

        #endregion
    }
}