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
    public partial class SelezioneProprieta : ContentPage
    {
        AnnuncioDtoInput annuncio = new AnnuncioDtoInput();

        public SelezioneProprieta()
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

        private async void BtnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        async void LvTipologiaProprieta_Selected(object sender, SelectedItemChangedEventArgs args)
        {
            TipologiaProprieta proprieta = args.SelectedItem as TipologiaProprieta;

            if (proprieta == null || proprieta.Id == null)
                return;

            //Modifico l'annuncio.
            annuncio.IdTipologiaProprieta = proprieta.Id;

            //Manually deselect item.
            lvTipologiaProprieta.SelectedItem = null;

           await Navigation.PushAsync(new SelezioneTipologiaAnnuncio(annuncio));
            
        }
    }
}