using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace AppAppartamenti.Converter
{
    public class ByteArrayToImageSourceConverter : IValueConverter { 
        
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) { 
            ImageSource retSource = null;
            if (value != null) {
                try
                {
                    byte[] imageAsBytes = (byte[])value;
                    retSource = ImageSource.FromStream(() => new MemoryStream(imageAsBytes));
                }
                catch(Exception ex)
                {
                    
                }
            } 
            
            return retSource; 
        } 
        
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException(); 
        } 
    }
}
