using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AppAppartamenti.Models;
using AppAppartamentiApiClient;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace AppAppartamenti.ViewModels
{
    public class AnnuncioDetailViewModel : BaseViewModel, INotifyPropertyChanged
    {

        public Guid IdAnnuncio { get; set; }
        public AnnuncioDtoOutput Item { get; set; }
        public Command LoadItemsCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public AnnuncioDetailViewModel()
        {
            Item = new AnnuncioDtoOutput();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            OnpropertyChanged("Item");

        }


        void OnpropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
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
                OnpropertyChanged("Item");

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
