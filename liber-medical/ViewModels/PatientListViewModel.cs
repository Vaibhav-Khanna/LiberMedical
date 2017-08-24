using libermedical.Models;
using libermedical.Services;
using libermedical.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace libermedical.ViewModels
{
    public class PatientListViewModel : ListViewModelBase<Patient>
    {
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
                FilterItems(_searchString);
                RaisePropertyChanged();
            }
        }

        private async void FilterItems(string searchString)
        {
            if(!string.IsNullOrEmpty(searchString))
            {
                ItemsSource = new ObservableCollection<Patient>(ItemsSource.Where(x => x.FullName.StartsWith(SearchString, StringComparison.OrdinalIgnoreCase)));
            }
            else
            {
                await GetDataAsync();
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
        }

        public override void ReverseInit(object value)
        {
            var newPatient = value as Patient;
            if (!ItemsSource.Contains(newPatient))
                ItemsSource.Add(newPatient);
        }
    }
}