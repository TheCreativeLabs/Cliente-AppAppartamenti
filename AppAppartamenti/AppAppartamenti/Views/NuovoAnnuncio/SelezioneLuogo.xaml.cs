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
        ListaComuniViewModel viewModel;

        bool IsMappaValid;

        public SelezioneLuogo(AnnuncioDtoInput Annuncio, AnnuncioDetailViewModel dtoToModify)
        {
            this.dtoToModify = dtoToModify;
            InitializeComponent();

            annuncio = Annuncio;

            if (dtoToModify != null)
            {
                if (lvComuni.SelectedItem == null)
                {
                    lvComuni.SelectedItem = new ComuneDto()
                    {
                        NomeComune = dtoToModify.Item.NomeComune,
                        CodiceComune = dtoToModify.Item.CodiceComune
                    };
                }
                if (entIndirizzo.Text == null && !String.IsNullOrEmpty(dtoToModify.Item.Indirizzo))
                {
                    entIndirizzo.Text = dtoToModify.Item.Indirizzo;
                }
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<ValidazioneMappa, string>(this, "Valida", async (sender, arg) =>
            {
                IsMappaValid = false;
                if (!string.IsNullOrEmpty(arg))
                {
                    IsMappaValid = true;
                }
            });

            if (IsMappaValid)
            {
                IsMappaValid = false;
                await Navigation.PushAsync(new SelezioneInfoGenerali(annuncio, dtoToModify));
            }

            BindingContext = viewModel = new ListaComuniViewModel();

           
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
            if(string.IsNullOrEmpty(entIndirizzo.Text) || string.IsNullOrEmpty(entCercaComune.Text))
            {
                await DisplayAlert("Attenzione", "Valorizzare il comune e l'indirizzo", "OK");
            }
            else
            {
                annuncio.Indirizzo = entIndirizzo.Text;

                Position position = (await (new Geocoder()).GetPositionsForAddressAsync($"{entIndirizzo.Text} , {entCercaComune.Text}")).FirstOrDefault();

                annuncio.CoordinateGeografiche = $"{position.Latitude.ToString()};{position.Longitude.ToString()}";
                await Navigation.PushModalAsync(new ValidazioneMappa(position,entCercaComune.Text, entIndirizzo.Text), true);
            }
        }

        private void EntCercaComune_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(entCercaComune.Text.Length >= 3) { 
                viewModel.NomeComune = entCercaComune.Text;
                viewModel.LoadItemsCommand.Execute(null);
                lvComuni.IsVisible = true;
                entIndirizzo.IsVisible = false;
                lblIndirizzo.IsVisible = false;
                btnIndirizzoProcedi.IsVisible = false;
            }
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
    }
}