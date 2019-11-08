using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppAppartamenti.Api;
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
    public partial class SelezioneDescrizione : ContentPage
    {
        AnnuncioDtoInput annuncio = new AnnuncioDtoInput();

        public SelezioneDescrizione(AnnuncioDtoInput Annuncio)
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


        private async void BtnImmaginiProcedi_Clicked(object sender, EventArgs e)
        {
            annuncio.Descrizione = edtDescrizione.Text;

            AnnunciClient annunciClient = new AnnunciClient(ApiHelper.GetApiClient());
            await annunciClient.InsertAnnuncioAsync(annuncio);

            await Navigation.PopModalAsync();
        }
    }
}