using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace AppAzureSQL.Converters
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return "broken1.png";
            }
            //value = new ImageService().ConvertImageFromBase64ToImageSource(value.ToString());
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
