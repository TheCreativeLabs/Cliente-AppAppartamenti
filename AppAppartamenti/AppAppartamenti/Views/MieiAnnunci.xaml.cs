using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppAppartamenti.Utility;
using AppAppartamenti.ViewModels;
using AppAppartamentiApiClient;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAppartamenti.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MieiAnnunci : ContentPage
    {
        AnnunciViewModel viewModel;

        public MieiAnnunci()
        {
            InitializeComponent();

            BindingContext = viewModel = new AnnunciViewModel(TipiRicerca.MieiAnnunci, null);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as AnnunciDtoOutput;

            if (item == null || item.Id == null)
                return;

            //EventoClient eventoClient = new EventoClient(ApiHelper.GetApiClient());
            //EventoDtoOutput dettaglioEvento = await eventoClient.GetEventoByIdAsync(new Guid(item.Id));

            //await Navigation.PushAsync(new EventoModifica(new EventoDetailViewModel(dettaglioEvento)));
            if (item.Id != null && item.Id != Guid.Empty)
            {
                // Manually deselect item.
                AnnunciiListView.SelectedItem = null;

                await Navigation.PushAsync(new DettaglioAnnuncio(item.Id.Value,true));
            }
        }

        async void BtnDeleteAd_Clicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            var item = btn.CommandParameter as AnnunciDtoOutput;

            AnnunciClient annunciClient = new AnnunciClient(Api.ApiHelper.GetApiClient());
            bool answer = await DisplayAlert("Cancellare l'annuncio", "Procedendo, l'annuncio verrà elimninato definitavamente.", "Si", "No");

            if (answer)
            {
                //await annunciClient.DeleteAnnuncio(item.Id.Value);
                viewModel.Items.Remove(item);
            }
        }

        async void BtnAdd_Clicked(object sender, EventArgs e)
        {
             await Navigation.PushModalAsync(new NavigationPage( new SelezioneProprieta(null))); //è un nuovo annuncio, non devo passare l'annuncio da modificare
        }
    }
}