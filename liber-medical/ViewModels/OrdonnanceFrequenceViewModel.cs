using System.Threading.Tasks;
using System.Windows.Input;
using libermedical.Enums;
using libermedical.Models;
using libermedical.ViewModels.Base;
using Xamarin.Forms;

namespace libermedical.ViewModels
{
	public class OrdonnanceFrequenceViewModel : ViewModelBase
	{
		public bool CanEdit { get; set; }
		public Frequency Frequency { get; set; }

		public OrdonnanceFrequenceViewModel()
		{
			Frequency = new Frequency();
			SubscribeMessage();
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
			await OpenFrequencyCotationsPage();
		});

		public ICommand LunchTappedCommand => new Command( async() =>
		{
			Frequency.Period = PeriodEnum.lunch;
			await OpenFrequencyCotationsPage();
		});

		public ICommand AfternoonTappedCommand => new Command( async() =>
		{
			Frequency.Period = PeriodEnum.afternoon;
			await OpenFrequencyCotationsPage();
		});

		public ICommand EveningTappedCommand => new Command(async() =>
		{
			Frequency.Period = PeriodEnum.evening;
			await OpenFrequencyCotationsPage();
		});

		private async Task OpenFrequencyCotationsPage()
		{
			await CoreMethods.PushPageModel<OrdonnanceFrequence2ViewModel>(Frequency, true);
			if (CanEdit)
				MessagingCenter.Send(this, Events.EnableCotationsEditMode, true);
		}

		private void SubscribeMessage()
		{
			MessagingCenter.Unsubscribe<OrdonnanceCreateEditViewModel, bool>(this, Events.EnableCotationsEditMode);
			MessagingCenter.Subscribe<OrdonnanceCreateEditViewModel, bool>(this, Events.EnableCotationsEditMode,
						(sender, canEdit) =>
						{
							CanEdit = canEdit;
						});
		}
	}
}
