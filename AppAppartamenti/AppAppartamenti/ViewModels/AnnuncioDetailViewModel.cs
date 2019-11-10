using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AppAppartamenti.Models;
using AppAppartamentiApiClient;
using Xamarin.Forms;

namespace AppAppartamenti.ViewModels
{
    public class AnnuncioDetailViewModel : BaseViewModel
    {
        public AnnuncioDtoOutput Item { get; set; }
        ////public Command LoadItemsCommand { get; set; }
        //public Guid idAnnuncio { get; set; }
        public AnnuncioDetailViewModel(AnnuncioDtoOutput item)
        {
            //if (idAnnuncio != null && idAnnuncio!= Guid.Empty)
            //{
                this.Item = item;
               // LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            //}
            //else
            //{
            //    Item = new AnnuncioDtoOutput(); 
            //}
            
        }

        //async Task ExecuteLoadItemsCommand()
        //{
        //    if (IsBusy)
        //        return;

        //    IsBusy = true;

        //    try
        //    {
        //        AnnunciClient annunciClient = new AnnunciClient(Api.ApiHelper.GetApiClient());
        //        Item = await annunciClient.GetAnnuncioByIdAsync(this.idAnnuncio);

        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex);
        //    }
        //    finally
        //    {
        //        IsBusy = false;
        //    }
        //}
    }
}
