using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Globalization;

namespace libermedical.Converters
{
	public class QuotationColorConverter : IValueConverter, IMarkupExtension
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((bool)value)
			{
				return (Color)Application.Current.Resources["HeaderFooterBackgroundColor"];
			}
			else
			{
				return (Color)Application.Current.Resources["LightGrey"];
			}
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
