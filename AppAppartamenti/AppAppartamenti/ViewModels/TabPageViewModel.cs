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
    public class TabPageViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public int NewMessages { get; set; }
        public int NewAppointement { get; set; }

        public Command LoadItemsCommand { get; set; }
        public Command ReloadItemsCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public TabPageViewModel()
        {
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            ReloadItemsCommand = new Command(async () => await ExecuteReloadItemsCommand());
            OnpropertyChanged("NewMessages");
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
                MessaggiClient messaggiClient = new MessaggiClient(await Api.ApiHelper.GetApiClient());
                NewMessages =  await messaggiClient.GetChatMessagesToReadAsync();
                OnpropertyChanged("NewMessages");

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

        async Task ExecuteReloadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                MessaggiClient messaggiClient = new MessaggiClient(await Api.ApiHelper.GetApiClient());
                NewMessages = await messaggiClient.GetChatMessagesToReadAsync();
                OnpropertyChanged("NewMessages");
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