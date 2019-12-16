using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppAppartamenti.ViewModels;
using Newtonsoft.Json;
using Plugin.Media;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAppartamenti.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : ContentPage
    {
        public Home()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var isBusy = false;

            MessagingCenter.Subscribe<Ricerca, string>(this, "Ricerca", async (sender, arg) =>
            {
                if (!isBusy)
                {
                    isBusy = true;

                    await Navigation.PushAsync(new ListaAnnunci(JsonConvert.DeserializeObject<RicercaModel>(arg)));

                    isBusy = false;
                }
            });
        }

        private async void entRicerca_Focused(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Ricerca());
        }
    }
}