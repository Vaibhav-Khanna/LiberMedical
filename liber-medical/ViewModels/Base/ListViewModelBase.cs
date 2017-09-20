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

		}

		protected override async void ViewIsAppearing(object sender, EventArgs e)
		{
			base.ViewIsAppearing(sender, e);
			await GetDataAsync();
		}

		protected abstract Task TapCommandFunc(Cell cell);




	}
}