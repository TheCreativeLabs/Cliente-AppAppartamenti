using System;
using System.Collections.Generic;
using AppAppartamenti.Views.Messaggi;
using Xamarin.Forms;

namespace AppAppartamenti.Views
{
    public partial class DettaglioAppuntamento : ContentPage
    {
        public DettaglioAppuntamento()
        {
            InitializeComponent();
        }

        private async void BtnMessaggio_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NuovoMessaggio());
        }
    }
}
