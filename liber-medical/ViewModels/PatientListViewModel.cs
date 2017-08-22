using libermedical.Models;
using libermedical.Services;
using libermedical.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
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
            BindData();
        }
        private async void BindData()
        {
            ItemsSource = new ObservableCollection<Patient>(await _patientsStorage.GetList());
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