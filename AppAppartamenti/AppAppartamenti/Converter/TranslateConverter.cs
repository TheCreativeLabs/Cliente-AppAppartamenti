﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace AppAppartamenti.Converter
{
    public class KeyToTranslateConverter : IValueConverter
    {

        static Helpers.TranslateExtension translate = new Helpers.TranslateExtension();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            String retSource = null; if (value != null)
            {
                string valueAsString = (string)value;
                retSource = Helpers.TranslateExtension.ResMgr.Value.GetString(valueAsString, translate.ci);
            }

            return retSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
