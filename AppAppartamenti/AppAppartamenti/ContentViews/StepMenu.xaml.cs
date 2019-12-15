using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace AppAppartamenti.ContentViews
{
    public class MenuItem
    {
        public Page RedirectPage { get; set; }
        public string DisplayName { get; set; }
        public string Icona { get; set; }
    }

    partial class StepMenu : ContentView
    {
        public static readonly BindableProperty CardTitleProperty = BindableProperty.Create(
        "CardTitle",        // the name of the bindable property
        typeof(string),     // the bindable property type
        typeof(StepMenu),   // the parent object type
        string.Empty);      // the default value for the property


        public string CardTitle
        {
            get => (string)GetValue(StepMenu.CardTitleProperty);
            set => SetValue(StepMenu.CardTitleProperty, value);
        }

        public StepMenu()
        {
            InitializeComponent();

            ObservableCollection<MenuItem> items = new ObservableCollection<MenuItem>();
            items.Add(new MenuItem() { DisplayName = "Tipologia proprietà", Icona = null, RedirectPage = null });
            items.Add(new MenuItem() { DisplayName = "Tipoogia annuncio", Icona = null, RedirectPage = null });
            items.Add(new MenuItem() { DisplayName = "Posizione", Icona = null, RedirectPage = null });
            items.Add(new MenuItem() { DisplayName = "Prezzo", Icona = null, RedirectPage = null });
            items.Add(new MenuItem() { DisplayName = "Informazioni generali", Icona = null, RedirectPage = null });
            items.Add(new MenuItem() { DisplayName = "Immagini", Icona = null, RedirectPage = null });
            items.Add(new MenuItem() { DisplayName = "Planimetria", Icona = null, RedirectPage = null });
            items.Add(new MenuItem() { DisplayName = "Video", Icona = null, RedirectPage = null });
            items.Add(new MenuItem() { DisplayName = "Descrizione", Icona = null, RedirectPage = null });

            lvStepMenu.ItemsSource = items;
        }
    }
}
