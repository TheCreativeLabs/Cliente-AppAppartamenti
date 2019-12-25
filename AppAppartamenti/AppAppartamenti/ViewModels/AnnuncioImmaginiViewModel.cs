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
    public class AnnuncioimmaginiViewModel : BaseViewModel
    {
        public Guid IdAnnuncio { get; set; }
        public ObservableCollection<Byte[]> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public AnnuncioimmaginiViewModel()
        {
            Items = new ObservableCollection<Byte[]>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        public  async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {

                AnnunciClient annunciClient = new AnnunciClient(Api.ApiHelper.GetApiClient());

                ICollection<byte[]> immagini = await annunciClient.GetImmaginiByIdAnnuncioAsync(IdAnnuncio);

                Items.Clear();

                foreach (var img in immagini)
                {
                    Items.Add(img);
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