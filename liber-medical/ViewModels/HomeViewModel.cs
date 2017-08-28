using System.Windows.Input;
using libermedical.ViewModels.Base;
using Xamarin.Forms;

namespace libermedical.ViewModels
{
	public class HomeViewModel : ViewModelBase
	{
		public HomeViewModel()
		{

		}
		public ICommand AssistCommand => new Command(async () =>
		{
			var action = await CoreMethods.DisplayActionSheet(null, "Annuler", null, "Appel vocal", "E-mail", "SMS");
			switch (action)
			{
				case "Appel vocal":
					Device.OpenUri(new System.Uri("tel:+33559311824"));
					break;

				case "E-mail":
					Device.OpenUri(new System.Uri("mailto:contact@libermedical.com"));
					break;

				case "SMS":
					Device.OpenUri(new System.Uri("sms:+33559311824"));
					break;
			}

		});
	}


}

