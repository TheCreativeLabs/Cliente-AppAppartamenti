﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppAppartamenti.ViewModels;
using AppAppartamentiApiClient;
using DependencyServiceDemos;
using Plugin.InputKit.Shared.Controls;
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
        AnnuncioDetailViewModel dtoToModify;

        TipologiaProprietaViewModel viewModel;

        public SelezioneProprieta(AnnuncioDetailViewModel annuncioDetailViewModel)
        {
            dtoToModify = annuncioDetailViewModel;
            InitializeComponent();

            BindingContext = viewModel = new TipologiaProprietaViewModel();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            //Carico la lista delle proprietà
            if (viewModel.Items.Count == 0)
                await viewModel.ExecuteLoadItemsCommand();

            if (dtoToModify!=null)
            {
                rbList.SelectedItem = viewModel.Items.Where(x => x == dtoToModify.Item.TipologiaProprieta).FirstOrDefault(); //FIXME CON TRADUZIONI, FUNZIONA SOLO IN ITALIANO
            }

            //try
            //{
            //    ((NavigationPage)this.Parent).BarBackgroundColor = Color.White;
            //    ((NavigationPage)this.Parent).BarTextColor = Color.Black;
            //    NavigationPage.SetHasNavigationBar(this, true);
            //}
            //catch (Exception ex)
            //{

            //}

            stkBody.IsVisible = true;
        }

        private async void BtnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        async void BtnAvanti_Clicked(object sender, EventArgs e)
        {
            if (rbList.SelectedItem != null)
            {
                TipologiaProprieta tipologiaProprieta =  viewModel.translationsMap[(string) rbList.SelectedItem];
                //TipologiaProprieta tipologiaProprieta = viewModel.listaProprieta.Where(x => x.Descrizione == rbList.SelectedItem).FirstOrDefault();
                annuncio.IdTipologiaProprieta =(Guid) tipologiaProprieta.Id;
                await Navigation.PushAsync(new SelezioneTipologiaAnnuncio(annuncio, dtoToModify));
            }
        }
    }
}