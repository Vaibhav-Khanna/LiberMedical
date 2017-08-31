using System.Threading.Tasks;
using libermedical.Models;
using libermedical.Services;
using libermedical.ViewModels.Base;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace libermedical.ViewModels
{
	public class OrdonnancesListViewModel : ListViewModelBase<Ordonnance>
	{
        private IStorageService<Ordonnance> _ordonnanceStorage;
        public ObservableCollection<Ordonnance> Ordonnances { get; set; }
        public OrdonnancesListViewModel(IStorageService<Ordonnance> storageService) : base(storageService)
		{
            _ordonnanceStorage = storageService;
            BindData();
		}

        private async void BindData()
        {
            Ordonnances = new ObservableCollection<Ordonnance>(await _ordonnanceStorage.GetList());
        }
        protected override async Task TapCommandFunc(Cell cell)
		{
			var ctx = cell.BindingContext;
			await CoreMethods.PushPageModelWithNewNavigation<DetailsPatientListViewModel>(ctx);
		}

	    public async void CreatePrescription()
	    {
	        await CoreMethods.PushPageModel<OrdonnanceDetailEditViewModel>(null, true);
	    }
	}
}
