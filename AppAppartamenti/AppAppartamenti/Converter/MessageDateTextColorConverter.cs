using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace AppAppartamenti.Converter
{
    public class MsgDateTextColorConverter : IValueConverter { 
        
        public object Convert (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return (Color)App.Current.Resources["LightColor"];

            if ((bool)value)
            {
                return (Color)App.Current.Resources["LightColor"];
            }
            else
            {
                return (Color)App.Current.Resources["GrayColor"];
            }
        }
        public object ConvertBack (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}
