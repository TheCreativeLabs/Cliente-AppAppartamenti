using AppAppartamenti.ViewModels;
using AppAppartamentiApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using NavigationPage = Xamarin.Forms.NavigationPage;

namespace AppAppartamenti.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NuovoAppuntamento : ContentPage
    {
        AnnuncioDetailViewModel viewModel;
        Guid IdAnnuncio;

        public NuovoAppuntamento(Guid Id,bool IsEditable)
        {
            InitializeComponent();

            IdAnnuncio = Id;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            BindingContext = viewModel = await AnnuncioDetailViewModel.ExecuteLoadItemsCommandAsync(IdAnnuncio);

            scrollView.IsVisible = true;
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

        private async void BtnModifica_Clicked(object sender, EventArgs e)
        {
            try
            {
                //await Navigation.PopAsync();
                //await Navigation.PushAsync(new ModificaAnnuncio(viewModel.Item.Id.Value));
                
                await Navigation.PushModalAsync(new NavigationPage(new SelezioneProprieta(viewModel)));
            }
            catch (Exception Ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }
    }
}