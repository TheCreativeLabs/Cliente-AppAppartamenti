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

    public class RicercaModel
    {
        public ComuneDto Comune { get; set; }
        public Guid? TipologiaAnnuncio { get; set; }
        public Guid? TipologiaProprieta { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public int MinSurface{ get; set; }
        public int MaxSurface { get; set; }
        public int NumCamereLetto { get; set; }
        public int NumBagni { get; set; }
        public int NumCucine { get; set; }
        public int NumPostiAuto { get; set; }
        public int NumGarage { get; set; }
        public bool? Giardino { get; set; }
        public bool? Terrazzo { get; set; }
        public bool? Cantina { get; set; }
        public bool? Piscina { get; set; }
        public bool? Ascensore { get; set; }
        public bool? Condizionatori { get; set; }

        public RicercaModel()
        {
        }
    }

    public enum TipiRicerca
    {
        MieiAnnunci = 0,
        Tutti = 1,
        Preferiti = 2
    }

    public class AnnunciViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private int CurrentPage = 1;
        private int PageSize = 5;

        public ObservableCollection<AnnunciDtoOutput> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Command LoadMore { get; set; }
        public Command AddPreferito { get; set; }
        public Command RimuoviPreferito { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
      
        RicercaModel FiltriRicerca { get; set; }

        public AnnunciViewModel(RicercaModel FiltriRicercaParam)
        {
            FiltriRicerca = FiltriRicercaParam;

            Items = new ObservableCollection<AnnunciDtoOutput>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            OnpropertyChanged("Items");
            this.LoadMore = new Command(async () => await LoadMoreCommand());
            this.AddPreferito = new Command<AnnunciDtoOutput>(async item => await AddPreferitoCommand(item));
            this.RimuoviPreferito = new Command<AnnunciDtoOutput>(async item => await RemovePreferitoCommand(item));
        }

        void OnpropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        async Task LoadMoreCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            ICollection<AnnunciDtoOutput> news = null;
            try
            {
                CurrentPage += 1;

                news = await GetAnnunci();

                if (news != null)
                {
                    foreach (var item in news)
                    {
                        Items.Add(item);
                        OnpropertyChanged("Items");
                    }
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

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                CurrentPage = 1;
                Items.Clear();

                ICollection<AnnunciDtoOutput> listaAnnunci = await GetAnnunci();
                
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

        async Task AddPreferitoCommand(AnnunciDtoOutput annuncio)
        {
            var position=  Items.IndexOf(annuncio);
            var ann = Items[position];
            ann.FlagPreferito = true;
            AnnunciClient annunciClient = new AnnunciClient(Api.ApiHelper.GetApiClient());
            await annunciClient.AggiungiPreferitoAsync(ann.Id.Value);
            OnpropertyChanged("Items");
        }

        async Task RemovePreferitoCommand(AnnunciDtoOutput annuncio)
        {
            var position = Items.IndexOf(annuncio);
            var ann = Items[position];
            ann.FlagPreferito = false;
            AnnunciClient annunciClient = new AnnunciClient(Api.ApiHelper.GetApiClient());
            await annunciClient.RimuoviPreferitoAsync(ann.Id.Value);
            OnpropertyChanged("Items");
        }


        private async Task<ICollection<AnnunciDtoOutput>> GetAnnunci()
        {
            AnnunciClient annunciClient = new AnnunciClient(Api.ApiHelper.GetApiClient());

            ICollection<AnnunciDtoOutput> listaAnnunci = null;
            
            listaAnnunci = await annunciClient.GetAnnunciAsync(CurrentPage, PageSize, FiltriRicerca.TipologiaAnnuncio, FiltriRicerca.TipologiaProprieta, FiltriRicerca.Comune.CodiceComune, FiltriRicerca.MinPrice, FiltriRicerca.MaxPrice,FiltriRicerca.MinSurface, FiltriRicerca.MaxSurface, FiltriRicerca.NumCamereLetto, FiltriRicerca.NumBagni, FiltriRicerca.NumCucine, FiltriRicerca.NumPostiAuto, FiltriRicerca.NumGarage,0,FiltriRicerca.Giardino, FiltriRicerca.Terrazzo, FiltriRicerca.Cantina, FiltriRicerca.Piscina, FiltriRicerca.Ascensore,FiltriRicerca.Condizionatori,null);

            return listaAnnunci;
        }
    }
}