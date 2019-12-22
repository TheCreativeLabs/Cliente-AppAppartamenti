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
    public class OrariDisponibiliViewModel : BaseViewModel
    {

        public class OrarioAppuntamento
        {
            public string Orario { get; set; }
        }

        public ObservableCollection<OrarioAppuntamento> Orari { get; set; }
        public DateTimeOffset Giorno { get; set; }
        public Guid IdAnnuncio { get; set; }
        public Command LoadItemsCommand { get; set; }

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

            try
            {
                Orari.Clear();

                AgendaClient agendaClient = new AgendaClient(Api.ApiHelper.GetApiClient());
                ICollection<string> listaOrari = await agendaClient.GetFasceDisponibiliAnnuncioByGiornoAsync(IdAnnuncio, Giorno); //fixme creare offset con gg

                foreach (var orario in listaOrari)
                {
                    Orari.Add(new OrarioAppuntamento() { Orario = orario});
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