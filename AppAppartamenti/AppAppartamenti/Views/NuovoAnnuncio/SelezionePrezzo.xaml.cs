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
                listClasseEnergetica = (await annunciClient.GetListaClasseEnergeticaAsync()).ToList();
            }

            if (listTipologiaRiscaldamento == null || listTipologiaRiscaldamento.Count == 0)
            {
                listTipologiaRiscaldamento = (await annunciClient.GetListaTipologiaRiscaldamentoAsync()).ToList();
            }

            pckClasseEnergetica.ItemsSource = listClasseEnergetica;
            pckRiscaldamento.ItemsSource = listTipologiaRiscaldamento;

            if (pckClasseEnergetica.SelectedItem ==null && dtoToModify != null && dtoToModify.Item.ClasseEnergetica != null)
            {
                //FIXME LE SEGUENTI DUE RIGHE FUNZIONANO SOLO X LINGUA ITALIANA, CAMBIARE GESTIONE E LAVORARE CON IL CODICE INVECE CHE CON LA DESCRIZIONE 
                pckClasseEnergetica.SelectedItem = listClasseEnergetica.Where(x => x.Descrizione == dtoToModify.Item.ClasseEnergetica).FirstOrDefault();
            }
            if (pckRiscaldamento.SelectedItem  == null && dtoToModify != null && dtoToModify.Item.TipologiaRiscaldamento!=null)
            {
                pckRiscaldamento.SelectedItem = listTipologiaRiscaldamento.Where(x => x.Descrizione == dtoToModify.Item.TipologiaRiscaldamento).FirstOrDefault();
            }

            //try
            //{
            //    ((NavigationPage)this.Parent).BarBackgroundColor = Color.White;
            //    ((NavigationPage)this.Parent).BarTextColor = Color.Black;
            //    NavigationPage.SetHasNavigationBar(this, true);
            //}
            //catch (Exception ex)
            //{

            //}

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

        private async void BtnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void BtnPrezzoProcedi_Clicked(object sender, EventArgs e)
        {
            annuncio.Prezzo = double.Parse(entPrezzo.Text);
            annuncio.SpesaMensileCondominio = double.Parse(entSpeseCondominiali.Text);
            annuncio.IdClasseEnergetica = (Guid) ((ClasseEnergetica)pckClasseEnergetica.SelectedItem).Id;
            annuncio.IdTipologiaRiscaldamento = (Guid) ((TipologiaRiscaldamento)pckRiscaldamento.SelectedItem).Id;

            await Navigation.PushAsync(new SelezioneImmagini(annuncio, dtoToModify));
        }

        private void EntClasseEnergetica_Focused(object sender, EventArgs e)
        {
            pckClasseEnergetica.Focus();
        }

        private void EntTipoRiscaldamento_Focused(object sender, EventArgs e)
        {
            pckRiscaldamento.Focus();
        }

        public void PckRiscaldamento_SelectedIndexChanged(object sender, EventArgs e)
        {
            //entRiscaldamento.Text = ((TipologiaRiscaldamento)pckRiscaldamento.SelectedItem).Descrizione;
            entRiscaldamento.Text = Helpers.TranslateExtension.ResMgr.Value.GetString(((TipologiaRiscaldamento)pckRiscaldamento.SelectedItem).Codice, translate.ci);
        }

        public void PckClasseEnergetica_SelectedIndexChanged(object sender, EventArgs e)
        {
            //entClasseEnergetica.Text = ((ClasseEnergetica)pckClasseEnergetica.SelectedItem).Descrizione;
            entClasseEnergetica.Text = Helpers.TranslateExtension.ResMgr.Value.GetString(((ClasseEnergetica)pckClasseEnergetica.SelectedItem).Codice, translate.ci);

        }
    }
}