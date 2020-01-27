using AppAppartamenti.ViewModels;
using AppAppartamentiApiClient;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using System.Linq;
using AppAppartamenti.Files;
using System.Threading.Tasks;

namespace AppAppartamenti.Views
{
    public partial class AssistenteVirtualeAcquisto : ContentPage
    {

        DocumentiViewModel viewModel;

        public AssistenteVirtualeAcquisto(bool isVendita)
        {
            InitializeComponent();

            BindingContext = viewModel = new DocumentiViewModel(isVendita);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            AccountClient accountClient = new AccountClient(await Api.ApiHelper.GetApiClient());
            byte[] ImmagineAssistente = await accountClient.GetAvatarCurrentUserAsync();
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
                byte[] bytes = documento.Documento;
                string fileName = documento.Titolo.Replace(" ", "") + ".pdf";

                //var externalPath = global::Android.OS.Environment.ExternalStorageDirectory.Path + "/" + fileName;
                //File.WriteAllBytes(externalPath, bytes);
                //Java.IO.File file = new Java.IO.File(externalPath);
                //Android.Net.Uri uri = Android.Net.Uri.FromFile(file);
                //Device.OpenUri(new Uri("https://www.canada.ca/content/dam/ircc/migration/ircc/english/pdf/kits/forms/imm5406e.pdf"));


                string filepath = await DependencyService.Get<ISaveFile>().SaveFiles(fileName, bytes);
                return filepath;

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
