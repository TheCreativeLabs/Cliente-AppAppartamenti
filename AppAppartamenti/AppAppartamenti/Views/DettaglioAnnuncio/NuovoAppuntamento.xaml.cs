using AppAppartamenti.Api;
using AppAppartamenti.ViewModels;
using AppAppartamentiApiClient;
using Syncfusion.SfCalendar.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace AppAppartamenti.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NuovoAppuntamento : ContentPage
    {
        //AnnuncioDetailViewModel viewModel;
        OrariDisponibiliViewModel orariDisponibiliViewModel;
        Guid IdAnnuncio;

        static Helpers.TranslateExtension translate = new Helpers.TranslateExtension();

        public CalendarEventCollection CalendarInlineEvents { get; set; } = new CalendarEventCollection();

        public NuovoAppuntamento(Guid Id)
        {
            InitializeComponent();

            IdAnnuncio = Id;


          
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            BindingContext = orariDisponibiliViewModel = new OrariDisponibiliViewModel(IdAnnuncio);
            ShowTimeSlot(calendar.SelectedDate);

           ShowAvailableDate();
        }

        /// <summary>
        /// Visualizza le date disponibili
        /// </summary>
        async private void ShowAvailableDate()
        {
            try
            {
                var firstVisibleDate = calendar.VisibleDates[0];
           
                AgendaClient agendaClient = new AgendaClient(await ApiHelper.GetApiClient());
                var listaDate = await agendaClient.GetGiorniDisponibiliByMeseAsync(IdAnnuncio, new DateTimeOffset(firstVisibleDate));

                List<DateTime> black_Dates = new List<DateTime>();
                var numDayOfYear = DateTime.DaysInMonth(firstVisibleDate.Year, firstVisibleDate.Month);

                for (int i = 1; i < numDayOfYear + 1; i++)
                {
                    var e = listaDate.Where(x => x.Day == i).ToList();

                    if (!e.Any())
                    {
                        black_Dates.Add(new DateTime(firstVisibleDate.Year, firstVisibleDate.Month, i));
                    }
                }
                calendar.BlackoutDates = black_Dates;

                }
            catch (Exception ex)
            {

            }
        }


        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        public void OnDataCalendarSelected(object sender, Xamarin.Forms.SelectionChangedEventArgs e)
        {
            Syncfusion.SfCalendar.XForms.SfCalendar cal = (Syncfusion.SfCalendar.XForms.SfCalendar)sender;

            ShowTimeSlot(cal.SelectedDate);
        }

        private void ShowTimeSlot(DateTime? SelectedDate)
        {
            if (SelectedDate.HasValue)
            {
                DateTime date = SelectedDate.Value;
                DateTimeOffset giorno = new DateTimeOffset(date.Year, date.Month, date.Day, 0, 0, 0, new TimeSpan(0, 0, 0));
                orariDisponibiliViewModel.Giorno = giorno;
                orariDisponibiliViewModel.LoadItemsCommand.Execute(null);
            }
        }

        public async void OnOrarioSelected(object sender, EventArgs e)
        {
            DateTime? dateCalendar = calendar.SelectedDate;

            try
            {
                DateTime date;
                if(dateCalendar == null)
                {
                    throw(new Exception());
                }
                else
                {
                    date = (DateTime)dateCalendar;
                }
                Label label = (Label)sender;
                string OraSelezionata = label.Text.Split('-')[0];

                bool answer = await App.Current.MainPage.DisplayAlert(Helpers.TranslateExtension.ResMgr.Value.GetString("NuovoAppuntamento.ConfirmTitle", translate.ci),
                    Helpers.TranslateExtension.ResMgr.Value.GetString("NuovoAppuntamento.ConfirmText1", translate.ci)
                    + date.ToShortDateString() + " "
                    + Helpers.TranslateExtension.ResMgr.Value.GetString("NuovoAppuntamento.ConfirmText2", translate.ci) + " "
                    + OraSelezionata 
                    + Helpers.TranslateExtension.ResMgr.Value.GetString("NuovoAppuntamento.ConfirmText3", translate.ci),
                    Helpers.TranslateExtension.ResMgr.Value.GetString("NuovoAppuntamento.ConfirmYes", translate.ci),
                    Helpers.TranslateExtension.ResMgr.Value.GetString("NuovoAppuntamento.ConfirmNo", translate.ci)
                    );
                if (answer)
                {
                    DateTimeOffset giornoEOra = new DateTimeOffset(date.Year, date.Month, date.Day, Int32.Parse(OraSelezionata.Split(':')[0]), Int32.Parse(OraSelezionata.Split(':')[1]), 0, new TimeSpan(0, 0, 0));
                    AppuntamentoDto appuntamento = new AppuntamentoDto()
                    {
                        IdAnnuncio = this.IdAnnuncio,
                        Data = giornoEOra
                    };

                    AgendaClient agendaClient = new AgendaClient(await ApiHelper.GetApiClient());
                    await agendaClient.InsertAppuntamentoAsync(appuntamento);
                    await App.Current.MainPage.DisplayAlert(Helpers.TranslateExtension.ResMgr.Value.GetString("NuovoAppuntamento.SuccessTitle", translate.ci),
                                                            Helpers.TranslateExtension.ResMgr.Value.GetString("NuovoAppuntamento.SuccessText", translate.ci),
                                                            Helpers.TranslateExtension.ResMgr.Value.GetString("NuovoAppuntamento.SuccessOk", translate.ci)
                                                           );
                    orariDisponibiliViewModel.LoadItemsCommand.Execute(null);
                }
            }
            catch (Exception ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }

        async void calendar_MonthChanged(System.Object sender, Syncfusion.SfCalendar.XForms.MonthChangedEventArgs e)
        {
            ShowAvailableDate();
        }
    }

}