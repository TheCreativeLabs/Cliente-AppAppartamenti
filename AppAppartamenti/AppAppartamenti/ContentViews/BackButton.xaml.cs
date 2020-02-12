using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace AppAppartamenti.ContentViews
{
    partial class BackButton : ContentView
    {
        public BackButton()
        {
            InitializeComponent();
        }

        private async void BtnBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
