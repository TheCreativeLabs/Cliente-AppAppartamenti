using System;
using System.Collections.Generic;
using AppAppartamenti.Views;
using Xamarin.Forms;

namespace AppAppartamenti.ContentViews
{
    public partial class GoogleButton : ContentView
    {
        public GoogleButton()
        {
            InitializeComponent();
        }

        private async void BtnGoogleAuth_Clicked(object sender, EventArgs e)
        {
            try
            {
                bool answer = await App.Current.MainPage.DisplayAlert("Promuovocasa.it vuole utilizzare Google.com per accedere", "Questo permette all'app e al sito web di accedere alle tue informazioni.", "Si", "No");
                if (answer)
                {
                    await Navigation.PushAsync(new NavigationPage(new Views.Account.GoogleLogin()));
                }
            }
            catch (Exception ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }
    }
}
