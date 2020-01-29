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
using Xamarin.Essentials;
using Xamarin.Forms.Maps;
using System.Linq;

namespace AppAppartamenti.ViewModels
{
    public class MapViewModel
    {
       public Guid IdAnnuncio { get; set; }
       public Pin AnnPin { get; set; }
    }

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
        public bool? SenzaBarriereArchitettoniche { get; set; }
        public bool? Montascale { get; set; }
        public bool? SenzaGradiniInternoProprieta { get; set; }

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
        public ObservableCollection<CustomPin> PositionItems { get; set; }

        public Command LoadItemsCommand { get; set; }
        public Command LoadMore { get; set; }
        public Command LoadAll { get; set; }
        public Command AddPreferito { get; set; }
        public Command RimuoviPreferito { get; set; }
        public RicercaModel FiltriRicerca { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsEmpty { get; set; }
        public AnnunciViewModel()
        {
            Items = new ObservableCollection<AnnunciDtoOutput>();
            PositionItems = new ObservableCollection<CustomPin>();

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            OnpropertyChanged("Items");

            this.LoadMore = new Command(async () => await LoadMoreCommand());
            this.LoadAll = new Command(async () => await LoadAllCommand());
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

                news = await GetAnnunci(false);

                if (news != null)
                {
                    foreach (var annuncio in news)
                    {
                        Items.Add(annuncio);

                        if (!string.IsNullOrEmpty(annuncio.CoordinateGeografiche))
                        {
                            var pos = annuncio.CoordinateGeografiche.Split(';');
                            double lat;
                            double.TryParse(pos[0], out lat);
                            double lon;
                            double.TryParse(pos[1], out lon);

                            PositionItems.Add(
                                new CustomPin
                                {
                                    Type = PinType.Place,
                                    Position = new Position(lat, lon),
                                    Label = $"{ annuncio.TipologiaProprieta } in { annuncio.TipologiaAnnuncio}a { annuncio.Prezzo:N0}€",
                                    Name = annuncio.Id.Value.ToString(),
                                    Address = $"{annuncio.Indirizzo} {annuncio.NomeComune}",
                                    AutomationId = annuncio.Id.Value.ToString()
                                }
                            ) ;
                        }

                        OnpropertyChanged("Items");
                        OnpropertyChanged("PositionItems");
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

        async Task LoadAllCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            ICollection<AnnunciDtoOutput> news = null;
            try
            {
                CurrentPage += 1;

                news = await GetAnnunci(true);

                if (news != null)
                {
                    foreach (var annuncio in news)
                    {
                        Items.Add(annuncio);

                        if (!string.IsNullOrEmpty(annuncio.CoordinateGeografiche))
                        {
                            var pos = annuncio.CoordinateGeografiche.Split(';');
                            double lat;
                            double.TryParse(pos[0], out lat);
                            double lon;
                            double.TryParse(pos[1], out lon);

                            PositionItems.Add(new CustomPin
                            {
                                Type = PinType.Place,
                                Position = new Position(lat, lon),
                                Label = $"{ annuncio.TipologiaProprieta } in { annuncio.TipologiaAnnuncio}a { annuncio.Prezzo:N0}€",
                                Name = annuncio.Id.Value.ToString(),
                                Address = $"{annuncio.Indirizzo} {annuncio.NomeComune}",
                                AutomationId = annuncio.Id.Value.ToString()
                            });
                        }

                        OnpropertyChanged("Items");
                        OnpropertyChanged("PositionItems");

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
            OnpropertyChanged("IsBusy");

            IsEmpty = false;
            OnpropertyChanged("IsEmpty");

            try
            {
                CurrentPage = 1;
                Items.Clear();
                PositionItems.Clear();

                ICollection<AnnunciDtoOutput> listaAnnunci = await GetAnnunci(false);

                foreach (var annuncio in listaAnnunci)
                {
                    Items.Add(annuncio);

                    if (!string.IsNullOrEmpty(annuncio.CoordinateGeografiche)) { 
                        var pos = annuncio.CoordinateGeografiche.Split(';');
                        double lat;
                        double.TryParse(pos[0],out lat);
                        double lon;
                        double.TryParse(pos[1],out lon);

                        PositionItems.Add(new CustomPin
                        {
                            Type = PinType.Place,
                            Position = new Position(lat, lon),
                            Label = $"{ annuncio.TipologiaProprieta } in { annuncio.TipologiaAnnuncio}a { annuncio.Prezzo:N0}€",
                            Name = annuncio.Id.Value.ToString(),
                            Address = $"{annuncio.Indirizzo} {annuncio.NomeComune}",
                            AutomationId = annuncio.Id.Value.ToString()
                        });
                    }
                }

                if (!Items.Any()) { 
                    IsEmpty = true;
                    OnpropertyChanged("IsEmpty");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
                OnpropertyChanged("IsBusy");
            }
        }

        async Task AddPreferitoCommand(AnnunciDtoOutput annuncio)
        {
            var position=  Items.IndexOf(annuncio);
            var ann = Items[position];
            ann.FlagPreferito = true;
            AnnunciClient annunciClient = new AnnunciClient(await Api.ApiHelper.GetApiClient());
            await annunciClient.AggiungiPreferitoAsync(ann.Id.Value);
            OnpropertyChanged("Items");
        }

        async Task RemovePreferitoCommand(AnnunciDtoOutput annuncio)
        {
            var position = Items.IndexOf(annuncio);
            var ann = Items[position];
            ann.FlagPreferito = false;
            AnnunciClient annunciClient = new AnnunciClient(await Api.ApiHelper.GetApiClient());
            await annunciClient.RimuoviPreferitoAsync(ann.Id.Value);
            OnpropertyChanged("Items");
        }


        private async Task<ICollection<AnnunciDtoOutput>> GetAnnunci(bool ViewAll)
        {
            AnnunciClient annunciClient = new AnnunciClient(await Api.ApiHelper.GetApiClient());

            ICollection<AnnunciDtoOutput> listaAnnunci = null;

            var pageSize = PageSize;
            if (ViewAll)
            {
                pageSize = 500;
            }

            listaAnnunci = await annunciClient.GetAnnunciAsync(CurrentPage, pageSize, FiltriRicerca.TipologiaAnnuncio, FiltriRicerca.TipologiaProprieta, FiltriRicerca.Comune.CodiceComune, FiltriRicerca.MinPrice, FiltriRicerca.MaxPrice,FiltriRicerca.MinSurface, FiltriRicerca.MaxSurface, FiltriRicerca.NumCamereLetto, FiltriRicerca.NumBagni, FiltriRicerca.NumCucine, FiltriRicerca.NumPostiAuto, FiltriRicerca.NumGarage,0,FiltriRicerca.Giardino, FiltriRicerca.Terrazzo, FiltriRicerca.Cantina, FiltriRicerca.Piscina, FiltriRicerca.Ascensore,FiltriRicerca.Condizionatori, FiltriRicerca.SenzaBarriereArchitettoniche, FiltriRicerca.Montascale, FiltriRicerca.SenzaGradiniInternoProprieta, null);

            return listaAnnunci;
        }
    }
}