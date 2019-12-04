using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using AppAppartamenti.Models;
using AppAppartamenti.Views;
using AppAppartamentiApiClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AppAppartamenti.ViewModels
{
    public enum TipiRicerca
    {
        MieiAnnunci = 0,
        Tutti = 1,
        Preferiti = 2
    }

    public class AnnunciViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public ObservableCollection<AnnunciDtoOutput> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Command LoadMore { get; set; }
        private TipiRicerca TipoRicerca { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private int currentPage = 1;
        private int pageSize = 5;

        public AnnunciViewModel(TipiRicerca tipoRicerca)
        {
            Items = new ObservableCollection<AnnunciDtoOutput>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            TipoRicerca = tipoRicerca;


            OnpropertyChanged("Items");
            this.LoadMore = new Command(async () => {
                //var newNews = repo.getNews(1);
                ICollection<AnnunciDtoOutput> news = null;
                AnnunciClient annunciClient = new AnnunciClient(Api.ApiHelper.GetApiClient());


                if (TipoRicerca == TipiRicerca.MieiAnnunci)
                {
                    news = await annunciClient.GetAnnunciByUserAsync(currentPage, pageSize); //quando ricarico prendo la prima pagina
                }
                else if (TipoRicerca == TipiRicerca.Tutti)
                {
                    news = await annunciClient.GetAnnunciAsync(currentPage, pageSize, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
                    //quando ricarico prendo la prima pagina
                }

                currentPage += 1;

                if(news != null) { 
                    foreach (var item in news)
                    {
                        Items.Add(item);
                        OnpropertyChanged("Items");
                    }
                }
            });

        }

        void OnpropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                currentPage = 1;
                Items.Clear();

                AnnunciClient annunciClient = new AnnunciClient(Api.ApiHelper.GetApiClient());

                ICollection<AnnunciDtoOutput> listaAnnunci = null;

                if (TipoRicerca == TipiRicerca.MieiAnnunci)
                {
                    listaAnnunci = await annunciClient.GetAnnunciByUserAsync(currentPage,pageSize); //quando ricarico prendo la prima pagina
                }
                else if(TipoRicerca == TipiRicerca.Tutti)
                {
                    listaAnnunci = await annunciClient.GetAnnunciAsync(currentPage, pageSize, null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null, null, null);
                    //quando ricarico prendo la prima pagina
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

        private Task<string> DisplayActionSheet(string v1, string v2, object p, string v3, string v4)
        {
            throw new NotImplementedException();
        }
    }
}