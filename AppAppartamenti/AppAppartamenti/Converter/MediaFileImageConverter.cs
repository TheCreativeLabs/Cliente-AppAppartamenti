using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace AppAppartamenti.Converter
{
    public class MediaFileImageSourceConverter : IValueConverter { 
        
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) { 
            ImageSource retSource = null;

            if (value != null) {
                retSource = ImageSource.FromStream(() => ((MediaFile)value).GetStream());
            } 
            
            return retSource; 
        } 
        
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException(); 
        } 
    }
}
