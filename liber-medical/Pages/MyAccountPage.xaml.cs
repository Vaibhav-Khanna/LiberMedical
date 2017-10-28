using libermedical.ViewModels;

namespace libermedical.Pages
{
	public partial class MyAccountPage
    {
        public MyAccountPage() : base(-1, 0, false)
        {
            InitializeComponent();
        }

        void Handle_Tapped(object sender, System.EventArgs e)
        {
            (BindingContext as MyAccountViewModel).ViewContract.Execute(null);
        }
    }
}