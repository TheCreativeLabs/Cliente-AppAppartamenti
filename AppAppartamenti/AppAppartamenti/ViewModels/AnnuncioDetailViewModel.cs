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
        public List<Immagine> Immagini { get; set; }
        ////public Command LoadItemsCommand { get; set; }
        //public Guid idAnnuncio { get; set; }
        public AnnuncioDetailViewModel(AnnuncioDtoOutput item)
        {
            this.Item = item;
            this.Immagini = new List<Immagine>();
            foreach (byte[] imm in item.ImmaginiAnnuncio)
            {
                Immagine immagine = new Immagine() { Value = imm };
                this.Immagini.Add(immagine);
            }
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
