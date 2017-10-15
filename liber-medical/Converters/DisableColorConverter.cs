using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Globalization;


namespace libermedical.Converters
{
	public class DisableColorConverter : IValueConverter, IMarkupExtension
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((bool)value)
			{
				return "#007aff";
			}
			else
			{
				return "#c8c7cc";
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
