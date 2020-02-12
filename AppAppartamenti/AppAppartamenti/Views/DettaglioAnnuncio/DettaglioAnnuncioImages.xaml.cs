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

        protected async override void OnAppearing()
        {
            base.OnAppearing();

                CarouselImagesProgress.Text = "1/" + viewModel.Items.Count;
        }

        private async void BtnBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        void OnPositionChanged(object sender, PositionChangedEventArgs e)
        {
            int previousPosition = e.PreviousPosition;
            int currentPosition = e.CurrentPosition;
            CarouselImagesProgress.Text = (currentPosition + 1) + "/" + viewModel.Items.Count;
        }
    }
}
