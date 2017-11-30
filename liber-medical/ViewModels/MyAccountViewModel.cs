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
        public bool NoBillsFound { get; set; } = false;
        public string AttachmentPath { get; set; } = null;
        public bool IsContractVisible { get; set; }

      

        public Command OpenBill => new Command(async (obj) =>
       {
            if ( ((Invoice)obj).FilePath.Contains(".pdf"))
                await CoreMethods.PushPageModel<SecuriseBillsViewModel>(((Invoice)obj), true);
           else
                await CoreMethods.PushPageModel<OrdonnanceViewViewModel>(((Invoice)obj), true);
       });


        public ObservableCollection<Invoice> Bills { get; set; } = new ObservableCollection<Invoice>();

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
            GetContract();
            GetBills();
		}

        async Task GetContract()
        {
            
            var response =  await App.UserManager.GetContract();

            if(response!=null)
            {
                AttachmentPath = response.AttachmentPath;
                IsContractVisible = true;
            }
            else
            {
                IsContractVisible = false;
            }
        }

        async Task GetBills()
        {
            var list_response = await App.BillsManager.GetListAsync(new Request.GetListRequest(100,0,sortDirection: Enums.SortDirectionEnum.Desc ));

            if (list_response!=null && list_response.rows!=null && list_response.rows.Any())
            {
                NoBillsFound = false;
                Bills = new ObservableCollection<Invoice>(list_response.rows);
            }
            else
            {
                NoBillsFound = true;
            }
        }


        public Command BackCommand => new Command(async() =>
       {
            await CoreMethods.PopPageModel();
       });

        public Command ViewContract => new Command( async() =>
       {
            if (AttachmentPath != null && IsContractVisible)
           {
               if (AttachmentPath.Contains(".pdf"))
                    await CoreMethods.PushPageModel<SecuriseBillsViewModel>(new Document(){ AttachmentPath = AttachmentPath, Patient = new Patient() { FirstName = "Mon Contrat" } }, true);
               else
                    await CoreMethods.PushPageModel<OrdonnanceViewViewModel>(AttachmentPath, true);
            }
            else
            {
                await ToastService.Show("Contrat non disponible");
            }
       });

		public ICommand EditCommand => new Command(async () =>
		                                           await CoreMethods.PushPageModel<MyAccountEditViewModel>(null, true));

		protected override void ViewIsAppearing(object sender, EventArgs e)
		{
			GetUserFromSettings();
			base.ViewIsAppearing(sender, e);
		}
	}
}