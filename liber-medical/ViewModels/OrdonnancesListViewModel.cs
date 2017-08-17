using System.Threading.Tasks;
using libermedical.Models;
using libermedical.Services;
using libermedical.ViewModels.Base;
using Xamarin.Forms;

namespace libermedical.ViewModels
{
	public class OrdonnancesListViewModel : ListViewModelBase<Ordonnance>
	{
		public OrdonnancesListViewModel(IStorageService<Ordonnance> storageService) : base(storageService)
		{
		}

		protected override async Task TapCommandFunc(Cell cell)
		{
			var ctx = cell.BindingContext;
			await CoreMethods.PushPageModelWithNewNavigation<DetailsPatientListViewModel>(ctx);
		}
	}
}
