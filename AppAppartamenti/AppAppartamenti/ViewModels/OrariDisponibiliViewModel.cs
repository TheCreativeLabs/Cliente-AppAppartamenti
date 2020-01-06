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
using System.Linq;

namespace AppAppartamenti.ViewModels
{
    public class OrariDisponibiliViewModel : BaseViewModel, INotifyPropertyChanged
    {

        public class OrarioAppuntamento
        {
            public string Orario { get; set; }
        }

        public ObservableCollection<OrarioAppuntamento> Orari { get; set; }
        public DateTimeOffset Giorno { get; set; }
        public Guid IdAnnuncio { get; set; }
        public Command LoadItemsCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsEmpty { get; set; }

        void OnpropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public OrariDisponibiliViewModel(Guid IdAnnuncio)
        {
            this.IdAnnuncio = IdAnnuncio;
            Orari = new ObservableCollection<OrarioAppuntamento>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
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
                Orari.Clear();

                AgendaClient agendaClient = new AgendaClient(await Api.ApiHelper.GetApiClient());
                var listaOrari = await agendaClient.GetFasceDisponibiliAnnuncioByGiornoAsync(IdAnnuncio, Giorno); //fixme creare offset con gg

                if (!listaOrari.Any())
                {
                    IsEmpty = true;
                    OnpropertyChanged("IsEmpty");
                }

                foreach (var orario in listaOrari)
                {
                    Orari.Add(new OrarioAppuntamento() { Orario = orario});
                }
                OnpropertyChanged("Orari");

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