using System.Windows.Input;
using libermedical.Models;
using libermedical.ViewModels.Base;
using Xamarin.Forms;

namespace libermedical.ViewModels
{
    public class OrdonnanceFrequence2ViewModel : ViewModelBase
    {
        private Frequency _frequency;
        public Frequency Frequency
        {
            get { return _frequency; }
            set
            {
                _frequency = value;
                RaisePropertyChanged();
            }
        }

        public OrdonnanceFrequence2ViewModel()
        {
            SubscribeMessage();
        }

        private void SubscribeMessage()
        {
            MessagingCenter.Subscribe<OrdonnanceCotationViewModel,Frequency>(this, Events.UpdateCotations, ((sender,args) => {

                if (args != null)
                    Frequency = args as Frequency;
            }));
        }
        public override void Init(object initData)
        {
            base.Init(initData);
            if (initData != null)
            {
                Frequency = initData as Frequency;
            }

            MessagingCenter.Send(this, Events.SetInitialPickerValue);
        }

        public ICommand SaveTappedCommand => new Command(async () =>
        {
            await CoreMethods.PopPageModel(null, true);
            await CoreMethods.PopPageModel(null, true);
        });

        public ICommand CotationsTappedCommand => new Command(async () =>
        {
            await CoreMethods.PushPageModel<OrdonnanceCotationViewModel>(Frequency, true);
        });
    }
}
