using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace AppAppartamenti.Converter
{
    public class PrezzoConverter : IValueConverter { 
        
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) { 
            string retSource = string.Empty;

            if (value != null) {

                retSource = $"\uf153 {value.ToString()}";

            } 
            
            return retSource; 
        } 
        
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException(); 
        } 
    }
}
