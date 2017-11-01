using System.Collections.ObjectModel;
using libermedical.Models;
using libermedical.ViewModels;
using Xamarin.Forms;
using System.Linq;
using System.Threading.Tasks;

namespace libermedical.Pages
{
    
	public partial class PatientListPage
	{
		public ObservableCollection<Patient> patients { get; set; }

		public PatientListPage()
		{
			//ToolItemBarConstruction because we arrive from TabbedPage Directely

			Title = "Patients";
			InitializeComponent();
		}

		void PatientTapped(object sender, System.EventArgs e)
		{
			var vm = (PatientListViewModel)BindingContext;
			var cell = (ViewCell)sender;
			if (cell == null) return;
			vm.ListElementTapCommand.Execute(cell);
		}

        async Task Handle_Refreshing(object sender, System.EventArgs e)
        {
            await (BindingContext as PatientListViewModel)._patientsStorage.SyncTables();
            await (BindingContext as PatientListViewModel).BindData(20);
            PatientListView.EndRefresh();
            isExecuting = false;
        }

        void Handle_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			((ListView) sender).SelectedItem = null;
		}

        void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.OldTextValue) && string.IsNullOrWhiteSpace(e.NewTextValue))
            {              
                    Device.BeginInvokeOnMainThread( () => 
                    {
                        searchBar.Unfocus();
                        PatientListView.Focus();
                    });
            }
        }

        bool isExecuting = false;

        private async void PatientListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (isExecuting)
                return;

            isExecuting = true;

            if ((BindingContext as PatientListViewModel).CurrentCount < (BindingContext as PatientListViewModel).MaxCount)
            {
                if (string.IsNullOrWhiteSpace(searchBar.Text))
                    indicator.IsVisible = true;
                else
                    indicator.IsVisible = false;

                var currentItem = e.Item as Patient;
                var lastItem = (BindingContext as PatientListViewModel).ItemsSource[(BindingContext as PatientListViewModel).ItemsSource.Count - 1];
               
                if (currentItem == lastItem[lastItem.Count - 1] && string.IsNullOrWhiteSpace(searchBar.Text))
                {
                    await (BindingContext as PatientListViewModel).BindData(20);
                }
            }
            else
            {
                indicator.IsVisible = false;
            }

            isExecuting = false;
        }
    }
}