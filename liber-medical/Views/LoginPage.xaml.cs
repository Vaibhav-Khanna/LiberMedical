using System;
using System.Collections.Generic;
using libermedical.CustomControls;
using Xamarin.Forms;

namespace libermedical.Views
{
    public partial class LoginPage : BasePage
    {
        public LoginPage() : base(-1, -1, false)
        {
            InitializeComponent();
        }
        async void Handle_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new MissingPasswordPage());
        }
    }
}
