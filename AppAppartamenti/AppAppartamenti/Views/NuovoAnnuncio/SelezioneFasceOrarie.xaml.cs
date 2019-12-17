﻿using System;
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
        public TimeSpan Dalle { get; set; }
        public TimeSpan Alle { get; set; }
        public bool DeleteEnabled { get; set; }
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

            listTimeSlotLunedi.Add(new TimeSlot() { DeleteEnabled=false});
            listViewMonday.ItemsSource = listTimeSlotLunedi;
            listViewMonday.HeightRequest = 60;

            listTimeSlotMartedi.Add(new TimeSlot() { DeleteEnabled = false });
            listViewMartedi.ItemsSource = listTimeSlotMartedi;
            listViewMartedi.HeightRequest = 60;


            listTimeSlotMercoledi.Add(new TimeSlot() { DeleteEnabled = false });
            listViewMercoledi.ItemsSource = listTimeSlotMercoledi;
            listViewMercoledi.HeightRequest = 60;


            listTimeSlotGiovedi.Add(new TimeSlot() { DeleteEnabled = false });
            listViewGiovedi.ItemsSource = listTimeSlotGiovedi;
            listViewGiovedi.HeightRequest = 60;


            listTimeSlotVenerdi.Add(new TimeSlot() { DeleteEnabled = false });
            listViewVenerdi.ItemsSource = listTimeSlotVenerdi;
            listViewVenerdi.HeightRequest = 60;


            listTimeSlotSabato.Add(new TimeSlot() { DeleteEnabled = false });
            listViewSabato.ItemsSource = listTimeSlotSabato;
            listViewSabato.HeightRequest = 60;


            listTimeSlotDomenica.Add(new TimeSlot() { DeleteEnabled = false });
            listViewDomenica.ItemsSource = listTimeSlotDomenica;
            listViewDomenica.HeightRequest = 60;


            annuncio = Annuncio;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //if (edtDescrizione.Text == null && dtoToModify != null && dtoToModify.Item != null)
            //{
            //    edtDescrizione.Text = dtoToModify.Item.Descrizione;
            //}
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
                btn.BackgroundColor = (Color)App.Current.Resources["LightColor"];
            }
            else
            {
                btn.BackgroundColor = Color.White;

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


        }
        private async void DeleteMartediElement(object sender, EventArgs e)
        {
            var item = ((Button)sender).CommandParameter as TimeSlot;
            listTimeSlotMartedi.Remove(item);
            listViewMartedi.HeightRequest = listTimeSlotMartedi.Count * 60;

        }
        private async void DeleteMercolediElement(object sender, EventArgs e)
        {
            var item = ((Button)sender).CommandParameter as TimeSlot;
            listTimeSlotMercoledi.Remove(item);
            listViewMercoledi.HeightRequest = listTimeSlotMercoledi.Count * 60;

        }
        private async void DeleteGiovediElement(object sender, EventArgs e)
        {
            var item = ((Button)sender).CommandParameter as TimeSlot;
            listTimeSlotGiovedi.Remove(item);
            listViewGiovedi.HeightRequest = listTimeSlotGiovedi.Count * 60;

        }
        private async void DeleteSabatoElement(object sender, EventArgs e)
        {
            var item = ((Button)sender).CommandParameter as TimeSlot;
            listTimeSlotSabato.Remove(item);
            listViewVenerdi.HeightRequest = listTimeSlotSabato.Count * 60;

        }
        private async void DeleteDomenicaElement(object sender, EventArgs e)
        {
            var item = ((Button)sender).CommandParameter as TimeSlot;
            listTimeSlotDomenica.Remove(item);
            listViewDomenica.HeightRequest = listTimeSlotDomenica.Count * 60;

        }
        private async void DeleteVenerdiElement(object sender, EventArgs e)
        {
            var item = ((Button)sender).CommandParameter as TimeSlot;
            listTimeSlotVenerdi.Remove(item);
            listViewVenerdi.HeightRequest = listTimeSlotVenerdi.Count * 60;

        }


        private async void BtnAddTimeSlot_Clicked(object sender, EventArgs e)
        {
            int id = int.Parse(((Button)sender).CommandParameter.ToString());

            switch (id)
            {
                case 1:
                    listTimeSlotLunedi.Add(new TimeSlot() { DeleteEnabled = true });
                    listViewMonday.HeightRequest = listTimeSlotLunedi.Count * 60;

                    break;

                case 2:
                    listTimeSlotMartedi.Add(new TimeSlot() { DeleteEnabled = true });
                    listViewMartedi.HeightRequest = listTimeSlotMartedi.Count * 60;

                    break;
                case 3:
                    listTimeSlotMercoledi.Add(new TimeSlot() { DeleteEnabled = true });
                    listViewMercoledi.HeightRequest = listTimeSlotMercoledi.Count * 60;

                    break;
                case 4:
                    listTimeSlotGiovedi.Add(new TimeSlot() { DeleteEnabled = true });
                    listViewGiovedi.HeightRequest = listTimeSlotGiovedi.Count * 60;

                    break;
                case 5:
                    listTimeSlotVenerdi.Add(new TimeSlot() { DeleteEnabled = true });
                    listViewVenerdi.HeightRequest = listTimeSlotVenerdi.Count * 60;

                    break;
                case 6:
                    listTimeSlotSabato.Add(new TimeSlot() { DeleteEnabled = true });
                    listViewVenerdi.HeightRequest = listTimeSlotSabato.Count * 60;

                    break;
                case 7:
                    listTimeSlotDomenica.Add(new TimeSlot() { DeleteEnabled = true });
                    listViewDomenica.HeightRequest = listTimeSlotDomenica.Count * 60;

                    break;
                default:
                    break;
            }
        }

        private async void BtnProcedi_Clicked(object sender, EventArgs e)
        {
            var fasceOrarieLunedi = string.Empty;
            foreach (var item in listTimeSlotLunedi)
            {
                fasceOrarieLunedi = $"{item.Dalle.Hours.ToString()}:{item.Dalle.Minutes.ToString()}-{item.Alle.Hours.ToString()}:{item.Alle.Minutes.ToString()};";
            }

            var fasceOrarieMartedi = string.Empty;
            foreach (var item in listTimeSlotMartedi)
            {
                fasceOrarieMartedi = $"{item.Dalle.Hours.ToString()}:{item.Dalle.Minutes.ToString()}-{item.Alle.Hours.ToString()}:{item.Alle.Minutes.ToString()};";
            }

            var fasceOrarieMercoledi = string.Empty;
            foreach (var item in listTimeSlotMercoledi)
            {
                fasceOrarieMercoledi = $"{item.Dalle.Hours.ToString()}:{item.Dalle.Minutes.ToString()}-{item.Alle.Hours.ToString()}:{item.Alle.Minutes.ToString()};";
            }

            var fasceOrarieGiovedi = string.Empty;
            foreach (var item in listTimeSlotGiovedi)
            {
                fasceOrarieGiovedi = $"{item.Dalle.Hours.ToString()}:{item.Dalle.Minutes.ToString()}-{item.Alle.Hours.ToString()}:{item.Alle.Minutes.ToString()};";
            }

            var fasceOrarieVenerdi = string.Empty;
            foreach (var item in listTimeSlotVenerdi)
            {
                fasceOrarieVenerdi = $"{item.Dalle.Hours.ToString()}:{item.Dalle.Minutes.ToString()}-{item.Alle.Hours.ToString()}:{item.Alle.Minutes.ToString()};";
            }

            var fasceOrarieSabato = string.Empty;
            foreach (var item in listTimeSlotSabato)
            {
                fasceOrarieSabato = $"{item.Dalle.Hours.ToString()}:{item.Dalle.Minutes.ToString()}-{item.Alle.Hours.ToString()}:{item.Alle.Minutes.ToString()};";
            }

            var fasceOrarieDomenica = string.Empty;
            foreach (var item in listTimeSlotDomenica)
            {
                fasceOrarieDomenica = $"{item.Dalle.Hours.ToString()}:{item.Dalle.Minutes.ToString()}-{item.Alle.Hours.ToString()}:{item.Alle.Minutes.ToString()};";
            }

            annuncio.DisponibilitaOraria.FasceOrarieLunedi = fasceOrarieLunedi;
            annuncio.DisponibilitaOraria.FasceOrarieMartedi = fasceOrarieMartedi;
            annuncio.DisponibilitaOraria.FasceOrarieMercoledi = fasceOrarieMercoledi;
            annuncio.DisponibilitaOraria.FasceOrarieGiovedi = fasceOrarieGiovedi;
            annuncio.DisponibilitaOraria.FasceOrarieVenerdi = fasceOrarieVenerdi;
            annuncio.DisponibilitaOraria.FasceOrarieSabato = fasceOrarieSabato;
            annuncio.DisponibilitaOraria.FasceOrarieDomenica = fasceOrarieDomenica;

            //TODO TRY CATCH
            AnnunciClient annunciClient = new AnnunciClient(ApiHelper.GetApiClient());
            if (dtoToModify == null) // caso inserimento
            {
                await annunciClient.InsertAnnuncioAsync(annuncio);
            } else if(dtoToModify.Item.Id != null)
            {
                await annunciClient.UpdateAnnuncioAsync((Guid)dtoToModify.Item.Id, annuncio);
            }

            await Navigation.PopModalAsync();
        }
    }
}