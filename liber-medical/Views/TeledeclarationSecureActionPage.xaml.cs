using System;
using System.Collections.Generic;
using libermedical.CustomControls;
using Xamarin.Forms;

namespace libermedical.Views
{
    public partial class TeledeclarationSecureActionPage : BasePage
    {
        public TeledeclarationSecureActionPage() : base(-1, 0, false)
        {
            InitializeComponent();
        }
        async void Back_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        async void Detail_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new TeledeclarationDetailPage());
        }
    }
}
