using System;
using System.Collections.Generic;
using System.Text;
using Plugin.InputKit.Shared.Controls;
using Xamarin.Forms;

namespace AppAppartamenti
{
    public class MyEntry : Entry
    {
    }


    public class CustomEntry : AdvancedEntry
    {

        public CustomEntry()
        {
            this.BackgroundColor = Color.White;
            this.PlaceholderColor = Color.FromRgb(152,152,152);
            this.BorderColor = Color.FromRgb(220, 220, 220);
            this.TextColor = Color.Black;
            this.CornerRadius = 5;
        }
    }
}
