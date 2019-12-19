using System;
using System.Collections.Generic;
using AppAppartamenti.Views;
using Xamarin.Forms;

namespace AppAppartamenti.ContentViews
{
    public partial class FacebookButton : ContentView
    {
        public FacebookButton()
        {
            InitializeComponent();
        }

        private async void btnAccediFacebook_Clicked(object sender, EventArgs e)
        {
            try
            {
                bool answer = await App.Current.MainPage.DisplayAlert("Liberacasa.it vuole utilizzare Facebook.com per accedere", "Questo permette all'app e al sito web di accedere alle tue informazioni.", "Si", "No");
                if (answer)
                {
                    await Navigation.PushAsync(new NavigationPage(new AppAppartamenti.Views.Account.FacebookLogin()));
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
