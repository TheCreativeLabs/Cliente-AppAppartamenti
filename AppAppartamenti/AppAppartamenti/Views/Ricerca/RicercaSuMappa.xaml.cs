using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AppAppartamenti.ViewModels;
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

            BindingContext =viewModel =  viewModelParam;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            map.MoveToRegion(MapSpan.FromCenterAndRadius(viewModel.PositionItems[0].AnnPin.Position, Distance.FromMiles(3)));

            viewModel.LoadMore.Execute(null);
        }

        private async void MarkerClicked(object sender, EventArgs e)
        {
            string Id = ((Pin)sender).AutomationId;

            if (!String.IsNullOrEmpty(Id)) { 
               var item= viewModel.Items.Where(x => x.Id == new Guid(Id)).First();
                cvLista.ScrollTo(item, null, ScrollToPosition.Center, true);
            }
        }

        private async void BtnBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
