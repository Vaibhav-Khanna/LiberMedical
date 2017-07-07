using System;
using System.Collections.Generic;
using libermedical.CustomControls;
using Xamarin.Forms;

namespace libermedical.Views
{
    public partial class AddPatient : BasePage
    {
        public AddPatient()
        {
            InitializeComponent();
        }
        void Cancel_Tapped(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new DetailsPatientListPage());
        }
        void Save_Tapped(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new DetailsPatientListPage());
        }

    }
}
