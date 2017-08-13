using System;
using System.Collections.Generic;
using libermedical.CustomControls;
using Xamarin.Forms;

namespace libermedical.Pages
{
    public partial class MissingPasswordPage : BasePage
    {
        public MissingPasswordPage() : base(0, 0, true)
        {
            InitializeComponent();
        }
        async void Handle_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }
    }
}
