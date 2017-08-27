using System.Windows.Input;
using libermedical.ViewModels.Base;
using Xamarin.Forms;

namespace libermedical.ViewModels
{
	public class PlusViewModel: ViewModelBase
	{
		public ICommand GoToProfileCommand => new Command(() => CoreMethods.PushPageModel<MyAccountViewModel>(null, true));

		public ICommand ConnectCommand => new Command(() =>
		{
			var action = CoreMethods.DisplayActionSheet("Contacter mon conseiller via:", "Annuler", null, "Appel vocal", "SMS");
		});
	}
}
