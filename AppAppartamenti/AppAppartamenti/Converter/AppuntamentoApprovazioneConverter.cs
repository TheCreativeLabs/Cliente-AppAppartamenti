using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AppAppartamentiApiClient;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace AppAppartamenti.Converter
{
    public class AppuntamentoApprovazioneConverter : IValueConverter { 
        
        public object Convert (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return false;

            var val = (AppuntamentoDettaglioDtoOutput)value;
            if (!val.FromMe.Value && !val.Confermato.Value)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
        public object ConvertBack (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}
