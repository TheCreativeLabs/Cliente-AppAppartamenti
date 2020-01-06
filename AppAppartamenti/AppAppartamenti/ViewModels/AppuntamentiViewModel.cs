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
    public class AppuntamentiViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public DateTime SelectedDate { get; set; }
        public ObservableCollection<AppuntamentoDtoOutput> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public bool IsEmpty { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public AppuntamentiViewModel()
        {
            Items = new ObservableCollection<AppuntamentoDtoOutput>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
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
            OnpropertyChanged("IsBusy");


            IsEmpty = false;
            OnpropertyChanged("IsEmpty");
            try
            {
                Items.Clear();

                AgendaClient agendaClient = new AgendaClient(await Api.ApiHelper.GetApiClient());
                var lista =await agendaClient.GetAgendaCurrentByGiornoAsync(SelectedDate);

                if (!lista.Any())
                {
                    IsEmpty = true;
                    OnpropertyChanged("IsEmpty");
                }

                foreach (var item in lista)
                {
                    Items.Add(item);
                }
                OnpropertyChanged("Items");

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

        private Task<string> DisplayActionSheet(string v1, string v2, object p, string v3, string v4)
        {
            throw new NotImplementedException();
        }
    }
}