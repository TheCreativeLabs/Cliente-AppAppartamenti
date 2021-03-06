﻿using System;
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
    public class AnnunciRecentiViewModel : BaseViewModel
    {
        public ObservableCollection<AnnunciDtoOutput> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public AnnunciRecentiViewModel()
        {
            Items = new ObservableCollection<AnnunciDtoOutput>();
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

                //AnnunciClient annunciClient = new AnnunciClient(await Api.ApiHelper.GetApiClient());

                List<AnnunciDtoOutput> listaAnnunci = await ApiHelper.GetListaAnnunciRecenti(false);// await annunciClient.GetRicercheRecentiCurrentAsync();

                foreach (var evento in listaAnnunci)
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

        private Task<string> DisplayActionSheet(string v1, string v2, object p, string v3, string v4)
        {
            throw new NotImplementedException();
        }
    }
}