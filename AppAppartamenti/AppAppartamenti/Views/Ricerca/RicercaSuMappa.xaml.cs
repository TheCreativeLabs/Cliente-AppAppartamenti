using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using AppAppartamenti.ViewModels;
using AppAppartamentiApiClient;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace AppAppartamenti.Views
{
    public partial class RicercaSuMappa : ContentPage
    {
        AnnunciViewModel viewModel;
        public RicercaSuMappa(AnnunciViewModel viewModelParam)
        {
            InitializeComponent();

            BindingContext = viewModel = viewModelParam;
            map.CustomPins = viewModel.PositionItems.ToList();
            foreach (var item in viewModel.PositionItems)
            {
                var pin = item;
                pin.MarkerClicked += (sender, e) => { MarkerClicked(sender, e); };
                map.Pins.Add(pin);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                map.MoveToRegion(MapSpan.FromCenterAndRadius(viewModel.PositionItems[0].Position, Distance.FromMiles(3)));
            }
            catch (Exception ex)
            {

            }

            viewModel.LoadMore.Execute(null);
        }

        private void MarkerClicked(object sender, EventArgs e)
        {
            var currentPin = ((Pin)sender);
            if (currentPin == null)
                return;

            string Id = ((Pin)sender).AutomationId;
            if (Id == null || string.IsNullOrEmpty(Id))
                return;

            var item = viewModel.Items.Where(x => x.Id.Value == new Guid(Id)).First();
            cvLista.CurrentItem = item;
            cvLista.ScrollTo(item, null, ScrollToPosition.Center, false);
        }

        void OnPositionChanged(object sender, PositionChangedEventArgs e)
        {
            try
            {
                var pins = map.Pins.Where(x => ((CustomPin)x).IsSelected).ToList();
                foreach (var item in pins)
                {
                    map.Pins.Remove(item);
                    map.CustomPins.Remove((CustomPin)item);
                    ((CustomPin)item).IsSelected = false;
                    map.Pins.Add(item);
                    map.CustomPins.Add((CustomPin)item);
                }

                int previousPosition = e.PreviousPosition;
                int currentPosition = e.CurrentPosition;

                if (currentPosition >= 0)
                {
                    var i = viewModel.Items[currentPosition];

                    if (i != null)
                    {
                        var pin = map.Pins.Where(x => x.AutomationId == i.Id.ToString()).FirstOrDefault();
                        if (pin != null)
                        {
                            map.Pins.Remove(pin);
                            map.CustomPins.Remove((CustomPin)pin);
                            ((CustomPin)pin).IsSelected = true;
                            map.Pins.Add(pin);
                            map.CustomPins.Add((CustomPin)pin);
                            map.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Position, Distance.FromMiles(3)));
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

       private async void BtnAnnuncio_Clicked(object sender, EventArgs e)
        {
            try
            {
                TappedEventArgs args = (TappedEventArgs)e;
                var item = args.Parameter as AnnunciDtoOutput;

                MessagingCenter.Send(this, "RicercaSuMappa", JsonConvert.SerializeObject(item));

                await Navigation.PopModalAsync(false);
            }
            catch (Exception Ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }

        void OnCollectionViewScrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            try
            {
                int currentIndexItem = 1;// e.FirstVisibleItemIndex;
                if (currentIndexItem > 0)
                {
                    var i = viewModel.Items[currentIndexItem];

                    if (i != null)
                    {
                        var pin = map.Pins.Where(x => x.AutomationId == i.Id.ToString()).FirstOrDefault();

                        map.Pins.Remove(pin);

                        ((CustomPin)pin).IsSelected = true;
                        map.Pins.Add(pin);

                        if (pin != null)
                        {
                            map.MoveToRegion(MapSpan.FromCenterAndRadius(pin.Position, Distance.FromMiles(0.3)));
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async void BtnBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
