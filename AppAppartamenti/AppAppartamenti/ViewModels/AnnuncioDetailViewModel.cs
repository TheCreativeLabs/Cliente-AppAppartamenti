using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using AppAppartamenti.Models;
using AppAppartamentiApiClient;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace AppAppartamenti.ViewModels
{
    public class AnnuncioDetailViewModel : BaseViewModel
    {

        public Guid IdAnnuncio { get; set; }
        public AnnuncioDtoOutput Item { get; set; }
        public Command LoadItemsCommand { get; set; }

        public AnnuncioDetailViewModel()
        {
            Item = new AnnuncioDtoOutput();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            OnPropertyChanged("Item");

        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                AnnunciClient annunciClient = new AnnunciClient(await Api.ApiHelper.GetApiClient());
                Item = await annunciClient.GetAnnuncioByIdAsync(IdAnnuncio);
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

        public static async Task<AnnuncioDetailViewModel> ExecuteLoadItemsCommandAsync(Guid Id)
        {
            AnnuncioDetailViewModel annuncioDetailViewModel = new AnnuncioDetailViewModel();

            try
            {
                AnnunciClient annunciClient = new AnnunciClient(await Api.ApiHelper.GetApiClient());
                annuncioDetailViewModel.Item = await annunciClient.GetAnnuncioByIdAsync(Id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw new ApplicationException(ex.Message);
            }

            return annuncioDetailViewModel;
        }
    }
}
