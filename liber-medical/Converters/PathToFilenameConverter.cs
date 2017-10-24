using System;
using System.Globalization;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace libermedical.Converters
{
    public class PathToFilenameConverter : IValueConverter, IMarkupExtension
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(value.ToString()))
                return string.Empty;
            return Path.GetFileName(value.ToString());
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
