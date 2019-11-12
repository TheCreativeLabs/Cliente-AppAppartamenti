using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppAppartamenti.ViewModels;
using AppAppartamentiApiClient;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAppartamenti.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaAnnunci : ContentPage
    {
        AnnunciViewModel viewModel;

        public Command PreferitiCommand { get; set; }


        public ListaAnnunci()
        {
            InitializeComponent();

            BindingContext = viewModel = new AnnunciViewModel(false);
            PreferitiCommand = new Command(() => lblComandiRapidi_Tapped(null, null));


            // Your label tap event
            //var lblPreferiti_tap = new TapGestureRecognizer();
            //lblPreferiti_tap.Tapped += (s, e) =>
            //{
            //    //
            //    //  Do your work here.
            //    //
            //};
            //lblPreferiti.GestureRecognizers.Add(lblPreferiti_tap);

        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        private async void lblComandiRapidi_Tapped(object sender, EventArgs e)
        {
            try
            {

                string action = await DisplayActionSheet("aaa",
                                                         "bbb", null,
                                                         "cc",
                                                         "dd");

            }
            catch (Exception)
            {
                throw;
            }
        }



        private async void entRicerca_Focused(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as AnnunciDtoOutput;

            if (item == null || item.Id == null)
                return;

            //EventoClient eventoClient = new EventoClient(ApiHelper.GetApiClient());
            //EventoDtoOutput dettaglioEvento = await eventoClient.GetEventoByIdAsync(new Guid(item.Id));


            //await Navigation.PushAsync(new EventoModifica(new EventoDetailViewModel(dettaglioEvento)));

            // Manually deselect item.
            AnnunciiListView.SelectedItem = null;
        }


        async void BtnAdd_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage( new SelezioneProprieta()));
        }
    }
}