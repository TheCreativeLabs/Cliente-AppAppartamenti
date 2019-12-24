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
        //AppuntamentiViewModel viewModel;

        public Agenda()
        {
            InitializeComponent();

           // BindingContext = viewModel = new AppuntamentiVieModel(DateTime.Now());
            


            //TimeSlotViewModel timeSlotViewModel = new TimeSlotViewModel();
            //picker.ItemsSource = timeSlotViewModel.Time;
            //picker.ColumnHeaderText = timeSlotViewModel.Header;
        }

        private async void BtnBack_Clicked(object sender, EventArgs e)
        {
            try
            {
                //picker.IsOpen = true;
            }
            catch (Exception Ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }
    }
}