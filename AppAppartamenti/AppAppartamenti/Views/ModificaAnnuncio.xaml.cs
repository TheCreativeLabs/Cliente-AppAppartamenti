using AppAppartamenti.ViewModels;
using AppAppartamentiApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using NavigationPage = Xamarin.Forms.NavigationPage;

namespace AppAppartamenti.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModificaAnnuncio : ContentPage
    {
        AnnuncioDetailViewModel viewModel;
        Guid IdAnnuncio;
        TipologiaAnnunciViewModel modelTipologiaAnnunci;
        TipologiaProprietaViewModel modelTipologiaProprieta;


        public ModificaAnnuncio(Guid Id)
        {
            InitializeComponent();
            modelTipologiaAnnunci = new TipologiaAnnunciViewModel();
            modelTipologiaProprieta = new TipologiaProprietaViewModel();

            IdAnnuncio = Id;
            
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (modelTipologiaAnnunci.Items.Count == 0)
                modelTipologiaAnnunci.LoadItemsCommand.Execute(null);

            if (modelTipologiaProprieta.Items.Count == 0)
                modelTipologiaProprieta.LoadItemsCommand.Execute(null);

            BindingContext = viewModel = await AnnuncioDetailViewModel.ExecuteLoadItemsCommandAsync(IdAnnuncio);


            rbList.ItemsSource = modelTipologiaAnnunci.Items;
            rbList.SelectedItem = modelTipologiaAnnunci.Items.Where(x => x == viewModel.Item.TipologiaAnnuncio).FirstOrDefault();

            selProprList.ItemsSource = modelTipologiaProprieta.Items;
            selProprList.SelectedItem = modelTipologiaProprieta.Items.Where(x => x == viewModel.Item.TipologiaProprieta).FirstOrDefault();

            //luogo
            lvComuni.SelectedItem = new ComuneDto() { NomeComune = viewModel.Item.NomeComune,
                                                      CodiceComune = viewModel.Item.CodiceComune};
            if ( !String.IsNullOrEmpty(viewModel.Item.Indirizzo) )
            {
                entIndirizzoModifica.Text = viewModel.Item.Indirizzo;
                await setMapLocation(); 
            }

            scrollView.IsVisible = true;
            scrollView.HeightRequest = scrollView.Content.Height;
        }

        //async void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
        //{
            //if (e.ScrollY > 20)
            //{
            //    stkHeader.IsVisible = false;
            //    NavigationPage.SetHasNavigationBar(this, true);
            //}
            //else
            //{
            //    NavigationPage.SetHasNavigationBar(this, false);
            //    stkHeader.IsVisible = true;

            //}
        //}

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

        #region Luogo

        private void EntCercaComune_TextChanged(object sender, TextChangedEventArgs e)
        {
            //refresh della lista dei comuni
            var listaComuni = new ListaComuniViewModel(entCercaComune.Text);
            listaComuni.LoadItemsCommand.Execute(null);
            lvComuni.ItemsSource = listaComuni.Items;
            lvComuni.IsVisible = true;
            entIndirizzoModifica.IsVisible = false;
        }

        async void LvComuni_Selected(object sender, SelectedItemChangedEventArgs args)
        {
            ComuneDto comune = args.SelectedItem as ComuneDto;

            if (comune == null || comune.CodiceComune == null)
                return;

            //modifico l'annuncio
            viewModel.Item.CodiceComune = comune.CodiceComune.Value;

            //modifico la textbox del comune inserendo il nome completo del comune
            entCercaComune.Text = comune.NomeComune;

            //nascondo la lista dei comuni e sbianco la entry dell'indirizzo
            lvComuni.IsVisible = false;

            entIndirizzoModifica.IsVisible = true;
            entIndirizzoModifica.Text = "";

            //nascondo la mappa
            mapModifica.IsVisible = false;

            // Manually deselect item.
            lvComuni.SelectedItem = null;
        }

        private async Task setMapLocation()
        {
            //ottengo la posizione dell'indirizzo.
            List<Position> postionList = new List<Position>();

            try
            {
                postionList = (await (new Geocoder()).GetPositionsForAddressAsync($"{entIndirizzoModifica.Text} , {entCercaComune.Text}")).ToList();
            }
            catch (Exception ex)
            {
            }

            if (postionList.Count != 0)
            {
                //Se la mappa ha già dei Pin presenti li cancello.
                if (mapModifica.Pins.Count > 0) mapModifica.Pins.RemoveAt(0);

                //Ottengo la posizione e la mostro nella mappa.
                var position = postionList.FirstOrDefault<Position>();
                mapModifica.Pins.Add(new Pin() { Position = position, Label = "Indirizzo" });
                mapModifica.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromMiles(0.1)));
                mapModifica.IsVisible = true;
            }

            //btnIndirizzoProcedi.IsVisible = true;
        }

        private async void EntIndirizzo_Unfocused(object sender, FocusEventArgs e)
        {
            await setMapLocation();
        }

        #endregion

        #region StepperInfoGenerali

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