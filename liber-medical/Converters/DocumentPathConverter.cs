using libermedical.Helpers;
using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace libermedical.Converters
{
    public class DocumentPathConverter : IValueConverter, IMarkupExtension
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value ==null || string.IsNullOrEmpty(value.ToString()))
                return string.Empty;
            if (value.ToString().StartsWith("Ordonnance/") || value.ToString().StartsWith("PatientDocuments/"))
            {
                var path = $"{Constants.RestUrl}file?path={System.Net.WebUtility.UrlEncode(value.ToString())}&token={Settings.Token}";
                return path;
            }
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
