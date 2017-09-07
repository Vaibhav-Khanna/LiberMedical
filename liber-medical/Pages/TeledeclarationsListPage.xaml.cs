using System;
using Xamarin.Forms;

namespace libermedical.Pages
{
	public partial class TeledeclarationsListPage
	{
		public TeledeclarationsListPage()
		{
			BindingContext = this;


			InitializeComponent();
		}

		private void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			((ListView) sender).SelectedItem = null;
		}


		private async void Bill_Tapped(object sender, EventArgs e)
		{
			await Navigation.PushModalAsync(new SecuriseBillsPage());
		}

		private void Handle_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			//((ListView)sender).SelectedItem = null;
		}
	}
}