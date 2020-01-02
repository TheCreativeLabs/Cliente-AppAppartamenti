using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppAppartamenti.ViewModels;
using AppAppartamentiApiClient;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAppartamenti.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaAnnunci : ContentPage
    {
        AnnunciViewModel viewModel;
        RicercaModel FiltriRicerca;

        public ListaAnnunci(RicercaModel FiltriRicercaParam)
        {
            InitializeComponent();

            FiltriRicerca = FiltriRicercaParam;

            BindingContext = viewModel = new AnnunciViewModel();
            viewModel.FiltriRicerca = FiltriRicerca;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<Ricerca, string>(this, "Ricarica", async (sender, arg) =>
            {
                if (!string.IsNullOrEmpty(arg))
                {
                    FiltriRicerca = JsonConvert.DeserializeObject<RicercaModel>(arg);
                }

                viewModel.FiltriRicerca = FiltriRicerca;
                //viewModel.Items = new System.Collections.ObjectModel.ObservableCollection<AnnunciDtoOutput>();
            });


            viewModel.LoadItemsCommand.Execute(null);

            entRicerca.Text = FiltriRicerca.Comune.NomeComune;
        }


        async void OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            await Navigation.PushModalAsync(new Ricerca(FiltriRicerca));
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as AnnunciDtoOutput;

            if (item == null || item.Id == null)
                return;

            if (item.Id != null && item.Id != Guid.Empty)
            {
                // Manually deselect item.
                AnnunciiListView.SelectedItem = null;

                await Navigation.PushAsync(new DettaglioAnnuncio(item,false));
            }
        }

        async void BtnAdd_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage( new SelezioneProprieta(null)));
        }

        private async void BtnBack_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new MainPage());
            }
            catch (Exception Ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }

        private async void BtnAddPreferito_Clicked(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                var item = btn.CommandParameter as AnnunciDtoOutput;

                item.FlagPreferito = true;

                var annuncio =  viewModel.Items.Where(x => x.Id.Value == item.Id.Value).First();
                annuncio.FlagPreferito = true;

                OnPropertyChanged("Items");

                AnnunciClient annunciClient = new AnnunciClient(await Api.ApiHelper.GetApiClient());
                await annunciClient.AggiungiPreferitoAsync(item.Id.Value);

                MessagingCenter.Send(this, "RefreshPreferiti", "OK");

            }
            catch (Exception Ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }

        private async void BtnShare_Clicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            var item = btn.CommandParameter as AnnunciDtoOutput;

            await Share.RequestAsync(new ShareTextRequest
            {
                Uri = $"{AppSetting.SiteApp}/Annunci/Detail/{item.Id}",
                Title = "Condividi il link"
            });
        }

        private async void BtnAddRemovePreferito_Clicked(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                var item = btn.CommandParameter as AnnunciDtoOutput;
                var isPreferito = item.FlagPreferito.Value;
                if (item.FlagPreferito.Value)
                {
                    btn.TextColor = Color.White;
                }
                else
                {
                    btn.TextColor = Color.Coral;
                }

                item.FlagPreferito = !isPreferito;
                btn.CommandParameter = item;

                if (isPreferito)
                {
                    AnnunciClient annunciClient = new AnnunciClient(await Api.ApiHelper.GetApiClient());
                    await annunciClient.RimuoviPreferitoAsync(item.Id.Value);
                }
                else
                {
                    AnnunciClient annunciClient = new AnnunciClient(await Api.ApiHelper.GetApiClient());
                    await annunciClient.AggiungiPreferitoAsync(item.Id.Value);
                }

                MessagingCenter.Send(this, "RefreshPreferiti", "OK");
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

                item.FlagPreferito = false;

                OnPropertyChanged("Items");

                var annuncio = viewModel.Items.Where(x => x.Id.Value == item.Id.Value).First();
                annuncio.FlagPreferito = false;

                AnnunciClient annunciClient = new AnnunciClient(await Api.ApiHelper.GetApiClient());
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