using System.Collections.ObjectModel;
using libermedical.Models;
using libermedical.ViewModels;
using Xamarin.Forms;

namespace libermedical.Pages
{
	public partial class PatientListPage
	{
		public ObservableCollection<Patient> patients { get; set; }

		public PatientListPage() : base(0, 0)
		{
			//ToolItemBarConstruction because we arrive from TabbedPage Directely

			Title = "Patients";
			InitializeComponent();
		}

		public PatientListPage(string navigationType, string typeDoc) : base(-1, 0, false)
		{
			InitializeComponent();
		}

		async void Back_Tapped(object sender, System.EventArgs e)
		{
			await Navigation.PopModalAsync();
		}

		void PatientTapped(object sender, System.EventArgs e)
		{
			var vm = (PatientListViewModel)BindingContext;
			var cell = (TextCell)sender;
			if (cell == null) return;
			vm.ListElementTapCommand.Execute(cell);
		}

		void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{
			((ListView) sender).SelectedItem = null;
		}
	}
}