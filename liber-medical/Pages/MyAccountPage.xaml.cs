using libermedical.CustomControls;

namespace libermedical.Pages
{
	public partial class MyAccountPage : BasePage
    {
        public MyAccountPage() : base(-1, 0, false)
        {
            InitializeComponent();
        }
        async void Back_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        async void Edit_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new MyAccountEditPage());
        }
    }
}
