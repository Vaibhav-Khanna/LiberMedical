using libermedical.CustomControls;

namespace libermedical.Pages
{
    public partial class OrdonnanceFrequencePage : BasePage
    {
        public OrdonnanceFrequencePage() : base(-1, 0, false)
        {
            InitializeComponent();
        }
        async void Cancel_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        async void Frequence_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new OrdonnanceFrequence2Page());
        }
    }
}
