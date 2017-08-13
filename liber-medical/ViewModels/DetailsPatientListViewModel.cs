using FreshMvvm;
using libermedical.DTO.Models;

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
	}
}
