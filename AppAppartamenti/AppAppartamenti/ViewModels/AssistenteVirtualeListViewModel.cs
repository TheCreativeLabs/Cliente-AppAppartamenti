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
using AppAppartamenti.Api;

namespace AppAppartamenti.ViewModels
{

    public class AssistenteVirtualeListViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public ObservableCollection<AvatarDtoOutput> Items { get; set; }
        public AvatarDtoOutput SelectedItem { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public Command LoadItemsCommand { get; set; }
      
        public AssistenteVirtualeListViewModel()
        {
            Items = new ObservableCollection<AvatarDtoOutput>();
            OnpropertyChanged("Items");
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
            try
            {
                Items.Clear();

                AccountClient  accountClient = new AccountClient(await Api.ApiHelper.GetApiClient());
                var avatars = await accountClient.GetAllAvatarAsync();

                foreach (var item in avatars)
                {
                    Items.Add(item);

                    if (item.IsAvatarOfCurrentUser.HasValue && item.IsAvatarOfCurrentUser == true)
                    {
                        SelectedItem = item;
                     
                    }
                }

                OnpropertyChanged("Items");
                OnpropertyChanged("SelectedItem");
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
    }
}