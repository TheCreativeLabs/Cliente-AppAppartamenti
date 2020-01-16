using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppAppartamenti.ViewModels;
using AppAppartamentiApiClient;
using DependencyServiceDemos;
using RestSharp.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace AppAppartamenti.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelezionePrezzo : ContentPage
    {
        AnnuncioDtoInput annuncio = new AnnuncioDtoInput();
        AnnuncioDetailViewModel dtoToModify;
        List<ClasseEnergetica> listClasseEnergetica = new List<ClasseEnergetica>();
        List<TipologiaRiscaldamento> listTipologiaRiscaldamento = new List<TipologiaRiscaldamento>();

        static Helpers.TranslateExtension translate = new Helpers.TranslateExtension();


        public SelezionePrezzo(AnnuncioDtoInput Annuncio, AnnuncioDetailViewModel dtoToModify)
        {
            this.dtoToModify = dtoToModify;
            InitializeComponent();

            if (this.dtoToModify != null)
            {
                if (entPrezzo.Text == null)
                {
                    entPrezzo.Text = dtoToModify.Item.Prezzo != null ? dtoToModify.Item.Prezzo.ToString() : null;
                }
                if (entSpeseCondominiali.Text == null)
                {
                    entSpeseCondominiali.Text = dtoToModify.Item.SpesaMensileCondominio != null ? dtoToModify.Item.SpesaMensileCondominio.ToString() : null;
                }
                
            }

            annuncio = Annuncio;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();


            AnnunciClient annunciClient = new AnnunciClient(await Api.ApiHelper.GetApiClient());

            if (listClasseEnergetica == null || listClasseEnergetica.Count ==0)
            {
                var list= (await annunciClient.GetListaClasseEnergeticaAsync()).ToList();
                list.Insert(0, new ClasseEnergetica() { Id = null, Codice = "EMPTY", Descrizione = "Seleziona..." });
                listClasseEnergetica = list;
            }

            if (listTipologiaRiscaldamento == null || listTipologiaRiscaldamento.Count == 0)
            {
                var list = (await annunciClient.GetListaTipologiaRiscaldamentoAsync()).ToList();
                list.Insert(0, new TipologiaRiscaldamento() { Id = null, Codice = "EMPTY", Descrizione = "Seleziona..." });
                listTipologiaRiscaldamento = list;
            }

            pckClasseEnergetica.ItemsSource = listClasseEnergetica;
            pckRiscaldamento.ItemsSource = listTipologiaRiscaldamento;

            if (pckClasseEnergetica.SelectedItem ==null && dtoToModify != null && dtoToModify.Item.ClasseEnergetica != null)
            {
                //FIXME LE SEGUENTI DUE RIGHE FUNZIONANO SOLO X LINGUA ITALIANA, CAMBIARE GESTIONE E LAVORARE CON IL CODICE INVECE CHE CON LA DESCRIZIONE 
                pckClasseEnergetica.SelectedItem = listClasseEnergetica.Where(x => x.Descrizione == dtoToModify.Item.ClasseEnergetica).FirstOrDefault();
            }
            else
            {
                pckClasseEnergetica.SelectedIndex = 0;

            }
            if (pckRiscaldamento.SelectedItem  == null && dtoToModify != null && dtoToModify.Item.TipologiaRiscaldamento!=null)
            {
                pckRiscaldamento.SelectedItem = listTipologiaRiscaldamento.Where(x => x.Descrizione == dtoToModify.Item.TipologiaRiscaldamento).FirstOrDefault();
            }
            else {
                pckRiscaldamento.SelectedIndex = 0;
            }
        }

        private async void BtnBack_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PopAsync();
            }
            catch(Exception)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }


        private void Entprezzo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty( entPrezzo.Text))
            {
                entPrezzo.Text = string.Format("{0:N0}",Decimal.Parse(entPrezzo.Text));
            }
        }

        private async void BtnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void BtnPrezzoProcedi_Clicked(object sender, EventArgs e)
        {

            if(string.IsNullOrEmpty(entPrezzo.Text) || string.IsNullOrEmpty(entSpeseCondominiali.Text) || ((ClasseEnergetica)pckClasseEnergetica.SelectedItem).Id == null
                || ((TipologiaRiscaldamento)pckRiscaldamento.SelectedItem).Id == null)
            {
                await DisplayAlert("Campo obbligatori", "Valorizzare tutti i campi.", "Ok");
                return;
            }

            annuncio.Prezzo = double.Parse(entPrezzo.Text);
            annuncio.SpesaMensileCondominio = double.Parse(entSpeseCondominiali.Text);
            annuncio.IdClasseEnergetica = (Guid) ((ClasseEnergetica)pckClasseEnergetica.SelectedItem).Id;
            annuncio.IdTipologiaRiscaldamento = (Guid) ((TipologiaRiscaldamento)pckRiscaldamento.SelectedItem).Id;

            await Navigation.PushAsync(new SelezioneImmagini(annuncio, dtoToModify));
        }
    }
}