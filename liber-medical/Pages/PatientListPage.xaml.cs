using System.Collections.ObjectModel;
using libermedical.Models;
using libermedical.ViewModels;
using Xamarin.Forms;

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

		void Handle_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			((ListView) sender).SelectedItem = null;
		}
	}
}