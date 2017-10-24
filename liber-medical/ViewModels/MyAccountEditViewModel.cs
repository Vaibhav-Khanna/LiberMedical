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
		public User UserInfo;
		private readonly IUserDialogs _dialogs;

		public MyAccountEditViewModel(IUserDialogs dialogs)
		{
			_dialogs = dialogs;
			CurrentUser = JsonConvert.DeserializeObject<User>(Settings.CurrentUser);
		}


		public ICommand SaveCommand => new Command(async () => await SaveProfile());

		private User _currentUser;
		public User CurrentUser
		{
			get { return _currentUser; }
			set
			{
				_currentUser = value;
				RaisePropertyChanged();
			}
		}

		public string OldPassword { get; set; } = string.Empty;
		public string NewPassword { get; set; } = string.Empty;
		public string ConfirmPassword { get; set; } = string.Empty;

		private async Task SaveProfile()
		{
			if (!EmailValidator.EmailIsValid(CurrentUser.Email))
			{
                await CoreMethods.DisplayAlert("Email", "Veuillez entrer une adresse email valide", "OK");
				return;
			}

            using (_dialogs.Loading("Enregistrement..."))
			{
				Settings.CurrentUser = JsonConvert.SerializeObject(CurrentUser);
				MessagingCenter.Send(this, "ProfileUpdate");
				var passwd = await ChangePassword();
				if (App.IsConnected())
					await App.UserManager.SaveOrUpdateAsync(CurrentUser.Id, CurrentUser, false);
				if (passwd)
					NavBackCommand.Execute(null);

			}
		}


		private async Task<bool> ChangePassword()
		{
			if (string.IsNullOrEmpty(NewPassword + OldPassword + ConfirmPassword)) return true;
			if (NewPassword.Length < 6)
			{
                await CoreMethods.DisplayAlert("Mot de passe incorrect", "Le mot de passe doit contenir un minimum de 6 caractères", "OK");
				return false;
			}
			if (NewPassword != ConfirmPassword)
			{
                await CoreMethods.DisplayAlert("Mot de passe incorrect", "Les mots de passes doivent êtres identiques", "OK");
				return false;
			}

			//TODO: Change password through API
			await Task.Delay(2000);
			return true;
		}
	}
}