using System.Windows.Input;
using libermedical.Helpers;
using libermedical.Models;
using libermedical.ViewModels.Base;
using Newtonsoft.Json;
using Xamarin.Forms;
using System.Threading.Tasks;
using libermedical.Request;
using Plugin.Share;
using Plugin.Messaging;
using libermedical.Pages;
using libermedical.PopUp;

namespace libermedical.ViewModels
{
	public class PlusViewModel: ViewModelBase
	{
        private bool _shouldShowContact;
        public string AttachmentPath { get; set; } = null;
        public bool IsContractVisible { get; set; }

       
        public bool ShouldShowContact
        {
            get { return _shouldShowContact; }
            set
            {
                _shouldShowContact = value;
                RaisePropertyChanged();
            }
        }

        public PlusViewModel()
        {
            ShouldShowContact = string.IsNullOrEmpty(Settings.AdvisorContact) ? false : true;
            GetContract();
        }

        protected override void ViewIsAppearing(object sender, System.EventArgs e)
        {
            base.ViewIsAppearing(sender, e);

            GetContract();
        }

       
        public Command OpenOtherCommand => new Command(async () =>
       {
            await CoreMethods.PushPageModel<PlusOtherPageModel>(null,true,false);
       });

        public ICommand GoToProfileCommand => new Command(
            async () =>
        {          
            await CoreMethods.PushPageModelWithNewNavigation<MyAccountViewModel>(null,false);
        }
        );


		public ICommand ConnectCommand => new Command(async () =>
		{	
            var action = await CoreMethods.DisplayActionSheet("Contacter mon conseiller via:", "Annuler", null, "Appel vocal","E-mail", "SMS" );
            switch (action)
            {
                case "Appel vocal":
                    if (CrossMessaging.Current.PhoneDialer.CanMakePhoneCall)
                        CrossMessaging.Current.PhoneDialer.MakePhoneCall(Settings.AdvisorContact);                   
                    break;
                case "SMS":
                    var smsMessenger = CrossMessaging.Current.SmsMessenger;
                    if (smsMessenger.CanSendSms)
                        smsMessenger.SendSms(Settings.AdvisorContact, "Bonjour, je vous informe que mon TLA est branché");
                    break;
                case "E-mail":
                    if (CrossMessaging.Current.EmailMessenger.CanSendEmail)
                        CrossMessaging.Current.EmailMessenger.SendEmail(Settings.AdvisorEmail);
                    break;                        
            }
        });

        async Task GetContract()
        {

            var response = await App.UserManager.GetContract();

            if (response != null)
            {
                AttachmentPath = response.AttachmentPath;
                IsContractVisible = true;
            }
            else
            {
                IsContractVisible = false;
            }
        } 

        public Command ViewBills => new Command(async () =>
        {
            await CoreMethods.PushPageModel<MyBillsPageModel>(null,true,false);
        });

        public Command ViewContract => new Command(async () =>
        {
            if (AttachmentPath != null && IsContractVisible)
            {
                if (AttachmentPath.Contains(".pdf"))
                    await CoreMethods.PushPageModel<SecuriseBillsViewModel>(new Document() { AttachmentPath = AttachmentPath, Patient = new Patient() { FirstName = "Mon Contrat" } }, true);
                else
                    await CoreMethods.PushPageModel<OrdonnanceViewViewModel>(AttachmentPath, true);
            }
            else
            {
                await ToastService.Show("Contrat non disponible");
            }
        });


	}
}
