using libermedical.Models;
using libermedical.Services;
using libermedical.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Diagnostics;
using System.Windows.Input;

namespace libermedical.ViewModels
{
	public class PatientListViewModel : ListViewModelBase<Patient>
	{
		private ObservableCollection<GroupedItem<Patient>> _filteredPatients;
		private IStorageService<Patient> _patientsStorage;
		public string ParentScreen { get; set; }

		public PatientListViewModel(IStorageService<Patient> storageService) : base(storageService)
		{
			_patientsStorage = storageService;
		}

		private string _searchString;
		public string SearchString
		{
			get { return _searchString; }
			set
			{
				_searchString = value;
				FilterGroupItems(_searchString);
				RaisePropertyChanged();
			}
		}

        public ICommand BackArrowCommand => new Command(
            async () => await CoreMethods.PopModalNavigationService());
        private async void FilterGroupItems(string searchString)
		{
			try
			{
				if (!string.IsNullOrEmpty(searchString))
				{
					var groupedList = new ObservableCollection<GroupedItem<Patient>>();

					var patientsList = (await _patientsStorage.GetList()).Where(x => x.FullName.StartsWith(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
					var headers = patientsList.Select(x => x.LastName.Substring(0, 1)).Distinct().OrderBy(x => x);
					foreach (var headerkey in headers)
					{
						var patientGroup = new GroupedItem<Patient>();
						patientGroup.HeaderKey = headerkey;
						foreach (var item in patientsList.Where(x => x.LastName.StartsWith(headerkey, StringComparison.OrdinalIgnoreCase)).ToList())
						{
							patientGroup.Add(item);
						}
						groupedList.Add((GroupedItem<Patient>)(object)patientGroup);
					}


					_filteredPatients = (ObservableCollection<GroupedItem<Patient>>)(object)groupedList;
					ItemsSource = _filteredPatients;
				}
				else
				{
					await GetDataAsync();
				}

			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}

		}
		protected override async Task TapCommandFunc(Cell cell)
		{
			var ctx = cell.BindingContext;
			await CoreMethods.PushPageModelWithNewNavigation<DetailsPatientListViewModel>(ctx);
		}

		protected override void ViewIsAppearing(object sender, EventArgs e)
		{
			base.ViewIsAppearing(sender, e);
			SearchString = string.Empty;
		}

	}
}