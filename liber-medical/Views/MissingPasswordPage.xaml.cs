using System;
using System.Collections.Generic;
using libermedical.CustomControls;
using Xamarin.Forms;

namespace libermedical.Views
{
    public partial class MissingPasswordPage : BasePage
    {
        public MissingPasswordPage() : base(-1, -1, false)
        {
            InitializeComponent();
        }
        async void Handle_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }
    }
}
