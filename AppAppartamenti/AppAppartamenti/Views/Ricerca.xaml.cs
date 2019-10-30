using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAppartamenti.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Ricerca : ContentPage
    {
        public Ricerca()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }

        private async void btnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void entRicerca_Focused(object sender, FocusEventArgs e)
        {
            if (entRicerca.Text == "Ricerca")
            {
                entRicerca.Text = "";
            }
        }

        private void entRicerca_Unfocused(object sender, FocusEventArgs e)
        {
            if (entRicerca.Text == "")
            {
                entRicerca.Text = "Ricerca";
            }
        }
    }
}