﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppAppartamenti.Utility;
using AppAppartamenti.ViewModels;
using AppAppartamentiApiClient;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAppartamenti.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MieiAnnunci : ContentPage
    {
        AnnunciPersonaliViewModel viewModel;

        public MieiAnnunci()
        {
            InitializeComponent();

            BindingContext = viewModel = new AnnunciPersonaliViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<SelezioneFasceOrarie, string>(this, "AnnuncioCreato", async (obj, arg) =>
            {
                viewModel.Items.Clear();
                viewModel.LoadItemsCommand.Execute(null);
            });

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as AnnunciDtoOutput;

            if (item == null || item.Id == null)
                return;

            //EventoClient eventoClient = new EventoClient(ApiHelper.GetApiClient());
            //EventoDtoOutput dettaglioEvento = await eventoClient.GetEventoByIdAsync(new Guid(item.Id));

            //await Navigation.PushAsync(new EventoModifica(new EventoDetailViewModel(dettaglioEvento)));
            if (item.Id != null && item.Id != Guid.Empty)
            {
                // Manually deselect item.
                AnnunciiListView.SelectedItem = null;

                await Navigation.PushAsync(new DettaglioAnnuncio(item,true));
            }
        }

        async void BtnDeleteAd_Clicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            var item = btn.CommandParameter as AnnunciDtoOutput;

            bool answer = await DisplayAlert("Cancellare l'annuncio?", "Procedendo, l'annuncio verrà elimninato definitavamente.", "Si", "No");
            AnnunciClient annunciClient = new AnnunciClient(await Api.ApiHelper.GetApiClient());

            if (answer)
            {
                await annunciClient.DeleteAnnuncioAsync(item.Id.Value);
                viewModel.Items.Remove(item);
            }
        }

        async void BtnAdd_Clicked(object sender, EventArgs e)
        {
             await Navigation.PushModalAsync(new NavigationPage( new SelezioneProprieta(null))); //è un nuovo annuncio, non devo passare l'annuncio da modificare
        }
    }
}