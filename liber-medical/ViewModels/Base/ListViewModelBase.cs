using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using libermedical.Models;
using libermedical.Services;
using Xamarin.Forms;

namespace libermedical.ViewModels.Base
{
	public abstract class ListViewModelBase<TModel> : ViewModelBase where TModel : BaseDTO
	{
		protected readonly IStorageService<TModel> _storageService;

		protected ListViewModelBase(IStorageService<TModel> storageService)
		{
			_storageService = storageService;
			ItemsSource = new ObservableCollection<TModel>();
		}


		public bool IsRefreshing { get; set; }
		public ObservableCollection<TModel> ItemsSource { get; set; }

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
			await GetDataAsync();
		}

		protected virtual async Task GetDataAsync()
		{
			ItemsSource.Clear();
			var observableCollection = await _storageService.GetList();
			foreach (var _ in observableCollection)
				ItemsSource.Add(_);
		}

		protected abstract Task TapCommandFunc(Cell cell);
	}
}