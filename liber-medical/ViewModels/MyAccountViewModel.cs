using System;
using System.Windows.Input;
using libermedical.Helpers;
using libermedical.Models;
using libermedical.ViewModels.Base;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace libermedical.ViewModels
{
	public class MyAccountViewModel : ViewModelBase
	{
		private User _userInfo;
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PhoneNumber { get; set; }
		public string EmailAddress { get; set; }

		public MyAccountViewModel()
		{
			GetUserFromSettings();
		}

		private void GetUserFromSettings()
		{
			_userInfo = JsonConvert.DeserializeObject<User>(Settings.CurrentUser);
			FirstName = _userInfo.Firstname;
			LastName = _userInfo.Lastname;
			PhoneNumber = _userInfo.Phone;
			EmailAddress = _userInfo.Email;
		}

		public ICommand EditCommand => new Command(async () =>
		                                           await CoreMethods.PushPageModel<MyAccountEditViewModel>(null, true));

		protected override void ViewIsAppearing(object sender, EventArgs e)
		{
			GetUserFromSettings();
			base.ViewIsAppearing(sender, e);
		}
	}
}