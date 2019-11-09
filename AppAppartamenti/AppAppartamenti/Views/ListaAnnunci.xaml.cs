using System;
using System.Collections.Generic;
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

        public ListaAnnunci()
        {
            InitializeComponent();

            BindingContext = viewModel = new AnnunciViewModel(false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
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