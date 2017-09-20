using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using libermedical.Models;
using libermedical.Request;
using libermedical.Services;
using libermedical.ViewModels.Base;
using Xamarin.Forms;

namespace libermedical.ViewModels
{
	public class PatientListViewModel : ListViewModelBase<Patient>
	{
		private ObservableCollection<GroupedItem<Patient>> _filteredPatients;
		private ObservableCollection<GroupedItem<Patient>> Patients;
		private IStorageService<Patient> _patientsStorage;
		private string NavigationType;
        private string ParentScreen;
        private string DocType;
		
		public PatientListViewModel(IStorageService<Patient> storageService) : base(storageService)
		{
			_patientsStorage = storageService;
			BindData();
		}

		private async void BindData()
		{
			if (App.IsConnected())
			{
				var request = new GetListRequest(20, 0);
				var patients =
					new ObservableCollection<Patient>((await App.PatientsManager.GetListAsync(request)).rows);

				//Updating records in local cache
				await _storageService.DeleteAllAsync();
				await _storageService.AddManyAsync(patients.ToList());
			}
			ItemsSource.Clear();
			GroupItems((await _storageService.GetList()).ToList());
		}

		private void GroupItems(List<Patient> observableCollection)
		{
			try
			{
				var groupedList = new ObservableCollection<GroupedItem<Patient>>();

				var patientsList = observableCollection.Cast<Patient>().ToList();
				var headers = patientsList.Select(x => x.LastName.Substring(0, 1));

				headers = headers.Select(h => char.ToUpper(h[0]).ToString()).Distinct().OrderBy(x => x);

				foreach (var headerkey in headers)
				{
					var patientGroup = new GroupedItem<Patient> { HeaderKey = headerkey };
					foreach (var item in patientsList.Where(x => x.LastName.StartsWith(headerkey, StringComparison.OrdinalIgnoreCase)).ToList())
					{
						patientGroup.Add(item);
					}
					groupedList.Add(patientGroup);
				}

				ItemsSource = groupedList;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
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
		public ICommand AddUserCommand => new Command(
			async () =>
			{
				await PushPageModelWithNewNavigation<AddEditPatientViewModel>(null);

			});

		private async void FilterGroupItems(string searchString)
		{
			try
			{
				if (!string.IsNullOrEmpty(searchString))
				{
					var groupedList = new ObservableCollection<GroupedItem<Patient>>();

					var patientsList = (await _patientsStorage.GetList()).Where(x => x.Fullname.StartsWith(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
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
			if (ParentScreen == "HomeSelectPatient")
			{
			    MessagingCenter.Send(this, Events.HomePageSetPatientForOrdonnance, cell.BindingContext as Patient);
                await Application.Current.MainPage.Navigation.PopModalAsync();
			}
            else if (ParentScreen == "OrdonanceSelectPatient")
			{
			    MessagingCenter.Send(this, Events.OrdonnancePageSetPatientForOrdonnance, cell.BindingContext as Patient);
			    await Application.Current.MainPage.Navigation.PopModalAsync();
			}
			else
			{
				var ctx = cell.BindingContext;
				await CoreMethods.PushPageModelWithNewNavigation<DetailsPatientListViewModel>(ctx);
			}
		}

		public override async void Init(object initData)
		{
			base.Init(initData);
			if (initData != null)
            {
                var values = initData as string[];
                NavigationType = values[1];
                ParentScreen = values[0];
                DocType = values[2];
            }
        }
		protected override async void ViewIsAppearing(object sender, EventArgs e)
		{
			base.ViewIsAppearing(sender, e);
			SearchString = string.Empty;
			GroupItems((await _storageService.GetList()).ToList());
		}

	}
}