using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using AppAppartamenti.Models;
using AppAppartamenti.Views;
using AppAppartamentiApiClient;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AppAppartamenti.Api;

namespace AppAppartamenti.ViewModels
{
    public class ChatListViewModel : BaseViewModel,INotifyPropertyChanged
    {
        public ObservableCollection<ChatListDtoOutput> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        void OnpropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public ChatListViewModel()
        {
            Items = new ObservableCollection<ChatListDtoOutput>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            OnpropertyChanged("Items");

        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                MessaggiClient messaggiClient = new MessaggiClient(await Api.ApiHelper.GetApiClient());

                ICollection<ChatListDtoOutput> listaChat;

                listaChat = await messaggiClient.GetChatListAsync();

                //if(listaChat.Where(x=>x.NumberMsgToRead > 0).Any())
                //{
                Items.Clear();

                foreach (var msg in listaChat)
                {
                    Items.Add(msg);
                }

                OnpropertyChanged("Items");

                //}
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

        public async Task UpdateItems(Guid IdChat)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var item = Items.Where(x => x.IdChat.Value == IdChat).FirstOrDefault();
                var index = Items.IndexOf(item);
                Items.RemoveAt(index);
                Items.Insert(index, item);
                item.NumberMsgToRead = 0;

                OnpropertyChanged("Items");

                //}
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