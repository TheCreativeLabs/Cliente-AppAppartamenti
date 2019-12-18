﻿using System;
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
    public partial class ListaAnnunci : ContentPage
    {
        AnnunciViewModel viewModel;
        RicercaModel FiltriRicerca;

        public ListaAnnunci(RicercaModel FiltriRicercaParam)
        {
            InitializeComponent();

            FiltriRicerca = FiltriRicercaParam;

            BindingContext = viewModel = new AnnunciViewModel(TipiRicerca.Tutti, FiltriRicerca);

            entRicerca.Text = FiltriRicerca.Comune.NomeComune;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }


        async void OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            await Navigation.PushModalAsync(new Ricerca(FiltriRicerca));
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as AnnunciDtoOutput;

            if (item == null || item.Id == null)
                return;

            if (item.Id != null && item.Id != Guid.Empty)
            {
                // Manually deselect item.
                AnnunciiListView.SelectedItem = null;

                await Navigation.PushAsync(new DettaglioAnnuncio(item.Id.Value,false));
            }
        }

        async void BtnAdd_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage( new SelezioneProprieta(null)));
        }

        private async void BtnBack_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new MainPage());
            }
            catch (Exception Ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }
    }
}