using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace AppAppartamenti.Views
{
    public partial class AssistenteVirtualeAcquisto : ContentPage
    {
        public AssistenteVirtualeAcquisto()
        {
            InitializeComponent();
        }

        private async void BtnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
