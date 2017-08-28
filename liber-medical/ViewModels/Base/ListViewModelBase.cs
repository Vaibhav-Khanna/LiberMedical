using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using libermedical.Models;
using libermedical.Services;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace libermedical.ViewModels.Base
{
	public abstract class ListViewModelBase<TModel> : ViewModelBase where TModel : BaseDTO
	{
		protected readonly IStorageService<TModel> _storageService;

		protected ListViewModelBase(IStorageService<TModel> storageService)
		{
			_storageService = storageService;
			ItemsSource = new ObservableCollection<GroupedItem<TModel>>();
		}

		private bool _isRefreshing;
		public bool IsRefreshing
		{
			get { return _isRefreshing; }
			set { _isRefreshing = value; RaisePropertyChanged(); }
		}


		private ObservableCollection<GroupedItem<TModel>> _itemsSource;
		public ObservableCollection<GroupedItem<TModel>> ItemsSource
		{
			get { return _itemsSource; }
			set { _itemsSource = value; RaisePropertyChanged(); }
		}
		public ICommand ListElementTapCommand => new Command<Cell>(async cell => await TapCommandFunc(cell));

		public ICommand AddUserCommand => new Command(
			async () => await CoreMethods.PushPageModelWithNewNavigation<AddEditPatientViewModel>(null));

		public ICommand RefreshCommand => new Command(async () =>
		{
			IsRefreshing = true;
			await GetDataAsync();
			IsRefreshing = false;
		});

		public override async void Init(object initData)
		{
			base.Init(initData);
		}

		protected virtual async Task GetDataAsync()
		{
			ItemsSource.Clear();
			var observableCollection = await _storageService.GetList();
			GroupItems(observableCollection.ToList());
		}

		protected override async void ViewIsAppearing(object sender, EventArgs e)
		{
			base.ViewIsAppearing(sender, e);
			await GetDataAsync();
		}

		protected abstract Task TapCommandFunc(Cell cell);

		private void GroupItems(List<TModel> observableCollection)
		{
			try
			{
				var groupedList = new ObservableCollection<GroupedItem<Patient>>();

				var patientsList = observableCollection.Cast<Patient>().ToList();
				var headers = patientsList.Select(x => x.LastName.Substring(0, 1)).Distinct().OrderBy(x=>x);
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


				ItemsSource = (ObservableCollection<GroupedItem<TModel>>)(object)groupedList;


			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}


	}
}