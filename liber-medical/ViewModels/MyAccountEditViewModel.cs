using System.Threading.Tasks;
using System.Windows.Input;
using libermedical.Helpers;
using libermedical.Models;
using libermedical.ViewModels.Base;
using Newtonsoft.Json;
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
			await Task.Delay(2000);
			IsLoading = false;
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

	public class Profile:BaseDTO
	{
		public string FirstName { get; set; } 
		public string LastName{ get; set; } 
		public string PhoneNumber{ get; set; } 
		public string EmailAddress{ get; set; }

		[JsonIgnore]
		public string FullName => $"{FirstName} {LastName}";

		public override string ToString()
		{
			return FullName;
		}
	}
}
