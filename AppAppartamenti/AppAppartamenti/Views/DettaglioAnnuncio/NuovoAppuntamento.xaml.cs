using AppAppartamenti.ViewModels;
using AppAppartamentiApiClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using NavigationPage = Xamarin.Forms.NavigationPage;

namespace AppAppartamenti.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NuovoAppuntamento : ContentPage
    {
        //AnnuncioDetailViewModel viewModel;
        OrariDisponibiliViewModel orariDisponibiliViewModel;
        Guid IdAnnuncio;

        public NuovoAppuntamento(Guid Id)
        {
            InitializeComponent();

            IdAnnuncio = Id;
        }

        protected async override void OnAppearing()
        {
            BindingContext = orariDisponibiliViewModel = new OrariDisponibiliViewModel(IdAnnuncio);
            base.OnAppearing();
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        public void OnDataCalendarSelected(object sender, SelectionChangedEventArgs e)//EventHandler<SelectionChangedEventArgs> e
        {
            Syncfusion.SfCalendar.XForms.SfCalendar cal = (Syncfusion.SfCalendar.XForms.SfCalendar)sender;
            DateTime? i = cal.SelectedDate;

            //List<DateTime> dates = args.DateAdded;
            //DateTime date = dates.FirstOrDefault();
            if( i != null)
            {
                DateTime date = (DateTime) i;
                DateTimeOffset giorno = new DateTimeOffset(date.Year, date.Month, date.Day, 0, 0, 0, new TimeSpan(0, 0, 0));
                orariDisponibiliViewModel.Giorno = giorno;
                orariDisponibiliViewModel.LoadItemsCommand.Execute(null);
                containerOrariDisponibili.IsVisible = true;
            }



            //return null;
        }
    }
}