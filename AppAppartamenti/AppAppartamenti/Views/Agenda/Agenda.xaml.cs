using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using AppAppartamenti.Models;
using AppAppartamenti.Views;
using AppAppartamenti.ViewModels;
using AppAppartamentiApiClient;

namespace AppAppartamenti.Views
{

    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class Agenda : ContentPage
    {
        AppuntamentiViewModel viewModel;

        public Agenda()
        {
            InitializeComponent();

           BindingContext = viewModel = new AppuntamentiViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.SelectedDate = DateTime.Now;
                viewModel.LoadItemsCommand.Execute(null);
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as AppuntamentoDtoOutput;

            if (item == null)
                return;

            await Navigation.PushAsync(new DettaglioAppuntamento(item));

            lvAppuntamenti.SelectedItem = null;
        }

        public void OnDataCalendarSelected(object sender, SelectionChangedEventArgs e)
        {
            Syncfusion.SfCalendar.XForms.SfCalendar cal = (Syncfusion.SfCalendar.XForms.SfCalendar)sender;
            DateTime? currentDate = cal.SelectedDate;

            if (currentDate.HasValue) {
                viewModel.SelectedDate = currentDate.Value;
                viewModel.LoadItemsCommand.Execute(null);
                BindingContext = viewModel;
            }
        }
    }
}