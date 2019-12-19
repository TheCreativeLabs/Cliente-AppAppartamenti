using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using AppAppartamenti.Models;
using AppAppartamenti.Views;
using AppAppartamentiApiClient;
using System.Collections.Generic;
using AppAppartamenti.Api;

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

                ICollection<TipologiaProprieta> tipologiaProprietas = await ApiHelper.GetListaTipologiaProprieta();

                foreach (var tipo in tipologiaProprietas)
                {
                    string translation = Helpers.TranslateExtension.ResMgr.Value.GetString(tipo.Codice, translate.ci);
                    translationsMap.Add(translation, tipo); //aggiungo alla mappa il codice associato alla traduzione
                    Items.Add(translation); //a fe si mostra la traduzione
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