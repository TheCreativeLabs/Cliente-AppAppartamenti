using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace AppAppartamenti.ContentViews
{
    public partial class ChatInputBarView : ContentView
    {
        public static readonly BindableProperty CardTitleProperty = BindableProperty.Create(
            "CardTitle",        // the name of the bindable property
            typeof(string),     // the bindable property type
            typeof(ChatInputBarView),   // the parent object type
            string.Empty);

        public string CardTitle
        {
            get => (string)GetValue(ChatInputBarView.CardTitleProperty);
            set => SetValue(ChatInputBarView.CardTitleProperty, value);
        }

        public ChatInputBarView()
        {
            InitializeComponent();

            
        }

        private async void Send_Clicked(object sender, EventArgs e)
        {
            
        }
    }
}
