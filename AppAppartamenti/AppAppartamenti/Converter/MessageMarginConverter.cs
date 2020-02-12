using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace AppAppartamenti.Converter
{
    public class MessageMarginConverter : IValueConverter { 
        
        public object Convert (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return new Thickness(40, 10, 10, 10);

            if ((bool)value)
            {
                return new Thickness(40, 10, 10, 10);
            }
            else
            {
                return new Thickness(10, 10, 40, 10);
            }
        }
        public object ConvertBack (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}
