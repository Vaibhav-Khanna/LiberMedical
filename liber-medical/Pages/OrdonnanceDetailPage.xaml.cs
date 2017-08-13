using System;
using System.Collections.Generic;
using libermedical.CustomControls;
using Xamarin.Forms;

namespace libermedical.Pages
{
    public partial class OrdonnanceDetailPage : BasePage
    {
        public OrdonnanceDetailPage() : base(-1, -1, false)
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
