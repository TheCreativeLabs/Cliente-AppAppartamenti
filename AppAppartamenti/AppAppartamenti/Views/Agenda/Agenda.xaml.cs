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

           BindingContext = viewModel = new AppuntamentiViewModel(DateTime.Now);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        public void OnDataCalendarSelected(object sender, SelectionChangedEventArgs e)
        {
            Syncfusion.SfCalendar.XForms.SfCalendar cal = (Syncfusion.SfCalendar.XForms.SfCalendar)sender;
            DateTime? currentDate = cal.SelectedDate;

            if (currentDate.HasValue) { 
                viewModel = new AppuntamentiViewModel(currentDate.Value);
                viewModel.LoadItemsCommand.Execute(null);
            }
        }
    }
}