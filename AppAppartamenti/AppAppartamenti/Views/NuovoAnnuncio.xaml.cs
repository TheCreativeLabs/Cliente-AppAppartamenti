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
    public partial class NuovoAnnuncio : ContentPage
    {
        AnnuncioDtoInput annuncio = new AnnuncioDtoInput();
        private List<byte[]> listaImmagini = new List<byte[]>();
        int currentImage = 0;

        public NuovoAnnuncio()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //Carico la lista delle proprietà
            var listaTipologiaProprieta = new TipologiaProprietaViewModel();
            listaTipologiaProprieta.LoadItemsCommand.Execute(null);
            lvTipologiaProprieta.ItemsSource = listaTipologiaProprieta.Items;
        }

        #region Bottoni
        private async void BtnBack_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (stkPassaggio1.IsVisible)
                {
                    //Nella prima pagina nascondo il bottone "indietro"
                    btnBack.IsVisible = false;

                }
                else if (stkPassaggio2.IsVisible)
                {
                    stkPassaggio2.IsVisible = false;
                    stkPassaggio1.IsVisible = true;
                    btnBack.IsVisible = false;
                }
                else if (stkPassaggio3.IsVisible)
                {
                    stkPassaggio2.IsVisible = true;
                    stkPassaggio3.IsVisible = false;
                }
                else if (stkPassaggio4.IsVisible)
                {
                    stkPassaggio3.IsVisible = true;
                    stkPassaggio4.IsVisible = false;
                    entCercaComune.IsVisible = true;
                    entIndirizzo.IsVisible = false;
                    map.IsVisible = false;
                    btnInfoGeneraliProcedi.IsVisible = false;
                }
                else if (stkPassaggio5.IsVisible)
                {
                    stkPassaggio4.IsVisible = true;
                    stkPassaggio5.IsVisible = false;
                }
                else if (stkPassaggio6.IsVisible)
                {
                    stkPassaggio5.IsVisible = true;
                    stkPassaggio6.IsVisible = false;
                }
                else if (stkPassaggio7.IsVisible)
                {
                    stkPassaggio6.IsVisible = true;
                    stkPassaggio7.IsVisible = false;
                }
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
            stkPassaggio4.IsVisible = true;
            stkPassaggio3.IsVisible = false;
        }

        private async void BtnPrezzoProcedi_Clicked(object sender, EventArgs e)
        {
            stkPassaggio5.IsVisible = false;
            stkPassaggio6.IsVisible = true;
        }

        private async void BtnImmaginiProcedi_Clicked(object sender, EventArgs e)
        {
            stkPassaggio6.IsVisible = false;
            stkPassaggio7.IsVisible = true;
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

            stkPassaggio4.IsVisible = false;
            stkPassaggio5.IsVisible = true;
        }
        #endregion

        #region Entry

        private async void EntIndirizzo_Unfocused(object sender, EventArgs e)
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

            btnIndirizzoProcedi.IsVisible = true;
        }

        private void EntCercaComune_TextChanged(object sender, TextChangedEventArgs e)
        {
            //refresh della lista dei comuni
            var listaComuni = new ListaComuniViewModel(entCercaComune.Text);
            listaComuni.LoadItemsCommand.Execute(null);
            lvComuni.ItemsSource = listaComuni.Items;
        }

        #endregion

        #region ListView
        async void LvTipologiaProprieta_Selected(object sender, SelectedItemChangedEventArgs args)
        {
            TipologiaProprieta proprieta = args.SelectedItem as TipologiaProprieta;

            if (proprieta == null || proprieta.Id == null)
                return;

            //Modifico l'annuncio.
            annuncio.IdTipologiaProprieta = proprieta.Id;

            //Nascondo lo stack precedente e mostro il prossimo.
            stkPassaggio1.IsVisible = false;
            btnBack.IsVisible = true;
            stkPassaggio2.IsVisible = true;

            //Carico la lista della tipologia annuncio
            var listaTipologiaAnnunci = new TipologiaAnnunciViewModel();
            listaTipologiaAnnunci.LoadItemsCommand.Execute(null);
            lvTipologiaAnnuncio.ItemsSource = listaTipologiaAnnunci.Items;

            //Manually deselect item.
            lvTipologiaProprieta.SelectedItem = null;
        }

        async void LvTipologiaAnnuncio_Selected(object sender, SelectedItemChangedEventArgs args)
        {
            TipologiaAnnuncio tipologiaAnnuncio = args.SelectedItem as TipologiaAnnuncio;

            if (tipologiaAnnuncio == null || tipologiaAnnuncio.Id == null)
                return;

            annuncio.IdTipologiaAnnuncio = tipologiaAnnuncio.Id;

            stkPassaggio2.IsVisible = false;
            stkPassaggio3.IsVisible = true;
            btnBack.IsVisible = true;

            // Manually deselect item.
            lvTipologiaAnnuncio.SelectedItem = null;
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

            // Manually deselect item.
            lvComuni.SelectedItem = null;
        }
        #endregion

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

        

        async void OnPickPhotoButtonClicked(object sender, EventArgs e)
        {
            (sender as Button).IsEnabled = false;

            Stream stream = await DependencyService.Get<IPhotoPickerService>().GetImageStreamAsync();
            if (stream != null)
            {
                switch (currentImage)
                {
                    case 0:
                        img1.Source = ImageSource.FromStream(() => stream);
                        btnImg1.IsVisible = false;
                        frmImg2.IsVisible = true;
                        currentImage += 1;
                        break;

                    case 1:
                        img2.Source = ImageSource.FromStream(() => stream);
                        img2.IsVisible = true;
                        btnImg2.IsVisible = false;
                        frmImg3.IsVisible = true;

                        currentImage += 1;
                        break;

                    case 2:
                        img3.Source = ImageSource.FromStream(() => stream);
                        btnImg3.IsVisible = false;
                        frmImg4.IsVisible = true;

                        currentImage += 1;
                        break;

                    case 3:
                        img4.Source = ImageSource.FromStream(() => stream);
                        btnImg4.IsVisible = false;
                        frmImg5.IsVisible = true;

                        currentImage += 1;
                        break;

                    case 4:
                        img5.Source = ImageSource.FromStream(() => stream);
                        btnImg5.IsVisible = false;
                        frmImg6.IsVisible = true;

                        currentImage += 1;
                        break;

                    case 5:
                        img6.Source = ImageSource.FromStream(() => stream);
                        btnImg6.IsVisible = false;
                        currentImage += 1;
                        break;

                    default:
                        break;
                }

                //Aggiungo l'immagine alla lista degli annunci
                listaImmagini.Add(stream.ReadAsBytes());
            }

            (sender as Button).IsEnabled = true;
        }
    }
}