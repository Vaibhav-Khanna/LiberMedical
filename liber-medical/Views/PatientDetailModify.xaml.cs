using System;
using System.Collections.Generic;
using libermedical.CustomControls;
using Xamarin.Forms;

namespace libermedical.Views
{
    public partial class PatientDetailModify : BasePage
    {
        public PatientDetailModify()
        {
            InitializeComponent();
        }
        void Cancel_Tapped(object sender, System.EventArgs e)
        {
            Navigation.PopModalAsync();
        }
        void Save_Tapped(object sender, System.EventArgs e)
        {
            Navigation.PopModalAsync();
        }

    }
}
