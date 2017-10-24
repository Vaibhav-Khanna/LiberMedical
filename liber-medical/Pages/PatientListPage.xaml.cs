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
			var cell = (TextCell)sender;
			if (cell == null) return;
			vm.ListElementTapCommand.Execute(cell);
		}

        async void Handle_Refreshing(object sender, System.EventArgs e)
        {
            await (BindingContext as PatientListViewModel).BindData();
            PatientListView.IsRefreshing = false;
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


	}
}