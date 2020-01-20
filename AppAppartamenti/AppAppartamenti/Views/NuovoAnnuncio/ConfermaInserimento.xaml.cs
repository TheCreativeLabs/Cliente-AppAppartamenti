using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace AppAppartamenti.Views.NuovoAnnuncio
{
    public partial class ConfermaInserimento : ContentPage
    {
        public ConfermaInserimento()
        {
            InitializeComponent();
        }

        private async void BtnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void BtnProcedi_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AnnuncioCreato", "Ok");
            await Navigation.PopModalAsync();
        }
    }
}
