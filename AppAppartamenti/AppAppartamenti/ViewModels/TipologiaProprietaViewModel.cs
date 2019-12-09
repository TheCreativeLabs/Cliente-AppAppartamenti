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
    public class TipologiaProprietaViewModel : BaseViewModel
    {
        public ObservableCollection<string> Items { get; set; }
        //public List<TipologiaProprieta> listaProprieta { get; set; }

        public Command LoadItemsCommand { get; set; }

        static Helpers.TranslateExtension translate = new Helpers.TranslateExtension();
        public Dictionary<string, TipologiaProprieta> translationsMap;

        public TipologiaProprietaViewModel()
        {
            Items = new ObservableCollection<string>();
            //listaProprieta = new List<TipologiaProprieta>();
            translationsMap = new Dictionary<string, TipologiaProprieta>();
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

                ICollection<TipologiaProprieta> tipologiaProprietas = await annunciClient.GetListaTipologiaProprietaAsync();
                //listaProprieta = (List<TipologiaProprieta>)tipologiaProprietas;
                foreach (var tipo in tipologiaProprietas)
                {
                    string translation = Helpers.TranslateExtension.ResMgr.Value.GetString(tipo.Codice, translate.ci);
                    translationsMap.Add(translation, tipo); //aggiungo alla mappa il codice associato alla traduzione
                    Items.Add(translation); //a fe si mostra la traduzione
                    //Items.Add(tipo.Codice);

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