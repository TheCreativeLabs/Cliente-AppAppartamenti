using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace AppAppartamenti.Views.GeneralCondition
{
    public partial class GeneralCondition : ContentPage
    {
        public GeneralCondition()
        {
            InitializeComponent();
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
