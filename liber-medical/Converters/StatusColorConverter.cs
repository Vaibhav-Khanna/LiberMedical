using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace libermedical.Converters
{
	public class StatusColorConverter : IValueConverter, IMarkupExtension
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var status = value.ToString();

           

            if (status == "En attente" || status == "En attente de validation")
                return "#f6a623";
            else if (status == "Traité" || status == "Validé")
                return "#77D42A";
			else if (status == "Refusé")
				return "#ff7586";         
            else if (status == "En attente de télétransmission")
            {
                return "#efd032";
            }
            else if (status == "Télétransmise")
            {
                return "#77D42A";
            }           
            else
                return "#f6a623";
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