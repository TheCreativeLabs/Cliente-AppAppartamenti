using AppAppartamenti.ViewModels;
using AppAppartamentiApiClient;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using System.Linq;
using AppAppartamenti.Files;
using System.Threading.Tasks;
using Xamarin.Essentials;
using AppAppartamenti.Api;

namespace AppAppartamenti.Views
{
    public partial class AssistenteVirtuale : ContentPage
    {

        DocumentiViewModel viewModel;

        public AssistenteVirtuale(bool isVendita)
        {
            InitializeComponent();

            frameAcquisto.IsVisible = !isVendita;
            frameVendita.IsVisible = isVendita;

            BindingContext = viewModel = new DocumentiViewModel(isVendita);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            byte[] ImmagineAssistente = await ApiHelper.GetVirtualAssistentImage(); //await accountClient.GetAvatarCurrentUserAsync();
            imgAssistenteVirtuale.Source = ImageSource.FromStream(() => new MemoryStream(ImmagineAssistente));

            //viewModel.LoadItemsCommand.Execute(null);
            await viewModel.ExecuteLoadItemsCommand();

            cvDocumenti.ItemsSource = viewModel.Documents;

            //cvLink.ItemsSource = viewModel.Links;
        }

        public async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (!e.CurrentSelection.Any())
                return;

            var documento = e.CurrentSelection.First() as DocumentoDto;

            cvDocumenti.SelectedItem = null;

            //TappedEventArgs args = (TappedEventArgs)e;
            //DocumentoDto documento = (DocumentoDto)args.Parameter;
            if (documento == null)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
            await OpenOrDownloadDocument(documento);
        }

            public async Task<string> OpenOrDownloadDocument(DocumentoDto documento)
        {

            if (documento.Link != null) //caso link
            {
                string link = documento.Link;
                Uri uri = new Uri(link);
                bool x = await Xamarin.Essentials.Launcher.TryOpenAsync(link);
                return "";
                //Device.OpenUri(uri);
            }
            else if(documento.Documento != null) //caso documento pdf
            {
                //byte[] bytes = documento.Documento;
                //string fileName = documento.Titolo.Replace(" ", "") + ".pdf";
                //string filepath = await DependencyService.Get<ISaveFile>().SaveFiles(fileName, bytes);
                //return filepath;
                string url = AppSetting.DocumentoUrl + "" + documento.Id;
                //await Browser.OpenAsync(url, new BrowserLaunchOptions
                //{
                //    LaunchMode = BrowserLaunchMode.SystemPreferred,
                //    TitleMode = BrowserTitleMode.Show
                //});
                bool x = await Xamarin.Essentials.Launcher.TryOpenAsync(url);


            }
            return "";
        }

        

        public async void OnInfoTapped(object sender, EventArgs e)
        {
            TappedEventArgs args = (TappedEventArgs)e;
            string descrizione = (string) args.Parameter;
            await DisplayAlert("Info", descrizione, "Ok");
        }

        private async void BtnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
