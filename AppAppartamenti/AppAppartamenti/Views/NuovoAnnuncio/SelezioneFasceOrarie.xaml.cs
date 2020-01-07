using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppAppartamenti.Api;
using AppAppartamenti.ViewModels;
using AppAppartamentiApiClient;
using DependencyServiceDemos;
using RestSharp.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace AppAppartamenti.Views
{
    public class DayItem
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public Color BackgroundColor { get; set; }
    }

    public class TimeSlot
    {
        //public ObservableCollection<string> Dalle { get; set; }
        //public ObservableCollection<string> Alle { get; set; }
        public string Dalle { get; set; }
        public string Alle { get; set; }
        public bool DeleteEnabled { get; set; }
        public int GiornoRiferimento { get; set; }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelezioneFasceOrarie : ContentPage
    {
        ObservableCollection<TimeSlot> listTimeSlotLunedi = new ObservableCollection<TimeSlot>();
        ObservableCollection<TimeSlot> listTimeSlotMartedi = new ObservableCollection<TimeSlot>();
        ObservableCollection<TimeSlot> listTimeSlotMercoledi = new ObservableCollection<TimeSlot>();
        ObservableCollection<TimeSlot> listTimeSlotGiovedi = new ObservableCollection<TimeSlot>();
        ObservableCollection<TimeSlot> listTimeSlotVenerdi = new ObservableCollection<TimeSlot>();
        ObservableCollection<TimeSlot> listTimeSlotSabato = new ObservableCollection<TimeSlot>();
        ObservableCollection<TimeSlot> listTimeSlotDomenica = new ObservableCollection<TimeSlot>();

        bool Lunedi;
        bool Martedi;
        bool Mercoledi;
        bool Giovedi;
        bool Venerdi;
        bool Sabato;
        bool Domenica;

        AnnuncioDtoInput annuncio = new AnnuncioDtoInput();
        AnnuncioDetailViewModel dtoToModify;

        public SelezioneFasceOrarie(AnnuncioDtoInput Annuncio, AnnuncioDetailViewModel dtoToModify)
        {
            this.dtoToModify = dtoToModify;
            InitializeComponent();
            InitializeFasceOrarie();
            annuncio = Annuncio;
        }

        private void InitializeFasceOrarie()
        {
            if (dtoToModify != null)
            {
                CreateTimeSlotInput(dtoToModify.Item.DisponibilitaOraria.FasceOrarieLunedi, ref listTimeSlotLunedi, 1);
                CreateTimeSlotInput(dtoToModify.Item.DisponibilitaOraria.FasceOrarieMartedi, ref listTimeSlotMartedi, 2);
                CreateTimeSlotInput(dtoToModify.Item.DisponibilitaOraria.FasceOrarieMercoledi, ref listTimeSlotMercoledi, 3);
                CreateTimeSlotInput(dtoToModify.Item.DisponibilitaOraria.FasceOrarieGiovedi, ref listTimeSlotGiovedi, 4);
                CreateTimeSlotInput(dtoToModify.Item.DisponibilitaOraria.FasceOrarieVenerdi, ref listTimeSlotVenerdi, 5);
                CreateTimeSlotInput(dtoToModify.Item.DisponibilitaOraria.FasceOrarieSabato, ref listTimeSlotSabato, 6);
                CreateTimeSlotInput(dtoToModify.Item.DisponibilitaOraria.FasceOrarieDomenica, ref listTimeSlotDomenica, 7);
            }
            else
            {
                listTimeSlotLunedi.Add(new TimeSlot() { DeleteEnabled = false, GiornoRiferimento = 1 });
                listTimeSlotMartedi.Add(new TimeSlot() { DeleteEnabled = false, GiornoRiferimento = 2 });
                listTimeSlotMercoledi.Add(new TimeSlot() { DeleteEnabled = false, GiornoRiferimento = 3 });
                listTimeSlotGiovedi.Add(new TimeSlot() { DeleteEnabled = false, GiornoRiferimento = 4 });
                listTimeSlotVenerdi.Add(new TimeSlot() { DeleteEnabled = false, GiornoRiferimento = 5 });
                listTimeSlotSabato.Add(new TimeSlot() { DeleteEnabled = false, GiornoRiferimento = 6 });
                listTimeSlotDomenica.Add(new TimeSlot() { DeleteEnabled = false, GiornoRiferimento = 7 });
            }

            listViewMonday.ItemsSource = listTimeSlotLunedi;
            listViewMonday.HeightRequest = listTimeSlotLunedi.Count * 60 ;

            listViewMartedi.ItemsSource = listTimeSlotMartedi;
            listViewMartedi.HeightRequest = listTimeSlotMartedi.Count *60;

            listViewMercoledi.ItemsSource = listTimeSlotMercoledi;
            listViewMercoledi.HeightRequest = listTimeSlotMartedi.Count * 60;

            listViewGiovedi.ItemsSource = listTimeSlotGiovedi;
            listViewGiovedi.HeightRequest = listTimeSlotGiovedi.Count * 60;

            listViewVenerdi.ItemsSource = listTimeSlotVenerdi;
            listViewVenerdi.HeightRequest = listTimeSlotVenerdi.Count * 60;

            listViewSabato.ItemsSource = listTimeSlotSabato;
            listViewSabato.HeightRequest = listTimeSlotSabato.Count * 60;

            listViewDomenica.ItemsSource = listTimeSlotDomenica;
            listViewDomenica.HeightRequest = listTimeSlotDomenica.Count * 60;
        }

        private void CreateTimeSlotInput(string FasceOrarieToModify, ref ObservableCollection<TimeSlot> ListToModify, int GiornoRiferimento)
        {
            if (!string.IsNullOrEmpty(FasceOrarieToModify))
            {
                var fasceorarie = FasceOrarieToModify.Split(';');

                for (int i = 0; i < fasceorarie.Length - 1; i++)
                {
                    if (!string.IsNullOrEmpty(fasceorarie[i]))
                    {
                        var orari = fasceorarie[i].Split('-');
                        //var dalleOre = new ObservableCollection<string>() { orari[0].Split(':')[0], orari[0].Split(':')[1] };
                        //var alleOre = new ObservableCollection<string>() { orari[1].Split(':')[0], orari[1].Split(':')[1] };

                        var dalleOre = orari[0];
                        var alleOre = orari[1];

                        ListToModify.Add(new TimeSlot() { Dalle = dalleOre, Alle =alleOre, DeleteEnabled = false, GiornoRiferimento = GiornoRiferimento });
                    }
                }

                switch (GiornoRiferimento)
                {
                    case 1:
                        Lunedi = true;
                        stkLunedi.IsVisible = true;
                        btn_ToggleLun.BackgroundColor = (Color)App.Current.Resources["PrimaryColor"];
                        break;
                    case 2:
                        Martedi = true;
                        stkMartedi.IsVisible = true;
                        btn_ToggleMar.BackgroundColor = (Color)App.Current.Resources["PrimaryColor"];
                        break;
                    case 3:
                        Mercoledi = true;
                        stkMercoledi.IsVisible = true;
                        btn_ToggleMer.BackgroundColor = (Color)App.Current.Resources["PrimaryColor"];
                        break;
                    case 4:
                        Giovedi = true;
                        stkGiovedi.IsVisible = true;
                        btn_ToggleGio.BackgroundColor = (Color)App.Current.Resources["PrimaryColor"];
                        break;
                    case 5:
                        Venerdi = true;
                        stkVenerdi.IsVisible = true;
                        btn_ToggleVen.BackgroundColor = (Color)App.Current.Resources["PrimaryColor"];
                        break;
                    case 6:
                        Sabato = true;
                        stkSabato.IsVisible = true;
                        btn_ToggleSab.BackgroundColor = (Color)App.Current.Resources["PrimaryColor"];
                        break;
                    case 7:
                        Domenica = true;
                        stkDomenica.IsVisible = true;
                        btn_ToggleDom.BackgroundColor = (Color)App.Current.Resources["PrimaryColor"];
                        break;
                }

            }
            else
            {
                ListToModify.Add(new TimeSlot() { DeleteEnabled = false, GiornoRiferimento = GiornoRiferimento });
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private async void ShowStackDay(object sender, EventArgs e)
        {
            Button  btn = (Button)sender;
            var item = int.Parse(btn.CommandParameter.ToString());

            bool isStackVisible = false;
            if (item == 1)
            {
                Lunedi = !Lunedi;
                stkLunedi.IsVisible = Lunedi;
                isStackVisible = Lunedi;
            }

            if (item == 2)
            {
                Martedi = !Martedi;
                stkMartedi.IsVisible = Martedi;
                isStackVisible = Martedi;
            }

            if (item == 3)
            {
                Mercoledi = !Mercoledi;
                stkMercoledi.IsVisible = Mercoledi;
                isStackVisible = Mercoledi;
            }

            if (item == 4)
            {
                Giovedi = !Giovedi;
                stkGiovedi.IsVisible = Giovedi;
                isStackVisible = Giovedi;

            }

            if (item == 5)
            {
                Venerdi = !Venerdi;
                stkVenerdi.IsVisible = Venerdi;
                isStackVisible = Venerdi;

            }

            if (item == 6)
            {
                Sabato = !Sabato;
                stkSabato.IsVisible = Sabato;
                isStackVisible = Sabato;

            }

            if (item == 7)
            {
                Domenica = !Domenica;
                stkDomenica.IsVisible = Domenica;
                isStackVisible = Domenica;
            }

            if (isStackVisible)
            {
                btn.BackgroundColor = (Color)App.Current.Resources["PrimaryColor"];
                btn.TextColor = Color.White;
            }
            else
            {
                var list = new List<TimeSlot>();
                list.Add(new TimeSlot() { DeleteEnabled = false, GiornoRiferimento = item });
                updateListView(item, list);
                btn.BackgroundColor = Color.White;
                btn.TextColor = (Color)App.Current.Resources["DarkColor"];
            }
        }
        

        private async void BtnBack_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PopAsync();
            }
            catch (Exception Ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }

        private async void BtnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void DeleteLunediElement(object sender, EventArgs e)
        {
            var item = ((Button)sender).CommandParameter as TimeSlot;
            listTimeSlotLunedi.Remove(item);
            listViewMonday.HeightRequest = listTimeSlotLunedi.Count * 60;
            List<TimeSlot> copy = listTimeSlotLunedi.ToList();
            listViewMonday.ItemsSource = copy;


        }
        private async void DeleteMartediElement(object sender, EventArgs e)
        {
            var item = ((Button)sender).CommandParameter as TimeSlot;
            listTimeSlotMartedi.Remove(item);
            listViewMartedi.HeightRequest = listTimeSlotMartedi.Count * 60;
            List<TimeSlot> copy = listTimeSlotMartedi.ToList();
            listViewMartedi.ItemsSource = copy;

        }
        private async void DeleteMercolediElement(object sender, EventArgs e)
        {
            var item = ((Button)sender).CommandParameter as TimeSlot;
            listTimeSlotMercoledi.Remove(item);
            listViewMercoledi.HeightRequest = listTimeSlotMercoledi.Count * 60;
            List<TimeSlot> copy = listTimeSlotMercoledi.ToList();
            listViewMercoledi.ItemsSource = copy;

        }
        private async void DeleteGiovediElement(object sender, EventArgs e)
        {
            var item = ((Button)sender).CommandParameter as TimeSlot;
            listTimeSlotGiovedi.Remove(item);
            listViewGiovedi.HeightRequest = listTimeSlotGiovedi.Count * 60;
            List<TimeSlot> copy = listTimeSlotGiovedi.ToList();
            listViewGiovedi.ItemsSource = copy;

        }
        private async void DeleteSabatoElement(object sender, EventArgs e)
        {
            var item = ((Button)sender).CommandParameter as TimeSlot;
            listTimeSlotSabato.Remove(item);
            listViewVenerdi.HeightRequest = listTimeSlotSabato.Count * 60;
            List<TimeSlot> copy = listTimeSlotSabato.ToList();
            listViewSabato.ItemsSource = copy;

        }
        private async void DeleteDomenicaElement(object sender, EventArgs e)
        {
            var item = ((Button)sender).CommandParameter as TimeSlot;
            listTimeSlotDomenica.Remove(item);
            listViewDomenica.HeightRequest = listTimeSlotDomenica.Count * 60;
            List<TimeSlot> copy = listTimeSlotDomenica.ToList();
            listViewDomenica.ItemsSource = copy;

        }
        private async void DeleteVenerdiElement(object sender, EventArgs e)
        {
            var item = ((Button)sender).CommandParameter as TimeSlot;
            listTimeSlotVenerdi.Remove(item);
            listViewVenerdi.HeightRequest = listTimeSlotVenerdi.Count * 60;
            List<TimeSlot> copy = listTimeSlotVenerdi.ToList();
            listViewVenerdi.ItemsSource = copy;

        }


        private async void BtnAddTimeSlot_Clicked(object sender, EventArgs e)
        {
            int id = int.Parse(((Button)sender).CommandParameter.ToString());

            switch (id)
            {
                case 1:
                    listTimeSlotLunedi.Add(new TimeSlot() { DeleteEnabled = true, GiornoRiferimento = 1 });
                    listViewMonday.HeightRequest = listTimeSlotLunedi.Count * 60;
                    List<TimeSlot> copyLun = listTimeSlotLunedi.ToList();
                    listViewMonday.ItemsSource = copyLun;
                    break;

                case 2:
                    listTimeSlotMartedi.Add(new TimeSlot() { DeleteEnabled = true, GiornoRiferimento = 2 });
                    listViewMartedi.HeightRequest = listTimeSlotMartedi.Count * 60;
                    List<TimeSlot> copyMar = listTimeSlotMartedi.ToList();
                    listViewMartedi.ItemsSource = copyMar;

                    break;
                case 3:
                    listTimeSlotMercoledi.Add(new TimeSlot() { DeleteEnabled = true, GiornoRiferimento = 3 });
                    listViewMercoledi.HeightRequest = listTimeSlotMercoledi.Count * 60;
                    List<TimeSlot> copyMer = listTimeSlotMercoledi.ToList();
                    listViewMercoledi.ItemsSource = copyMer;

                    break;
                case 4:
                    listTimeSlotGiovedi.Add(new TimeSlot() { DeleteEnabled = true, GiornoRiferimento = 4 });
                    listViewGiovedi.HeightRequest = listTimeSlotGiovedi.Count * 60;
                    List<TimeSlot> copyGio = listTimeSlotGiovedi.ToList();
                    listViewGiovedi.ItemsSource = copyGio;

                    break;
                case 5:
                    listTimeSlotVenerdi.Add(new TimeSlot() { DeleteEnabled = true, GiornoRiferimento = 5 });
                    listViewVenerdi.HeightRequest = listTimeSlotVenerdi.Count * 60;
                    List<TimeSlot> copyVen = listTimeSlotVenerdi.ToList();
                    listViewVenerdi.ItemsSource = copyVen;

                    break;
                case 6:
                    listTimeSlotSabato.Add(new TimeSlot() { DeleteEnabled = true, GiornoRiferimento = 6 });
                    listViewSabato.HeightRequest = listTimeSlotSabato.Count * 60;
                    List<TimeSlot> copySab = listTimeSlotSabato.ToList();
                    listViewSabato.ItemsSource = copySab;

                    break;
                case 7:
                    listTimeSlotDomenica.Add(new TimeSlot() { DeleteEnabled = true, GiornoRiferimento = 7 });
                    listViewDomenica.HeightRequest = listTimeSlotDomenica.Count * 60;
                    List<TimeSlot> copyDom = listTimeSlotDomenica.ToList();
                    listViewDomenica.ItemsSource = copyDom;

                    break;
                default:
                    break;
            }
        }

        //TimeSlot timeSlot;
        int posizioneTimeSlotCorrente;
        ObservableCollection<TimeSlot> listaRiferimentoCorrente;
        int GiornoCorrente;

        private async void PickerDalle_LblClicked(object sender, EventArgs e)
        {
            try
            {
                var timeSlotSelezionato = (TimeSlot)((TappedEventArgs)e).Parameter;
                var GiornoRiferimento = timeSlotSelezionato.GiornoRiferimento;
                int posizioneTimeSlot = 0;
                ObservableCollection<TimeSlot> listaRiferimento = null;
                GiornoCorrente = GiornoRiferimento;
                switch (GiornoRiferimento)
                {
                    case 1:
                        listaRiferimento = listTimeSlotLunedi;
                        posizioneTimeSlot = listTimeSlotLunedi.IndexOf(timeSlotSelezionato);
                        break;
                    case 2:
                        listaRiferimento = listTimeSlotMartedi;
                        posizioneTimeSlot = listTimeSlotMartedi.IndexOf(timeSlotSelezionato);
                        break;
                    case 3:
                        listaRiferimento = listTimeSlotMercoledi;
                        posizioneTimeSlot = listTimeSlotMercoledi.IndexOf(timeSlotSelezionato);
                        break;
                    case 4:
                        listaRiferimento = listTimeSlotGiovedi;
                        posizioneTimeSlot = listTimeSlotGiovedi.IndexOf(timeSlotSelezionato);
                        break;
                    case 5:
                        listaRiferimento = listTimeSlotVenerdi;
                        posizioneTimeSlot = listTimeSlotVenerdi.IndexOf(timeSlotSelezionato);
                        break;
                    case 6:
                        listaRiferimento = listTimeSlotSabato;
                        posizioneTimeSlot = listTimeSlotSabato.IndexOf(timeSlotSelezionato);
                        break;
                    case 7:
                        listaRiferimento = listTimeSlotDomenica;
                        posizioneTimeSlot = listTimeSlotDomenica.IndexOf(timeSlotSelezionato);
                        break;
                }
                posizioneTimeSlotCorrente = posizioneTimeSlot;
                listaRiferimentoCorrente = listaRiferimento;


                TimeSlotViewModel timeSlotViewModel = new TimeSlotViewModel();
                pickerDalle.ItemsSource = timeSlotViewModel.Time;
                pickerDalle.ColumnHeaderText = timeSlotViewModel.Header;
                pickerDalle.IsOpen = true;
                pickerDalle.IsVisible = true;
                stkPicker_Dalle.IsVisible = true;
            }
            catch (Exception ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }

        private void pickerDalle_SelectionChanged(object sender, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e){
            ObservableCollection<object> i = (ObservableCollection<object>) pickerDalle.SelectedItem;

            listaRiferimentoCorrente[posizioneTimeSlotCorrente].Dalle = "";
            foreach (object el in i)
            {
                if(listaRiferimentoCorrente[posizioneTimeSlotCorrente].Dalle != "")
                {
                    listaRiferimentoCorrente[posizioneTimeSlotCorrente].Dalle += ":";
                }
                listaRiferimentoCorrente[posizioneTimeSlotCorrente].Dalle += el.ToString();

            }

            List<TimeSlot> copy = listaRiferimentoCorrente.ToList();
            updateListView(GiornoCorrente, copy);
        }

        private void updateListView(int GiornoCorrente, List<TimeSlot> newList)
        {
            switch (GiornoCorrente)
            {
                case 1:
                    listTimeSlotLunedi = new ObservableCollection<TimeSlot>(newList); //serve solo per il metodo ShowStackDay
                    listViewMonday.ItemsSource = newList;
                    break;
                case 2:
                    listTimeSlotMartedi = new ObservableCollection<TimeSlot>(newList); //serve solo per il metodo ShowStackDay
                    listViewMartedi.ItemsSource = newList;
                    break;
                case 3:
                    listTimeSlotMercoledi = new ObservableCollection<TimeSlot>(newList); //serve solo per il metodo ShowStackDay
                    listViewMercoledi.ItemsSource = newList;
                    break;
                case 4:
                    listTimeSlotGiovedi = new ObservableCollection<TimeSlot>(newList); //serve solo per il metodo ShowStackDay
                    listViewGiovedi.ItemsSource = newList;
                    break;
                case 5:
                    listTimeSlotVenerdi = new ObservableCollection<TimeSlot>(newList); //serve solo per il metodo ShowStackDay
                    listViewVenerdi.ItemsSource = newList;
                    break;
                case 6:
                    listTimeSlotSabato = new ObservableCollection<TimeSlot>(newList); //serve solo per il metodo ShowStackDay
                    listViewSabato.ItemsSource = newList;
                    break;
                case 7:
                    listTimeSlotDomenica = new ObservableCollection<TimeSlot>(newList); //serve solo per il metodo ShowStackDay
                    listViewDomenica.ItemsSource = newList;
                    break;
            }
        }

        private async void PickerAlle_LblClicked(object sender, EventArgs e)
        {
            try
            {
                var timeSlotSelezionato = (TimeSlot)((TappedEventArgs)e).Parameter;
                var GiornoRiferimento = timeSlotSelezionato.GiornoRiferimento;
                int posizioneTimeSlot = 0;
                ObservableCollection<TimeSlot> listaRiferimento = null;
                GiornoCorrente = GiornoRiferimento;
                switch (GiornoRiferimento)
                {
                    case 1:
                        listaRiferimento = listTimeSlotLunedi;
                        posizioneTimeSlot = listTimeSlotLunedi.IndexOf(timeSlotSelezionato);
                        break;
                    case 2:
                        listaRiferimento = listTimeSlotMartedi;
                        posizioneTimeSlot = listTimeSlotMartedi.IndexOf(timeSlotSelezionato);
                        break;
                    case 3:
                        listaRiferimento = listTimeSlotMercoledi;
                        posizioneTimeSlot = listTimeSlotMercoledi.IndexOf(timeSlotSelezionato);
                        break;
                    case 4:
                        listaRiferimento = listTimeSlotGiovedi;
                        posizioneTimeSlot = listTimeSlotGiovedi.IndexOf(timeSlotSelezionato);
                        break;
                    case 5:
                        listaRiferimento = listTimeSlotVenerdi;
                        posizioneTimeSlot = listTimeSlotVenerdi.IndexOf(timeSlotSelezionato);
                        break;
                    case 6:
                        listaRiferimento = listTimeSlotSabato;
                        posizioneTimeSlot = listTimeSlotSabato.IndexOf(timeSlotSelezionato);
                        break;
                    case 7:
                        listaRiferimento = listTimeSlotDomenica;
                        posizioneTimeSlot = listTimeSlotDomenica.IndexOf(timeSlotSelezionato);
                        break;
                }
                posizioneTimeSlotCorrente = posizioneTimeSlot;
                listaRiferimentoCorrente = listaRiferimento;


                TimeSlotViewModel timeSlotViewModel = new TimeSlotViewModel();
                pickerAlle.ItemsSource = timeSlotViewModel.Time;
                pickerAlle.ColumnHeaderText = timeSlotViewModel.Header;
                pickerAlle.IsOpen = true;
                pickerAlle.IsVisible = true;
                stkPicker_Alle.IsVisible = true;
            }
            catch (Exception ex)
            {
                //Navigo alla pagina d'errore.
                await Navigation.PushAsync(new ErrorPage());
            }
        }

        private void pickerAlle_SelectionChanged(object sender, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        {
            ObservableCollection<object> i = (ObservableCollection<object>)pickerAlle.SelectedItem;

            listaRiferimentoCorrente[posizioneTimeSlotCorrente].Alle = "";
            foreach (object el in i)
            {
                if (listaRiferimentoCorrente[posizioneTimeSlotCorrente].Alle != "")
                {
                    listaRiferimentoCorrente[posizioneTimeSlotCorrente].Alle += ":";
                }
                listaRiferimentoCorrente[posizioneTimeSlotCorrente].Alle += el.ToString();

            }

            List<TimeSlot> copy = listaRiferimentoCorrente.ToList();
            updateListView(GiornoCorrente, copy);
        }


        private void Picker_Closed(object sender,EventArgs e)
        {
            stkPicker_Alle.IsVisible = false;
            stkPicker_Dalle.IsVisible = false;
            pickerAlle.IsVisible = false;
            pickerDalle.IsVisible = false;
        }

        private async void BtnProcedi_Clicked(object sender, EventArgs e)
        {
            var fasceOrarieLunedi = string.Empty;
            foreach (var item in listTimeSlotLunedi)
            {
                if (item.Dalle != null && item.Alle != null)
                {
                    fasceOrarieLunedi += $"{item.Dalle}-{item.Alle};";
                }
            }

            var fasceOrarieMartedi = string.Empty;
            foreach (var item in listTimeSlotMartedi)
            {
                if (item.Dalle != null && item.Alle != null)
                {
                    fasceOrarieMartedi += $"{item.Dalle}-{item.Alle};";
                }
            }

            var fasceOrarieMercoledi = string.Empty;
            foreach (var item in listTimeSlotMercoledi)
            {
                if (item.Dalle != null && item.Alle != null)
                {
                    fasceOrarieMercoledi += $"{item.Dalle}-{item.Alle};";
                }
            }

            var fasceOrarieGiovedi = string.Empty;
            foreach (var item in listTimeSlotGiovedi)
            {
                if (item.Dalle != null && item.Alle != null)
                {
                    fasceOrarieGiovedi += $"{item.Dalle}-{item.Alle};";
                }
            }

            var fasceOrarieVenerdi = string.Empty;
            foreach (var item in listTimeSlotVenerdi)
            {
                if (item.Dalle != null && item.Alle != null)
                {
                    fasceOrarieVenerdi += $"{item.Dalle}-{item.Alle};";
                }
            }

            var fasceOrarieSabato = string.Empty;
            foreach (var item in listTimeSlotSabato)
            {
                if (item.Dalle != null && item.Alle != null)
                {
                    fasceOrarieSabato += $"{item.Dalle}-{item.Alle};";
                }
            }

            var fasceOrarieDomenica = string.Empty;
            foreach (var item in listTimeSlotDomenica)
            {
                if (item.Dalle != null && item.Alle != null)
                {
                    fasceOrarieDomenica += $"{item.Dalle}-{item.Alle};";
                }
            }

            if (annuncio.DisponibilitaOraria == null)
            {
                annuncio.DisponibilitaOraria = new DisponibilitaOrariaDto();
            }
            annuncio.DisponibilitaOraria.FasceOrarieLunedi = fasceOrarieLunedi;
            annuncio.DisponibilitaOraria.FasceOrarieMartedi = fasceOrarieMartedi;
            annuncio.DisponibilitaOraria.FasceOrarieMercoledi = fasceOrarieMercoledi;
            annuncio.DisponibilitaOraria.FasceOrarieGiovedi = fasceOrarieGiovedi;
            annuncio.DisponibilitaOraria.FasceOrarieVenerdi = fasceOrarieVenerdi;
            annuncio.DisponibilitaOraria.FasceOrarieSabato = fasceOrarieSabato;
            annuncio.DisponibilitaOraria.FasceOrarieDomenica = fasceOrarieDomenica;

            //TODO TRY CATCH
            try { 
                AnnunciClient annunciClient = new AnnunciClient(await ApiHelper.GetApiClient());
                if (dtoToModify == null) // caso inserimento
                {
                    await annunciClient.InsertAnnuncioAsync(annuncio);
                }
                else if (dtoToModify.Item.Id != null)
                {
                    await annunciClient.UpdateAnnuncioAsync((Guid)dtoToModify.Item.Id, annuncio);
                }

                MessagingCenter.Send(this, "AnnuncioCreato", "Ok");
                await Navigation.PopModalAsync();
            }
            catch
            {
                await Navigation.PushAsync(new ErrorPage());
            }
        }
    }
}