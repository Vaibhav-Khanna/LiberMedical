using libermedical.CustomControls;
using libermedical.Models;

namespace libermedical.Pages
{
    public partial class OrdonnanceDetailPage : BasePage
    {
        public OrdonnanceDetailPage(Ordonnance ordonnance) : base(-1, 0, false)
        {
            InitializeComponent();
        }
        async void Back_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        async void Ordonnance_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new OrdonnanceViewPage());
        }
    }
}
