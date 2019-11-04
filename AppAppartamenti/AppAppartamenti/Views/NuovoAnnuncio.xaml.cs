using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppAppartamenti.ViewModels;
using AppAppartamentiApiClient;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAppartamenti.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NuovoAnnuncio : ContentPage
    {
        string currentVisibleStack;

        public NuovoAnnuncio()
        {
            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var listaTipologiaProprieta = new TipologiaProprietaViewModel();
            listaTipologiaProprieta.LoadItemsCommand.Execute(null);
            lvTipologiaProprieta.ItemsSource = listaTipologiaProprieta.Items;
        }

        async void lvTipologiaProprieta_Selected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as TipologiaProprieta;

            if (item == null || item.Id == null)
                return;

            stkPassaggio1.IsVisible = false;
            btnBack.IsVisible = true;
            stkPassaggio2.IsVisible = true;

            var listaTipologiaAnnunci = new TipologiaAnnunciViewModel();
            listaTipologiaAnnunci.LoadItemsCommand.Execute(null);
            lvTipologiaAnnuncio.ItemsSource = listaTipologiaAnnunci.Items;

            // Manually deselect item.
            lvTipologiaProprieta.SelectedItem = null;
        }

        async void lvTipologiaAnnuncio_Selected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as TipologiaAnnuncio;

            if (item == null || item.Id == null)
                return;

            stkPassaggio2.IsVisible = false;
            btnBack.IsVisible = true;
            stkPassaggio3.IsVisible = true;


            // Manually deselect item.
            lvTipologiaAnnuncio.SelectedItem = null;
        }

        async void BtnBack_Clicked(object sender, EventArgs e)
        {
            if (stkPassaggio1.IsVisible)
            {
                btnBack.IsVisible = false;
            }else if (stkPassaggio2.IsVisible)
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
        }

        async void BtnCancel_Clicked(object sender, EventArgs e)
        {
          await Navigation.PopModalAsync();
        }
    }
}