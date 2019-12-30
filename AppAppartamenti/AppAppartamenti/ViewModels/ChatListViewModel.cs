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
    public class ChatListViewModel : BaseViewModel
    {
        public ObservableCollection<ChatListDtoOutput> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ChatListViewModel()
        {
            Items = new ObservableCollection<ChatListDtoOutput>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
          
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();

                MessaggiClient messaggiClient = new MessaggiClient(await Api.ApiHelper.GetApiClient());

                ICollection<ChatListDtoOutput> listaChat;
                listaChat = await messaggiClient.GetChatListAsync();
                foreach (var msg in listaChat)
                {
                    Items.Add(msg);
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