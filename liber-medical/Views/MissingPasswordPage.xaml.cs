using System;
using System.Collections.Generic;
using libermedical.CustomControls;
using Xamarin.Forms;

namespace libermedical.Views
{
    public partial class MissingPasswordPage : BasePage
    {
        public MissingPasswordPage()
        {
            InitializeComponent();
        }
		void Handle_Tapped(object sender, System.EventArgs e)
		{
			Navigation.PushAsync(new LoginPage());
		}
    }
}
