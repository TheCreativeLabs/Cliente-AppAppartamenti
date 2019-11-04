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
        public ObservableCollection<TipologiaProprieta> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        private bool SoloPersonali { get; set; }

        public TipologiaProprietaViewModel()
        {
            Items = new ObservableCollection<TipologiaProprieta>();
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

                ICollection<TipologiaProprieta> tipologiaProprietas;


                tipologiaProprietas = await annunciClient.GetListaTipologiaProprietaAsync();
              

                foreach (var evento in tipologiaProprietas)
                {
                    Items.Add(evento);
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