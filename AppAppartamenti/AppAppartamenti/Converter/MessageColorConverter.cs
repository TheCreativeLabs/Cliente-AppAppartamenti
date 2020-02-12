using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace AppAppartamenti.Converter
{
    public class MessageColorConverter : IValueConverter { 
        
        public object Convert (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return (Style)App.Current.Resources["MessageFromMe"];

            if ((bool)value)
            {
                return (Style)App.Current.Resources["MessageFromMe"];
            }
            else
            {
                return (Style)App.Current.Resources["MessageToMe"];
            }
        }
        public object ConvertBack (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}
