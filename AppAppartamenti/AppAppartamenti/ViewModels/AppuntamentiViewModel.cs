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
    public class AppuntamentiViewModel : BaseViewModel
    {
        public DateTime SelectedDate { get; set; }
        public ObservableCollection<AppuntamentoDtoOutput> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public AppuntamentiViewModel()
        {
            Items = new ObservableCollection<AppuntamentoDtoOutput>();
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

                AgendaClient agendaClient = new AgendaClient(await Api.ApiHelper.GetApiClient());
                var lista =await agendaClient.GetAgendaCurrentByGiornoAsync(SelectedDate);

                foreach (var item in lista)
                {
                    Items.Add(item);
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