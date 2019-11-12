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
    public partial class SelezioneTipologiaAnnuncio : ContentPage
    {
        AnnuncioDtoInput annuncio = new AnnuncioDtoInput();

        TipologiaAnnunciViewModel viewModel;

        public SelezioneTipologiaAnnuncio(AnnuncioDtoInput Annuncio)
        {
            InitializeComponent();

            annuncio = Annuncio;
            BindingContext = viewModel = new TipologiaAnnunciViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //Carico la lista della tipologia annuncio
            //var listaTipologiaAnnunci = new TipologiaAnnunciViewModel();
            //listaTipologiaAnnunci.LoadItemsCommand.Execute(null);
            //lvTipologiaAnnuncio.ItemsSource = listaTipologiaAnnunci.Items;
            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);

            try
            {
                ((NavigationPage)this.Parent).BarBackgroundColor = Color.White;
                ((NavigationPage)this.Parent).BarTextColor = Color.Black;
                NavigationPage.SetHasNavigationBar(this, true);
            }
            catch (Exception ex)
            {

            }

            staBody.IsVisible = true;
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

        async void LvTipologiaAnnuncio_Selected(object sender, SelectedItemChangedEventArgs args)
        {
            TipologiaAnnuncio tipologiaAnnuncio = args.SelectedItem as TipologiaAnnuncio;

            if (tipologiaAnnuncio == null || tipologiaAnnuncio.Id == null)
                return;

            annuncio.IdTipologiaAnnuncio = tipologiaAnnuncio.Id;

            // Manually deselect item.
            //lvTipologiaAnnuncio.SelectedItem = null;

            await Navigation.PushAsync(new SelezioneLuogo(annuncio));
        }

        async void BtnAvanti_Clicked(object sender, EventArgs e)
        {
            if (rbList.SelectedItem != null)
            {
                TipologiaAnnuncio tipologiaAnnuncio = viewModel.listaAnnunci.Where(x => (string)x.Descrizione == (string)rbList.SelectedItem).FirstOrDefault();
                annuncio.IdTipologiaAnnuncio = tipologiaAnnuncio.Id;
                await Navigation.PushAsync(new SelezioneLuogo(annuncio));
            }
        }
    }
}