using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using AppAppartamenti.Models;
using AppAppartamenti.Views;
using AppAppartamentiApiClient;
using System.Collections.Generic;

namespace AppAppartamenti.ViewModels
{
    public class TipologiaAnnunciViewModel : BaseViewModel
    {
        public ObservableCollection<string> Items { get; set; }
        //public List<TipologiaAnnuncio> listaAnnunci { get; set; }
        public Command LoadItemsCommand { get; set; }

        static Helpers.TranslateExtension translate = new Helpers.TranslateExtension();
        public Dictionary<string, TipologiaAnnuncio> translationsMap;

        public TipologiaAnnunciViewModel()
        {
            Items = new ObservableCollection<string>();
            //listaAnnunci = new List<TipologiaAnnuncio>();
            translationsMap = new Dictionary<string, TipologiaAnnuncio>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        public async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();

                AnnunciClient annunciClient = new AnnunciClient(Api.ApiHelper.GetApiClient());
                ICollection<TipologiaAnnuncio> tipologiaAnnuncios = await annunciClient.GetListaTipologiaAnnunciAsync();
                //listaAnnunci = (List<TipologiaAnnuncio>)tipologiaAnnuncios;

                foreach (var tipo in tipologiaAnnuncios)
                {
                    string translation = Helpers.TranslateExtension.ResMgr.Value.GetString(tipo.Codice, translate.ci);
                    translationsMap.Add(translation, tipo); //aggiungo alla mappa il codice associato alla traduzione
                    Items.Add(translation); //a fe si mostra la traduzione
                    //Items.Add(evento.Descrizione);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}