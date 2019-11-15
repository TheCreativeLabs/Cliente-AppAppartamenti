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
        public List<TipologiaProprieta> listaProprieta { get; set; }

        public Command LoadItemsCommand { get; set; }

        public TipologiaProprietaViewModel()
        {
            Items = new ObservableCollection<string>();
            listaProprieta = new List<TipologiaProprieta>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();

                AnnunciClient annunciClient = new AnnunciClient(Api.ApiHelper.GetApiClient());

                ICollection<TipologiaProprieta> tipologiaProprietas = await annunciClient.GetListaTipologiaProprietaAsync();
                listaProprieta = (List<TipologiaProprieta>)tipologiaProprietas;
                foreach (var evento in tipologiaProprietas)
                {
                    Items.Add(evento.Descrizione);
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