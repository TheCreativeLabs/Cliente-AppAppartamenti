﻿using System;
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
    public enum TipiRicerca
    {
        MieiAnnunci = 0,
        Tutti = 1,
        Preferiti = 2
    }

    public class AnnunciViewModel : BaseViewModel
    {
        public ObservableCollection<AnnunciDtoOutput> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        private TipiRicerca TipoRicerca { get; set; }

        public AnnunciViewModel(TipiRicerca tipoRicerca)
        {
            Items = new ObservableCollection<AnnunciDtoOutput>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            TipoRicerca = tipoRicerca;
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

                ICollection<AnnunciDtoOutput> listaAnnunci = null;

                if (TipoRicerca == TipiRicerca.MieiAnnunci)
                {
                    listaAnnunci = await annunciClient.GetAnnunciByUserAsync();
                }
                else if(TipoRicerca == TipiRicerca.Tutti)
                {
                    listaAnnunci = await annunciClient.GetAnnunciAsync();
                }
                //else if (TipoRicerca == TipiRicerca.Preferiti)
                //{
                //    listaAnnunci = await annunciClient.getp();
                //}

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
    }
}