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
    public partial class Ricerca : ContentPage
    {
        public Ricerca()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private async void btnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void btnRicerca_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListaAnnunci());
        }

        private void EntRicerca_TextChanged(object sender, TextChangedEventArgs e)
        {
            //refresh della lista dei comuni
            var listaComuni = new ListaComuniViewModel(entRicerca.Text);
            listaComuni.LoadItemsCommand.Execute(null);
            lvComuni.ItemsSource = listaComuni.Items;

            stkRicercaAggiuntiva.IsVisible = false;
            lvComuni.IsVisible = true;
            btnRicerca.IsVisible = false;
        }

        async void LvComuni_Selected(object sender, SelectedItemChangedEventArgs args)
        {
            Comuni comune = args.SelectedItem as Comuni;

            if (comune == null || comune.CodiceComune == null)
                return;

            //modifico la textbox del comune inserendo il nome completo del comune
            entRicerca.Text = comune.NomeComune;

            lvComuni.IsVisible = false;
            stkRicercaAggiuntiva.IsVisible = true;
            btnRicerca.IsVisible = true;

            lvComuni.SelectedItem = null;
        }
    }
}