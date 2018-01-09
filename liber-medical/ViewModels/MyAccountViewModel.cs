using System;
using System.Windows.Input;
using libermedical.Helpers;
using libermedical.Models;
using libermedical.ViewModels.Base;
using Newtonsoft.Json;
using Xamarin.Forms;
using System.Threading.Tasks;
using libermedical.PopUp;
using System.Collections.ObjectModel;
using System.Linq;

namespace libermedical.ViewModels
{
	public class MyAccountViewModel : ViewModelBase
	{
		private User _userInfo;
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PhoneNumber { get; set; }
		public string EmailAddress { get; set; }
       
        public string AttachmentPath { get; set; } = null;
        public bool IsContractVisible { get; set; }

       

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
     
        public override void Init(object initData)
        {
            base.Init(initData);

            MessagingCenter.Subscribe<MyAccountEditViewModel>(this, "ShowPasswordMessage", async (obj) =>
            {                
                BackCommand.Execute(null);
                await Task.Delay(800);
                await ToastService.Show("Votre nouveau mot de passe à bien été enregistré");
            });
        }

        public Command BackCommand => new Command(async() =>
       {
            MessagingCenter.Unsubscribe<MyAccountEditViewModel>(this, "ShowPasswordMessage");
            await App.tabbedNavigation.PopPage(true, false);
       });


		public ICommand EditCommand => new Command(async () =>
		                                           await CoreMethods.PushPageModel<MyAccountEditViewModel>(null,true));

		protected override void ViewIsAppearing(object sender, EventArgs e)
		{
			GetUserFromSettings();
			base.ViewIsAppearing(sender, e);
		}
	}
}