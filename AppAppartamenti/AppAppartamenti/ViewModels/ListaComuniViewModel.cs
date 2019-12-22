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
    public class ListaComuniViewModel : BaseViewModel
    {
        public ObservableCollection<ComuneDto> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public bool IsLoading { get; set; }

        public ListaComuniViewModel(string NomeComune)
        {
            Items = new ObservableCollection<ComuneDto>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand(NomeComune));
        }

        async Task ExecuteLoadItemsCommand(string NomeComune)
        {
            if (IsBusy)
                return;

            IsBusy = true;
            IsLoading = true;
            try
            {
                Items.Clear();

                //AnnunciClient annunciClient = new AnnunciClient(Api.ApiHelper.GetApiClient());
                ICollection<ComuneDto> listaComuni = await ApiHelper.GetListaComuni(NomeComune);// annunciClient.GetListaComuniAsync(NomeComune);

                foreach (var evento in listaComuni)
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
                IsLoading = false;
            }
        }
    }
}