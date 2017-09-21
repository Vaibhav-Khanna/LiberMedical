using System.Windows.Input;
using libermedical.Helpers;
using libermedical.Models;
using libermedical.ViewModels.Base;
using Newtonsoft.Json;
using Xamarin.Forms;
using System.Threading.Tasks;
using libermedical.Request;

namespace libermedical.ViewModels
{
	public class PlusViewModel: ViewModelBase
	{
        private bool _shouldShowContact;
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
        public ICommand GoToProfileCommand => new Command(async () => 
			await CoreMethods.PushPageModel<MyAccountViewModel>(null, true));

		public ICommand ConnectCommand => new Command(async () =>
		{	
			var action = await CoreMethods.DisplayActionSheet("Contacter mon conseiller via:", "Annuler", null, "Appel vocal", "SMS");
            switch (action)
            {
                case "Appel vocal":
                    Device.OpenUri(new System.Uri($"tel:{Settings.AdvisorContact}"));
                    break;
                case "SMS":
                    Device.OpenUri(new System.Uri($"sms:{Settings.AdvisorContact}"));
                    break;
            }
        });

       

	}
}
