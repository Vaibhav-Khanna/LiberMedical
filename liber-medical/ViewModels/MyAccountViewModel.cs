using System;
using System.Windows.Input;
using libermedical.Helpers;
using libermedical.ViewModels.Base;
using PropertyChanged;
using Xamarin.Forms;

namespace libermedical.ViewModels
{
	[AddINotifyPropertyChangedInterfaceAttribute]
	public class MyAccountViewModel : ViewModelBase
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PhoneNumber { get; set; }
		public string EmailAddress { get; set; }

		public ICommand EditCommand => new Command(async () =>
			await CoreMethods.PushPageModel<MyAccountEditViewModel>(null, true));


		public ICommand TestCommand => new Command(() =>
		{
			FirstName += "0";
//			RaisePropertyChanged("FirstName");
		});

		protected override void ViewIsAppearing(object sender, EventArgs e)
		{

			FirstName = Settings.CurrentUser.FirstName;
			LastName = Settings.CurrentUser.LastName;
			PhoneNumber = Settings.CurrentUser.PhoneNumber;
			EmailAddress = Settings.CurrentUser.EmailAddress;
			base.ViewIsAppearing(sender, e);
		}
	}
}