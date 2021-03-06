﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppAppartamenti.Api;
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
    public partial class SelezioneDescrizione : ContentPage
    {
        AnnuncioDtoInput annuncio = new AnnuncioDtoInput();
        AnnuncioDetailViewModel dtoToModify;

        public SelezioneDescrizione(AnnuncioDtoInput Annuncio, AnnuncioDetailViewModel dtoToModify)
        {
            this.dtoToModify = dtoToModify;
            InitializeComponent();
            lbl_nuovoAnnuncio.IsVisible = dtoToModify == null;
            lbl_modificaAnnuncio.IsVisible = dtoToModify != null;

            annuncio = Annuncio;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (edtDescrizione.Text == null && dtoToModify != null && dtoToModify.Item != null)
            {
                edtDescrizione.Text = dtoToModify.Item.Descrizione;
            }

            edtDescrizione.Focus();
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

        private async void BtnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void BtnDescrizioneProcedi_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(edtDescrizione.Text))
            {
                await DisplayAlert("Campo obbligatori", "Inserire la descrizione", "Ok");
                return;
            }

            annuncio.Descrizione = edtDescrizione.Text;
            if(dtoToModify != null && dtoToModify.Item != null)
            {
                if(dtoToModify.Item.Cancellato != null)
                {
                    annuncio.Cancellato = (bool)dtoToModify.Item.Cancellato;
                }
                if (dtoToModify.Item.Completato != null)
                {
                    annuncio.Completato = (bool)dtoToModify.Item.Completato;
                }
                if (dtoToModify.Item.Disponibile != null)
                {
                    annuncio.Disponibile = (bool)dtoToModify.Item.Disponibile;
                }
                if (dtoToModify.Item.Piano != null)
                {
                    annuncio.Piano = (int)dtoToModify.Item.Piano;
                }
                if (dtoToModify.Item.UltimoPiano!=null)
                {
                    annuncio.UltimoPiano = (bool)dtoToModify.Item.UltimoPiano;
                }
            }

            await Navigation.PushAsync(new SelezioneFasceOrarie(annuncio, dtoToModify));
        }
    }
}