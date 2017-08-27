using System.Threading.Tasks;
using System.Windows.Input;
using libermedical.Helpers;
using libermedical.Models;
using libermedical.ViewModels.Base;
using Xamarin.Forms;

namespace libermedical.ViewModels
{
	public class MyAccountEditViewModel: ViewModelBase
	{
		public ICommand SaveCommand => new Command(async () => await SaveProfile());

		private async Task SaveProfile()
		{
			IsLoading = true;
			Settings.CurrentUser = CurrentUser;
			MessagingCenter.Send(this, "ProfileUpdate");
			NavBackCommand.Execute(null);
		}

		public bool IsLoading { get; set; }

		public Profile CurrentUser { get; set; }

		public MyAccountEditViewModel()
		{
			CurrentUser = Settings.CurrentUser;
		}
	}

}
	namespace libermedical.Models
{
}
