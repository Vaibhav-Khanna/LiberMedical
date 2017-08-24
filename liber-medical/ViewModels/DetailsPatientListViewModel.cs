using FreshMvvm;
using libermedical.Models;
using Xamarin.Forms;

namespace libermedical.ViewModels
{
	public class DetailsPatientListViewModel: FreshBasePageModel
	{
		public Patient Patient { get; set; }
		public override void Init(object initData)
		{
			base.Init(initData);
			this.Patient = (Patient) initData;
		}

        public Command EditPatient
        {
            get
            {
                return new Command(async () => {
                    await CoreMethods.PushPageModel<AddEditPatientViewModel>(Patient);
                });
            }
        }
    }
}
