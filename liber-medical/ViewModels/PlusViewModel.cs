using System.Windows.Input;
using libermedical.ViewModels.Base;
using Xamarin.Forms;

namespace libermedical.ViewModels
{
	public class PlusViewModel: ViewModelBase
	{
		public ICommand GoToProfileCommand => new Command(async () => 
			await CoreMethods.PushPageModel<MyAccountViewModel>(null, true));

		public ICommand ConnectCommand => new Command(async () =>
		{
			var action = await CoreMethods.DisplayActionSheet("Contacter mon conseiller via:", "Annuler", null, "Appel vocal", "SMS");
            switch (action)
            {
                case "Appel vocal":
                    Device.OpenUri(new System.Uri("tel:+33559311824"));
                    break;
                case "SMS":
                    Device.OpenUri(new System.Uri("sms:+33559311824"));
                    break;
            }
        });
	}
}
