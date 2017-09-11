using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using libermedical.Helpers;
using libermedical.Models;
using libermedical.Utility;
using libermedical.ViewModels.Base;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace libermedical.ViewModels
{
	public class MyAccountEditViewModel : ViewModelBase
	{
		private readonly IUserDialogs _dialogs;

		public MyAccountEditViewModel(IUserDialogs dialogs)
		{
			_dialogs = dialogs;
			CurrentUser = JsonConvert.DeserializeObject<User>(Settings.CurrentUser);
		}

		public ICommand SaveCommand => new Command(async () => await SaveProfile());


		public User CurrentUser { get; set; }

		public string OldPassword { get; set; } = string.Empty;
		public string NewPassword { get; set; } = string.Empty;
		public string ConfirmPassword { get; set; } = string.Empty;

		private async Task SaveProfile()
		{
			if (!EmailValidator.EmailIsValid(CurrentUser.Email))
			{
				await CoreMethods.DisplayAlert("Email", "Please use a valid Email address", "OK");
				return;
			}

			using (_dialogs.Loading("Saving..."))
			{
				Settings.CurrentUser = JsonConvert.SerializeObject(CurrentUser);
				MessagingCenter.Send(this, "ProfileUpdate");
				var passwd = await ChangePassword();
				if(passwd)
					NavBackCommand.Execute(null);
			}
		}

		private async Task<bool> ChangePassword()
		{
			if (string.IsNullOrEmpty(NewPassword + OldPassword + ConfirmPassword)) return true;
			if (NewPassword.Length < 6)
			{
				await CoreMethods.DisplayAlert("Wrong password","Password should be at least 6 characters long", "OK");
				return false;
			}
			if (NewPassword != ConfirmPassword)
			{
				await CoreMethods.DisplayAlert("Wrong password", "Entered passwords don't match", "OK");
				return false;
			}
			//TODO: Change password through API
			await Task.Delay(2000);
			return true;
		}
	}
}