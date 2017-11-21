﻿using System.Windows.Input;
using libermedical.Helpers;
using libermedical.Models;
using libermedical.ViewModels.Base;
using Newtonsoft.Json;
using Xamarin.Forms;
using System.Threading.Tasks;
using libermedical.Request;
using Plugin.Share;
using Plugin.Messaging;

namespace libermedical.ViewModels
{
	public class PlusViewModel: ViewModelBase
	{
        private bool _shouldShowContact;

        private Plugin.Share.Abstractions.BrowserOptions options = new Plugin.Share.Abstractions.BrowserOptions() {  ChromeShowTitle = true, SafariBarTintColor = new Plugin.Share.Abstractions.ShareColor(145,198,2), ChromeToolbarColor = new Plugin.Share.Abstractions.ShareColor(145, 198, 2), SafariControlTintColor = new Plugin.Share.Abstractions.ShareColor(255,255,255), UseSafariWebViewController = true };

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
        }


        public Command CGVCommand => new Command(async(obj) =>
       {
            await CrossShare.Current.OpenBrowser("https://www.google.com",options);
       });

        public Command CGUCommand => new Command(async (obj) =>
        {
            await CrossShare.Current.OpenBrowser("https://www.google.com", options);
        });

        public Command FAQCommand => new Command(async (obj) =>
        {
            await CrossShare.Current.OpenBrowser("https://www.google.com", options);
        });

        public ICommand GoToProfileCommand => new Command(async () => 
			await CoreMethods.PushPageModel<MyAccountViewModel>(null, false));



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

       

	}
}
