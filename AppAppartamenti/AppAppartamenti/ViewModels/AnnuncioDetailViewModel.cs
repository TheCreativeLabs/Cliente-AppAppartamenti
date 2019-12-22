using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using AppAppartamenti.Models;
using AppAppartamentiApiClient;
using Xamarin.Forms;

namespace AppAppartamenti.ViewModels
{
    public class AnnuncioDetailViewModel : BaseViewModel
    {
        public class Immagine
        {
            public byte[] Value { get; set; }
        }

        public AnnuncioDtoOutput Item { get; set; }

        ////public Command LoadItemsCommand { get; set; }
        //public Guid idAnnuncio { get; set; }
        public AnnuncioDetailViewModel()
        {
            Item = new AnnuncioDtoOutput();
        }

        public static async Task<AnnuncioDetailViewModel> ExecuteLoadItemsCommandAsync(Guid Id)
        {
            AnnuncioDetailViewModel annuncioDetailViewModel = new AnnuncioDetailViewModel();

            try
            {
                AnnunciClient annunciClient = new AnnunciClient(Api.ApiHelper.GetApiClient());
                annuncioDetailViewModel.Item = await annunciClient.GetAnnuncioByIdAsync(Id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return annuncioDetailViewModel;
        }
    }
}
