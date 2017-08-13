using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using libermedical.DTO.Models;
using libermedical.Services;
using libermedical.ViewModels.Base;
using Xamarin.Forms;

namespace libermedical.ViewModels
{
	public class PatientListViewModel : ViewModelBase
	{
		private readonly IAzureService<Patient> _azureService;
		//IDatabaseService _databaseService;

		public string TypeDoc { get; set; }
		public string NavigationAfter { get; set; }

		public PatientListViewModel(IAzureService<Patient> azureService)
		{
			_azureService = azureService;
			ItemsSource = new ObservableCollection<Patient>();
		}

		public ICommand PatientTapCommand => new Command<TextCell>(async cell => await PatientTapCommandFunc(cell));
		public ICommand AddUserCommand => new Command(AddUserCommandFunc);
		public ICommand RefreshCommand => new Command(async () => await RefreshFunc());

		private async Task RefreshFunc()
		{
			IsRefreshing = true;

			await GetDataAsync();
			IsRefreshing = false;
		}

		private async Task GetDataAsync()
		{
			ItemsSource.Clear();
			var observableCollection = await _azureService.GetList();
			foreach (var _ in observableCollection)
			{
				ItemsSource.Add(_);
			}
		}

		public bool IsRefreshing { get; set; }

		private async void AddUserCommandFunc()
		{
			//navigation depuis la modal précédente donc en mode navigation
			await CoreMethods.PushPageModelWithNewNavigation<AddEditPatientViewModel>(null);
		}


		public ObservableCollection<Patient> ItemsSource { get; set; }

		public override async void Init(object initData)
		{
			await GetDataAsync();
		}

		// This is called when a page id pop'd
		public override void ReverseInit(object value)
		{
			var item = value as Patient;
			if (!ItemsSource.Contains(item))
			{
				ItemsSource.Add(item);
			}
		}

		public async Task PatientTapCommandFunc(TextCell textCell)
		{
			var ctx = textCell.BindingContext;
			await CoreMethods.PushPageModelWithNewNavigation<DetailsPatientListViewModel>(ctx);
		}
	}
}