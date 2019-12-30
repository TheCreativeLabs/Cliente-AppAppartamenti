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
    public class MessaggiViewModel : BaseViewModel
    {
        public ObservableCollection<MessaggioDto> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public Guid? IdChatParam { get; set; }
        public Guid? IdAnnuncioParam { get; set; }

        public MessaggiViewModel()
        {
            Items = new ObservableCollection<MessaggioDto>();
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
                var chatInfo = await messaggiClient.GetChatAsync(IdChatParam,IdAnnuncioParam);

                if(chatInfo != null)
                {
                    foreach (var msg in chatInfo.Messaggi)
                    {
                        Items.Add(msg);
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

        private Task<string> DisplayActionSheet(string v1, string v2, object p, string v3, string v4)
        {
            throw new NotImplementedException();
        }
    }
}