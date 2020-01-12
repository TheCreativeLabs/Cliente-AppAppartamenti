using AppAppartamenti.ViewModels;
using AppAppartamenti.Views.Messaggi;
using AppAppartamentiApiClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using NavigationPage = Xamarin.Forms.NavigationPage;

namespace AppAppartamenti.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DettaglioAnnuncio : ContentPage
    {

        public  class BindingModel
        {
            public AnnuncioDetailViewModel viewModel { get; set; }
            public AnnuncioimmaginiViewModel viewModelImmagini { get; set; }
        }

        BindingModel bindingModel;
        Guid IdAnnuncio;
        Guid IdUtente;
        bool IsEditable;

        public DettaglioAnnuncio(AnnunciDtoOutput annuncio,bool IsEditableParam)
        {
            InitializeComponent();
            IsEditable = IsEditableParam;
            IdAnnuncio = annuncio.Id.Value;
            IdUtente = annuncio.IdUtente.Value;

            btnModifica.IsVisible = IsEditable;
            btnModificaNavBar.IsVisible = IsEditable;
            btnMessageNav.IsVisible = !IsEditable;
            btnMessage.IsVisible = !IsEditable;
            BtnSegnalaNav.IsVisible = !IsEditable;
            stkPulsanti.IsVisible = !IsEditable;

            bindingModel = new BindingModel();
            bindingModel.viewModelImmagini = new AnnuncioimmaginiViewModel();
            bindingModel.viewModel = new AnnuncioDetailViewModel();
            bindingModel.viewModelImmagini.Items.Add(annuncio.ImmaginePrincipale);

        }

        private async void LoadUserInfo()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                AccountClient accountClient = new AccountClient(await Api.ApiHelper.GetApiClient());
                var User = await accountClient.GetUserInfoAsync(IdUtente);

                lblUserName.Text = User.Nome;
                lblUserSurname.Text = User.Cognome;
                if (User.FotoProfilo != null)
                {
                    imgUserImage.Source = ImageSource.FromStream(() => new MemoryStream(User.FotoProfilo));
                }

                stkUserInfo.IsVisible = true;
            });
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            bindingModel.viewModelImmagini.IdAnnuncio = IdAnnuncio;
            bindingModel.viewModelImmagini.LoadItemsCommand.Execute(null);
            bindingModel.viewModel = await AnnuncioDetailViewModel.ExecuteLoadItemsCommandAsync(IdAnnuncio);

            BindingContext = bindingModel;
            scrollView.IsVisible = true;

            Task.Run(async () => { LoadUserInfo(); });

            if (!IsEditable)
            {
                btnAddPreferito.IsVisible = !bindingModel.viewModel.Item.FlagPreferito.Value;
                btnRemovePreferito.IsVisible = bindingModel.viewModel.Item.FlagPreferito.Value;
                btnAddPreferitoNav.IsVisible = !bindingModel.viewModel.Item.FlagPreferito.Value;
                btnRemovePreferitoNav.IsVisible = bindingModel.viewModel.Item.FlagPreferito.Value;
            }

            if (!string.IsNullOrEmpty(bindingModel.viewModel.Item.CoordinateGeografiche))
            {
                var pos = bindingModel.viewModel.Item.CoordinateGeografiche.Split(';');
                var lat = pos[0];
                var lon = pos[1];

                Pin pin = new Pin
                {
                    Label = bindingModel.viewModel.Item.Indirizzo,
                    Address = $"{bindingModel.viewModel.Item.Indirizzo},{bindingModel.viewModel.Item.NomeComune}",
                    Type = PinType.Generic,
                    Position = new Position(Double.Parse(lat), Double.Parse(lon))
                };

                map.Pins.Add(pin);
                map.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Position, Distance.FromMiles(0.1)));
            }
            else
            {
                map.IsVisible = false;
            }

            MessagingCenter.Subscribe<AnnuncioimmaginiViewModel, int>(this, "ImmaginiCaricate", async (sender, arg) =>
            {
                CarouselImagesProgress.Text = "1/" + arg;
            });

            StackLoader.IsVisible = false;
            StackPage.IsVisible = true;
        }

        async void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
        {
            //if (e.ScrollY > 20)
            //{
            //    stkHeader.IsVisible = false;
            //    NavigationPage.SetHasNavigationBar(this, true);
            //}
            //else
            //{
            //    NavigationPage.SetHasNavigationBar(this, false);
            //    stkHeader.IsVisible = true;
            //}
        }

        private async void BtnBack_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PopAsync();
            }
            catch (Exception Ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }

        private async void BtnOpenMaps_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    // https://developer.apple.com/library/ios/featuredarticles/iPhoneURLScheme_Reference/MapLinks/MapLinks.html
                    await Launcher.OpenAsync($"http://maps.apple.com/?q={bindingModel.viewModel.Item.Indirizzo.Replace(" ","+")}+{bindingModel.viewModel.Item.NomeComune.Replace(" ","+")}");
                }
                else if (Device.RuntimePlatform == Device.Android)
                {
                    // open the maps app directly
                    await Launcher.OpenAsync($"geo:0,0?q={bindingModel.viewModel.Item.Indirizzo.Replace(" ","+")}+{bindingModel.viewModel.Item.NomeComune.Replace(" ","+")}");
                }
            }
            catch (Exception Ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }

        private async void BtnOpenImages_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushModalAsync(new DettaglioAnnuncioImages(bindingModel.viewModelImmagini));
            }
            catch (Exception Ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }

        private async void BtnSegnala_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new SegnalaAnnuncio(IdAnnuncio));
            }
            catch (Exception Ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }

        private async void BtnNewMessage_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new NuovoMessaggio(IdAnnuncio, bindingModel.viewModel.Item.IdUtente.Value));
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
                btnRemovePreferito.IsVisible = true;
                btnAddPreferito.IsVisible = false;
                btnRemovePreferitoNav.IsVisible = true;
                btnAddPreferitoNav.IsVisible = false;
                AnnunciClient annunciClient = new AnnunciClient(await Api.ApiHelper.GetApiClient());
                await annunciClient.AggiungiPreferitoAsync(bindingModel.viewModel.Item.Id.Value);
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
                btnRemovePreferito.IsVisible = false;
                btnAddPreferito.IsVisible = true;
                btnAddPreferitoNav.IsVisible = false;
                btnRemovePreferitoNav.IsVisible = true;
                AnnunciClient annunciClient = new AnnunciClient(await Api.ApiHelper.GetApiClient());
                await annunciClient.RimuoviPreferitoAsync(bindingModel.viewModel.Item.Id.Value);
            }
            catch (Exception Ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }

        private async void BtnAppuntamento_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new NuovoAppuntamento(IdAnnuncio));
            }
            catch (Exception Ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }

        private async void BtnAssistente_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushModalAsync(new AssistenteVirtualeAcquisto());
            }
            catch (Exception Ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }

        private async void BtnModifica_Clicked(object sender, EventArgs e)
        {
            try
            {
                bindingModel.viewModel.Item.ImmaginiAnnuncio = bindingModel.viewModelImmagini.Items;
                await Navigation.PushModalAsync(new NavigationPage(new SelezioneProprieta(bindingModel.viewModel)));
            }
            catch (Exception Ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }

        private async void BtnShare_Clicked(object sender, EventArgs e)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Uri = $"{AppSetting.SiteApp}/Annunci/Detail/{bindingModel.viewModel.Item.Id}",
                Title = "Condividi il link"
            });
        }

        void OnPositionChanged(object sender, PositionChangedEventArgs e)
        {
            int previousPosition = e.PreviousPosition;
            int currentPosition = e.CurrentPosition;
            CarouselImagesProgress.Text = (currentPosition+1) + "/" + bindingModel.viewModelImmagini.Items.Count;
        }
    }
}