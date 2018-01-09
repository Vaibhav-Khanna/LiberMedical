using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using libermedical.Helpers;
using libermedical.Models;
using libermedical.Request;
using libermedical.Services;
using libermedical.ViewModels.Base;
using Xamarin.Forms;

namespace libermedical.ViewModels
{
	public class PatientListViewModel : ListViewModelBase<Patient>
	{
        int _initCount = 0;
        public int MaxCount { get; set; }
        public int CurrentCount { get; set; }
        private ObservableCollection<GroupedItem<Patient>> _filteredPatients;
        public ObservableCollection<GroupedItem<Patient>> Patients;
        public IStorageService<Patient> _patientsStorage;
		private string NavigationType;
        public string ParentScreen=null;
        private string DocType;
		
		public PatientListViewModel(IStorageService<Patient> storageService) : base(storageService)
		{
			_patientsStorage = storageService;
			BindData(20);
        }


        public async Task BindData(int count)
        {
            _initCount = _initCount + count;

            MaxCount = await new StorageService<Patient>().DownloadPatients(_initCount);

            DownlaodDocuments();

            var list = await _storageService.GetList();
            if (list != null && list.Count() != 0)
            {
                list = list.DistinctBy((arg) => arg.Id);
                list = list.OrderByDescending((arg) => arg.CreatedAt);
            }

            CurrentCount = list.Count();
            GroupItems(list.ToList());
        }
       
        private async Task DownlaodDocuments()
        {
            if (App.IsConnected())
            {
                var request = new GetListRequest(600, 0);
               
                var documents = await App.DocumentsManager.GetListAsync(request);

                //Updating records in local cache
                if(documents!=null && documents.rows!=null)
                await new StorageService<Document>().InvalidateSyncedItems();
                
                await new StorageService<Document>().AddManyAsync(documents.rows);
            }
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

                Patients = groupedList;
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


        public Command CloseCommand => new Command((obj) =>
       {
            CoreMethods.PopPageModel(true,false);
       });

		private async void FilterGroupItems(string searchString)
		{
			try
			{
                searchString = searchString.ToLower();

				if (!string.IsNullOrEmpty(searchString))
				{
					var groupedList = new ObservableCollection<GroupedItem<Patient>>();
                    var xlist = (await _patientsStorage.GetList());
                    var patientsList = xlist.Where(x => x.Fullname.ToLower().Contains(searchString) ).ToList();
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


                    if (!_filteredPatients.Any() && xlist.Count() < MaxCount)
                    {
                        MaxCount = await new StorageService<Patient>().DownloadPatients(MaxCount);
                        FilterGroupItems(searchString);
                    }

				}
				else
				{
                    ItemsSource = Patients;
					//await GetDataAsync();
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
                await CoreMethods.PopPageModel(true);
			}
            else if (ParentScreen == "OrdonanceSelectPatient")
			{
			    MessagingCenter.Send(this, Events.OrdonnancePageSetPatientForOrdonnance, cell.BindingContext as Patient);
                await CoreMethods.PopPageModel(true);
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

            var list = await _storageService.GetList();
            if (list != null && list.Count() != 0)
            {
                list = list.DistinctBy((arg) => arg.Id);
                list = list.OrderByDescending((arg) => arg.CreatedAt);
            }

            GroupItems(list.ToList());
           
        }

	}
}