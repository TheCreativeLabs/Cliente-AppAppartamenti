using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace AppAppartamenti.Converter
{
    public class MessageListBadge : IValueConverter { 
        
        public object Convert (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return false;

            return (bool)value;
        }
        public object ConvertBack (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}
