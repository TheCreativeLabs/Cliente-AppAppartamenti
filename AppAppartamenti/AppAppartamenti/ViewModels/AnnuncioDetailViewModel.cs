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
        public Map Map { get; private set; }

        public AnnuncioDetailViewModel()
        {
            Item = new AnnuncioDtoOutput();
            Map = new Map();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                AnnunciClient annunciClient = new AnnunciClient(Api.ApiHelper.GetApiClient());
                Item = await annunciClient.GetAnnuncioByIdAsync(IdAnnuncio);

                if (!string.IsNullOrEmpty(Item.CoordinateGeografiche))
                {

                    var pos = Item.CoordinateGeografiche.Split(';');
                    var lat = pos[0];
                    var lon = pos[1];

                    Pin pin = new Pin
                    {
                        Label = Item.Indirizzo,
                        Address = $"{Item.Indirizzo},{Item.NomeComune}",
                        Type = PinType.Generic,
                        Position = new Position(Double.Parse(lat), Double.Parse(lon))
                    };

                    Map.Pins.Add(pin);
                    Map.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Position, Distance.FromMiles(0.1)));
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
