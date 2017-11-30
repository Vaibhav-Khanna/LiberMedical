using libermedical.Models;
using libermedical.ViewModels;

namespace libermedical.Pages
{
	public partial class MyAccountPage
    {
        public MyAccountPage() : base(-1, 0, false)
        {
            InitializeComponent();
        }


        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            (BindingContext as MyAccountViewModel).OpenBill.Execute(e.Item as Invoice);
            listView.SelectedItem = null;
        }
    }
}