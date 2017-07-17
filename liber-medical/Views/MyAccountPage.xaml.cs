using System;
using System.Collections.Generic;
using libermedical.CustomControls;
using Xamarin.Forms;

namespace libermedical.Views
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
