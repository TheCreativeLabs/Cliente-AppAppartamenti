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
    public class appuntamento
	{
        public DateTime DataAppuntamento { get; set; }
		public string Luogo { get; set; }
		public string Partecipante { get; set; }
	}

	public class AppuntamentiViewModel : BaseViewModel
    {
        private DateTime SelectedDate { get; set; }
        public ObservableCollection<appuntamento> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public AppuntamentiViewModel(DateTime SelectedDay)
        {
			SelectedDate = SelectedDay;
            Items = new ObservableCollection<appuntamento>();
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

                AnnunciClient annunciClient = new AnnunciClient(Api.ApiHelper.GetApiClient());

				//listaAnnunci = await annunciClient.GetAnnunciByUserAsync(1,1000);

				for (int i = 0; i < 4; i++)
				{
					items.Add(new appuntamento() { DataAppuntamento = DateTime.Now(), Luogo = "Bologna", "Matteo" });
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