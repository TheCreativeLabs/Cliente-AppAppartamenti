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
    public partial class DettaglioAnnuncio : ContentPage
    {
        AnnuncioDetailViewModel viewModel;
        Guid IdAnnuncio;
        public DettaglioAnnuncio(Guid Id)
        {
            InitializeComponent();

            IdAnnuncio = Id;

     
            Carousel.IsVisible = true;

          

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            BindingContext = viewModel = await AnnuncioDetailViewModel.ExecuteLoadItemsCommandAsync(IdAnnuncio);
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
    }
}