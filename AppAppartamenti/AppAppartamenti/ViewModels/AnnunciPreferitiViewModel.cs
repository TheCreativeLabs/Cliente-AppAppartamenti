using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using AppAppartamenti.Models;
using AppAppartamenti.Views;
using AppAppartamentiApiClient;
using System.Collections.Generic;
using Xamarin.Forms.Maps;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;

namespace AppAppartamenti.ViewModels
{
    public class AnnunciPreferitiViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public ObservableCollection<AnnunciDtoOutput> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public bool IsEmpty { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;


        void OnpropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public AnnunciPreferitiViewModel()
        {
            Items = new ObservableCollection<AnnunciDtoOutput>();
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
                Items.Clear();

                AnnunciClient annunciClient = new AnnunciClient(await Api.ApiHelper.GetApiClient());

                ICollection<AnnunciDtoOutput> listaAnnunci;

                listaAnnunci = await annunciClient.GetAnnunciPreferitiAsync();

                foreach (var evento in listaAnnunci)
                {
                    Items.Add(evento);
                }

                if (!Items.Any())
                {
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

        private Task<string> DisplayActionSheet(string v1, string v2, object p, string v3, string v4)
        {
            throw new NotImplementedException();
        }
    }
}