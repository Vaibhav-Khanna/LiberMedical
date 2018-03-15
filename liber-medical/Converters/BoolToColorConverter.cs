using System;
using System.Globalization;
using Xamarin.Forms;

namespace libermedical.Converters
{
    public class BoolToColorConverter: IValueConverter
    {
       
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return (Color)App.Current.Resources["HeaderFooterBackgroundColor"];
            }
            else
            {
                return Color.White;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

     
    }
}
