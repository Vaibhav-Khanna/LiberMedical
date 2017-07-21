using System;
using System.Collections.Generic;
using libermedical.CustomControls;
using Xamarin.Forms;

namespace libermedical.Views
{
    public partial class FilterPage : BasePage
    {
        public FilterPage() : base(-1, 64, false)
        {
            InitializeComponent();
        }
        async void Back_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        async void Reset_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        async void Search_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }


    }
}
