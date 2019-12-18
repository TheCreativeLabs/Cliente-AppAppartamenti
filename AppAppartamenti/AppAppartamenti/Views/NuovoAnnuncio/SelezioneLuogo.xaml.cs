using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppAppartamenti.ViewModels;
using AppAppartamenti.Views.NuovoAnnuncio;
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
        AnnuncioDetailViewModel dtoToModify;

        public SelezioneLuogo(AnnuncioDtoInput Annuncio, AnnuncioDetailViewModel dtoToModify)
        {
            this.dtoToModify = dtoToModify;
            InitializeComponent();

            annuncio = Annuncio;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if ( dtoToModify != null)
            {
                if(lvComuni.SelectedItem == null) { 
                    lvComuni.SelectedItem = new ComuneDto()
                        {
                            NomeComune = dtoToModify.Item.NomeComune,
                            CodiceComune = dtoToModify.Item.CodiceComune
                        };
                }
                if (entIndirizzo.Text == null && !String.IsNullOrEmpty(dtoToModify.Item.Indirizzo))
                {
                    entIndirizzo.Text = dtoToModify.Item.Indirizzo;
                    await setMapLocation();
                }

                btnIndirizzoProcedi.IsEnabled = true;
            }
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

            await Navigation.PushModalAsync(new ValidazioneMappa(entCercaComune.Text, entIndirizzo.Text),true);

            //await Navigation.PushAsync(new SelezioneInfoGenerali(annuncio, dtoToModify));
        }

        private async Task setMapLocation()
        {
            

            //btnIndirizzoProcedi.IsVisible = true;
        }

        private void EntCercaComune_TextChanged(object sender, TextChangedEventArgs e)
        {
            //refresh della lista dei comuni
            var listaComuni = new ListaComuniViewModel(entCercaComune.Text);
            listaComuni.LoadItemsCommand.Execute(null);
            lvComuni.ItemsSource = listaComuni.Items;
            lvComuni.IsVisible = true;
            entIndirizzo.IsVisible = false;
            lblIndirizzo.IsVisible = false;
            btnIndirizzoProcedi.IsVisible = false;
        }

        async void LvComuni_Selected(object sender, SelectedItemChangedEventArgs args)
        {
            ComuneDto comune = args.SelectedItem as ComuneDto;

            if (comune == null || comune.CodiceComune == null)
                return;

            //modifico l'annuncio
            annuncio.CodiceComune = comune.CodiceComune.Value;

            //modifico la textbox del comune inserendo il nome completo del comune
            entCercaComune.Text = comune.NomeComune;

            //nascondo la lista dei comuni e mostro la entry dell'indirizzo
            lvComuni.IsVisible = false;

            entIndirizzo.IsVisible = true;
            lblIndirizzo.IsVisible = true;
            btnIndirizzoProcedi.IsVisible = true;

            // Manually deselect item.
            lvComuni.SelectedItem = null;
        }

        private async void EntIndirizzo_Unfocused(object sender, FocusEventArgs e)
        {
            await setMapLocation();

            btnIndirizzoProcedi.IsEnabled = true;
        }
    }
}