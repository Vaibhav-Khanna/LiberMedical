using System.Windows.Input;
using libermedical.Enums;
using libermedical.Models;
using libermedical.ViewModels.Base;
using Xamarin.Forms;

namespace libermedical.ViewModels
{
    public class OrdonnanceFrequenceViewModel : ViewModelBase
    {
        public Frequency Frequency{ get; set; }

        public OrdonnanceFrequenceViewModel()
        {
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            if (initData != null)
            {
                Frequency = initData as Frequency;
            }
        }

        public ICommand MoringTappedCommand => new Command(async () =>
        {
            Frequency.Period = PeriodEnum.morning;
            await CoreMethods.PushPageModel<OrdonnanceFrequence2ViewModel>(Frequency, true);
        });

        public ICommand LunchTappedCommand => new Command(async () =>
        {
            Frequency.Period = PeriodEnum.lunch;
            await CoreMethods.PushPageModel<OrdonnanceFrequence2ViewModel>(Frequency, true);
        });

        public ICommand AfternoonTappedCommand => new Command(async () =>
        {
            Frequency.Period = PeriodEnum.afternoon;
            await CoreMethods.PushPageModel<OrdonnanceFrequence2ViewModel>(Frequency, true);
        });

        public ICommand EveningTappedCommand => new Command(async () =>
        {
            Frequency.Period = PeriodEnum.evening;
            await CoreMethods.PushPageModel<OrdonnanceFrequence2ViewModel>(Frequency, true);
        });
    }
}
