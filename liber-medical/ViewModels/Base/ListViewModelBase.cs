using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using libermedical.Models;
using libermedical.Services;
using Xamarin.Forms;
using libermedical.Request;

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

		public ICommand RefreshCommand => new Command(async () =>
		{
			IsRefreshing = true;
			await GetDataAsync();
			IsRefreshing = false;
		});



		protected virtual async Task GetDataAsync()
		{
            
            if (App.IsConnected())
            {
                var request = new GetListRequest(20, 0);
                var patients =
                    new ObservableCollection<Patient>((await App.PatientsManager.GetListAsync(request)).rows);

                //Updating records in local cache
                await _storageService.DeleteAllAsync();
                await _storageService.AddManyAsync((List<TModel>)(object)patients.ToList());
            }
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
				var groupedList = new ObservableCollection<GroupedItem<TModel>>();

				var patientsList = observableCollection.Cast<Patient>().ToList();
				var headers = patientsList.Select(x => x.LastName.Substring(0, 1));

				headers = headers.Select(h => char.ToUpper(h[0]).ToString()).Distinct().OrderBy(x => x);

				foreach (var headerkey in headers)
				{
					var patientGroup = new GroupedItem<TModel> {HeaderKey = headerkey};
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


	}
}