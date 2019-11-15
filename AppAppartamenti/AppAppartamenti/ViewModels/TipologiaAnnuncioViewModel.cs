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
        public List<TipologiaAnnuncio> listaAnnunci { get; set; }
        public Command LoadItemsCommand { get; set; }

        public TipologiaAnnunciViewModel()
        {
            Items = new ObservableCollection<string>();
            listaAnnunci = new List<TipologiaAnnuncio>();
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
                ICollection<TipologiaAnnuncio> tipologiaAnnuncios = await annunciClient.GetListaTipologiaAnnunciAsync();
                listaAnnunci = (List<TipologiaAnnuncio>)tipologiaAnnuncios;

                foreach (var evento in tipologiaAnnuncios)
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