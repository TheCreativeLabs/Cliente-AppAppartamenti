using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Plugin.InputKit.Shared.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace AppAppartamenti
{
 
    public class DatePickerCtrl : DatePicker
    {
        public static readonly BindableProperty EnterTextProperty = BindableProperty.Create(propertyName: "Placeholder", returnType: typeof(string), declaringType: typeof(DatePickerCtrl), defaultValue: default(string));
        public string Placeholder
        {
            get;
            set;
        }

        public static readonly BindableProperty PlaceholderVisibleTextProperty = BindableProperty.Create(propertyName: "PlaceholderVisible", returnType: typeof(bool), declaringType: typeof(DatePickerCtrl), defaultValue: false);
        public bool PlaceholderHidden
        {
            get;
            set;
        }
    }

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

    public class SearchEntry : Entry
    {
    }

    public class ShadowFrame : Frame
    {
    }

    public class WebViewUserAgent : WebView
    {
    }

    public class CustomContentView : ContentView
    {
    }

    public class CustomEditor : Editor
    {
    }

    public class ChatEntry : Editor
    {
    }

    public class CustomMap : Map
    {
        public List<CustomPin> CustomPins { get; set; }
    }

    public class CustomPin : Pin
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public bool IsSelected { get; set; }
    }

    public class TimeSlotLabel : Label
    {
        public static readonly BindableProperty EnterTextProperty = BindableProperty.Create(propertyName: "Placeholder", returnType: typeof(string), declaringType: typeof(TimeSlotLabel), defaultValue: default(string));
        public string Placeholder
        {
            get;
            set;
        }
    }
}
