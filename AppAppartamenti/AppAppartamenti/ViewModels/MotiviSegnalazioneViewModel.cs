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
    public class MotiviSegnalazioneViewModel : BaseViewModel
    {
        public ObservableCollection<string> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Dictionary<string, MotivoSegnalazione> translationsMap;

        public MotiviSegnalazioneViewModel()
        {
            Items = new ObservableCollection<string>();
            translationsMap = new Dictionary<string, MotivoSegnalazione>();
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

                AppAppartamentiApiClient.AnnunciClient adminClient = new AppAppartamentiApiClient.AnnunciClient(await ApiHelper.GetApiClient());
                var listaMotivi = await adminClient.GetListaMotiviSegnalazioneAsync();
                foreach (var tipo in listaMotivi)
                {
                    Items.Add(tipo.Descrizione);
                    translationsMap.Add(tipo.Descrizione, tipo);
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