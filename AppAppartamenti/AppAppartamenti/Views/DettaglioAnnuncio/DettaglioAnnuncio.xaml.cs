using AppAppartamenti.ViewModels;
using AppAppartamentiApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using NavigationPage = Xamarin.Forms.NavigationPage;

namespace AppAppartamenti.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DettaglioAnnuncio : ContentPage
    {
        AnnuncioDetailViewModel viewModel;
        Guid IdAnnuncio;
        bool IsEditable;

        public DettaglioAnnuncio(Guid Id,bool IsEditableParam)
        {
            InitializeComponent();
            IsEditable = IsEditableParam;

            IdAnnuncio = Id;

            btnModifica.IsVisible = IsEditable;
            btnModificaNavBar.IsVisible = IsEditable;

            stkPulsanti.IsVisible = !IsEditable;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            BindingContext = viewModel = await AnnuncioDetailViewModel.ExecuteLoadItemsCommandAsync(IdAnnuncio);

            scrollView.IsVisible = true;

            if (!IsEditable)
            {
                btnAddPreferito.IsVisible = !viewModel.Item.FlagPreferito.Value;
                btnRemovePreferito.IsVisible = viewModel.Item.FlagPreferito.Value;
                btnAddPreferitoNav.IsVisible = !viewModel.Item.FlagPreferito.Value;
                btnRemovePreferitoNav.IsVisible = viewModel.Item.FlagPreferito.Value;
            }
        }

        async void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
        {
            if (e.ScrollY > 20)
            {
                stkHeader.IsVisible = false;
                NavigationPage.SetHasNavigationBar(this, true);
            }
            else
            {
                NavigationPage.SetHasNavigationBar(this, false);
                stkHeader.IsVisible = true;
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


        private async void BtnAddPreferito_Clicked(object sender, EventArgs e)
        {
            try
            {
                btnRemovePreferito.IsVisible = true;
                btnAddPreferito.IsVisible = false;
                btnRemovePreferitoNav.IsVisible = true;
                btnAddPreferitoNav.IsVisible = false;
                AnnunciClient annunciClient = new AnnunciClient(Api.ApiHelper.GetApiClient());
                await annunciClient.AggiungiPreferitoAsync(viewModel.Item.Id.Value);
            }
            catch (Exception Ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }

        private async void BtnRemovePreferito_Clicked(object sender, EventArgs e)
        {
            try
            {
                btnRemovePreferito.IsVisible = false;
                btnAddPreferito.IsVisible = true;
                btnAddPreferitoNav.IsVisible = false;
                btnRemovePreferitoNav.IsVisible = true;
                AnnunciClient annunciClient = new AnnunciClient(Api.ApiHelper.GetApiClient());
                await annunciClient.RimuoviPreferitoAsync(viewModel.Item.Id.Value);
            }
            catch (Exception Ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }

        private async void BtnAppuntamento_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushModalAsync(new NuovoAppuntamento(IdAnnuncio));
            }
            catch (Exception Ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }

        private async void BtnModifica_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushModalAsync(new NavigationPage(new SelezioneProprieta(viewModel)));
            }
            catch (Exception Ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }

        private async void BtnShare_Clicked(object sender, EventArgs e)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Uri = $"{AppSetting.SiteApp}/Annunci/Detail/{viewModel.Item.Id}",
                Title = "Condividi il link"
            });
        }
    }
}