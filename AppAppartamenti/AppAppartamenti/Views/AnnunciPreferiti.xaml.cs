using AppAppartamenti.ViewModels;
using AppAppartamentiApiClient;
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
    public partial class AnnunciPreferiti : ContentPage
    {
        AnnunciPreferitiViewModel viewModel;

        public AnnunciPreferiti()
        {
            InitializeComponent();

            BindingContext = viewModel = new AnnunciPreferitiViewModel();
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
                AnnunciClient annunciClient = new AnnunciClient(Api.ApiHelper.GetApiClient());
                AnnuncioDtoOutput annuncioDetail = await annunciClient.GetAnnuncioByIdAsync((Guid)item.Id);

                // Manually deselect item.
                AnnunciiListView.SelectedItem = null;

                await Navigation.PushAsync(new DettaglioAnnuncio(annuncioDetail));
            }

        }
    }
}