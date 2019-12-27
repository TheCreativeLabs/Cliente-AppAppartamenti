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
        public string NomeComune { get; set; }


        public ListaComuniViewModel()
        {
            Items = new ObservableCollection<ComuneDto>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {
            //if (IsBusy)
            //    return;

            IsBusy = true;
            IsLoading = true;

            try
            {
                Items.Clear();

                if(NomeComune.Length > 3) { 
                //AnnunciClient annunciClient = new AnnunciClient(Api.ApiHelper.GetApiClient());
                ICollection<ComuneDto> listaComuni; // = await ApiHelper.GetListaComuni(NomeComune);// annunciClient.GetListaComuniAsync(NomeComune);
                AnnunciClient annunciClient = new AnnunciClient(await Api.ApiHelper.GetApiClient());
                listaComuni = await annunciClient.GetListaComuniAsync(NomeComune);
                    Items.Clear();
                    foreach (var evento in listaComuni)
                    {
                        Items.Add(evento);
                    }
                }
                else
                {
                    Items.Clear();
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