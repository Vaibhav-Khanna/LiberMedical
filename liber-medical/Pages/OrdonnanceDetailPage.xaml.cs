using System;
using libermedical.CustomControls;
using libermedical.ViewModels;

namespace libermedical.Pages
{
    public partial class OrdonnanceDetailPage : BasePage
    {
        public OrdonnanceDetailPage() : base(-1, 0, false)
        {
            InitializeComponent();
        }
        async void Back_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        async void Ordonnance_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new OrdonnanceViewPage());
        }

        private void Edit_OnTapped(object sender, EventArgs e)
        {
            (BindingContext as OrdonnanceDetailViewModel).CreateOrdonnanceCommand.Execute(sender);
        }
    }
}
