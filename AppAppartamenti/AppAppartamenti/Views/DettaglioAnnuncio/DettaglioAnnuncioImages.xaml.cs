using System;
using System.Collections.Generic;
using AppAppartamenti.ViewModels;
using Xamarin.Forms;

namespace AppAppartamenti.Views
{
    public partial class DettaglioAnnuncioImages : ContentPage
    {
        AnnuncioimmaginiViewModel viewModel;

        public DettaglioAnnuncioImages(AnnuncioimmaginiViewModel Immagini)
        {
            InitializeComponent();

            BindingContext = viewModel = Immagini;
        }


        private async void BtnBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
