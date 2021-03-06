﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppAppartamenti.Api;
using AppAppartamenti.Notify;
using AppAppartamenti.ViewModels;
using AppAppartamentiApiClient;
using Newtonsoft.Json;
using Plugin.Media;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAppartamenti.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : ContentPage
    {
        RicercaModel RicercaModel;
        AnnunciRecentiViewModel viewModel;

        public Home()
        {
            InitializeComponent();

            BindingContext = viewModel = new AnnunciRecentiViewModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            NavigationPage.SetHasNavigationBar(this, false);

            //if (Device.RuntimePlatform == Device.Android)
            //{
            //    DependencyService.Get<IStatusBarStyleManager>().SetColoredStatusBar("#2196F3");
            //}

            try
            {
                MessagingCenter.Subscribe<Ricerca, string>(this, "Ricerca", async (sender, arg) =>
                {
                    RicercaModel = null;
                    if (!string.IsNullOrEmpty(arg))
                    {
                        RicercaModel = JsonConvert.DeserializeObject<RicercaModel>(arg);
                    }
                });

                if (RicercaModel != null)
                {
                    await Navigation.PushAsync(new ListaAnnunci(RicercaModel));
                    RicercaModel = null;
                }

                if (!viewModel.Items.Any())
                    viewModel.LoadItemsCommand.Execute(null);
            }
            catch (Exception ex)
            {
                await Navigation.PushAsync(new ErrorPage());
            }
        }

        private async void entRicerca_Focused(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Ricerca());
        }

        private async void btnNuovoAnnuncio_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new SelezioneProprieta(null))); //è un nuovo annuncio, non devo passare l'annuncio da modificare
        }

        async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (!e.CurrentSelection.Any())
                    return;

                var item = e.CurrentSelection.First() as AnnunciDtoOutput;

                if (item.Id != null && item.Id != Guid.Empty)
                {
                    await Navigation.PushAsync(new DettaglioAnnuncio(item, false));
                }

                // Manually deselect item.
                cvRecenti.SelectedItem = null;
            }
            catch (Exception ex)
            {
                await Navigation.PushAsync(new ErrorPage());
            }
        }
    }
}