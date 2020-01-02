using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace AppAppartamenti.ContentViews
{
    public class MenuItem
    {
        public int Id { get; set; }
        public Page RedirectPage { get; set; }
        public string DisplayName { get; set; }
        public string Icona { get; set;}
        public Color BackgroundColor { get; set; }
        public Color BorderColor { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsVisible { get; set; }
    }

    partial class StepMenu : ContentView
    {

        public ObservableCollection<MenuItem> items = new ObservableCollection<MenuItem>();


        public static readonly BindableProperty CurrentIdProperty = BindableProperty.Create(
        "CurrentId",        
        typeof(int),     
        typeof(StepMenu),   
        0,
        BindingMode.Default,
        propertyChanged: DescriptionTextChangedAsync
        );     

        public int CurrentId
        {
            get => (int)GetValue(StepMenu.CurrentIdProperty);
            set => SetValue(StepMenu.CurrentIdProperty, value);
        }

        private static async void DescriptionTextChangedAsync(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (StepMenu)bindable;

            control.items.Add(new MenuItem() { Id = 1, DisplayName = "Tipologia proprietà", Icona = null, RedirectPage = null });
            control.items.Add(new MenuItem() { Id = 2, DisplayName = "Tipologia annuncio", Icona = null, RedirectPage = null });
            control.items.Add(new MenuItem() { Id = 3, DisplayName = "Posizione", Icona = null, RedirectPage = null });
            control.items.Add(new MenuItem() { Id = 4, DisplayName = "Informazioni generali", Icona = null, RedirectPage = null });
            control.items.Add(new MenuItem() { Id = 5, DisplayName = "Prezzo", Icona = null, RedirectPage = null });
            control.items.Add(new MenuItem() { Id = 6, DisplayName = "Immagini", Icona = null, RedirectPage = null });
            control.items.Add(new MenuItem() { Id = 7, DisplayName = "Planimetria", Icona = null, RedirectPage = null });
            //items.Add(new MenuItem() { Id = 8, DisplayName = "Video", Icona = null, RedirectPage = null });
            control.items.Add(new MenuItem() { Id = 9, DisplayName = "Descrizione", Icona = null, RedirectPage = null });
            control.items.Add(new MenuItem() { Id = 10, DisplayName = "Fasce orarie", Icona = null, RedirectPage = null });

            foreach (var item in control.items)
            {
                item.IsVisible = true;

                if (item.Id < (int)newValue)
                {
                    item.BackgroundColor = (Color)App.Current.Resources["LightColor"];
                    item.BorderColor = (Color)App.Current.Resources["LightColor"];
                    item.IsCompleted = true;
                    item.IsVisible = false;

                }
                else if (item.Id == (int)newValue)
                {
                    item.BackgroundColor = Color.White;
                    item.BorderColor = (Color)App.Current.Resources["PrimaryColor"];
                }
            }

            control.lvStepMenu.ItemsSource = control.items;
            control.CurrentId =(int)newValue;
        }

        public StepMenu()
        {
            InitializeComponent();
        }
    }
}
