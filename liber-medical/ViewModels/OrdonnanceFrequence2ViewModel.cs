using System.Windows.Input;
using libermedical.Models;
using libermedical.ViewModels.Base;
using Xamarin.Forms;

namespace libermedical.ViewModels
{
    public class OrdonnanceFrequence2ViewModel : ViewModelBase
    {
        public Frequency Frequency { get; set; }

        public OrdonnanceFrequence2ViewModel()
        {
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
