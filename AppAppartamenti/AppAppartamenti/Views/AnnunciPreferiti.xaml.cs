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

            //await Navigation.PushAsync(new EventoModifica(new EventoDetailViewModel(dettaglioEvento)));
            if (item.Id != null && item.Id != Guid.Empty)
            {
                // Manually deselect item.
                AnnunciiListView.SelectedItem = null;

                await Navigation.PushAsync(new DettaglioAnnuncio(item.Id.Value, false));
            }
        }

        private async void BtnAddPreferito_Clicked(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                var item = Guid.Parse(btn.CommandParameter.ToString());

                AnnunciClient annunciClient = new AnnunciClient(Api.ApiHelper.GetApiClient());
                await annunciClient.AggiungiPreferitoAsync(item);
            }
            catch (Exception Ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }

        private async void BtnRemovePreferito_Clicked(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                var item = btn.CommandParameter as AnnunciDtoOutput;

                viewModel.Items.Remove(item);

                AnnunciClient annunciClient = new AnnunciClient(Api.ApiHelper.GetApiClient());
                await annunciClient.RimuoviPreferitoAsync(item.Id.Value);
            }
            catch (Exception Ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }
    }
}