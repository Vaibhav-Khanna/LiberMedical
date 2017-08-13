using System;
using System.Collections.Generic;
using libermedical.CustomControls;
using Xamarin.Forms;

namespace libermedical.Pages
{
    public partial class PatientDetailModify : BasePage
    {
        public PatientDetailModify() : base(-1, 0, false)
        {
            InitializeComponent();
        }
        void Cancel_Tapped(object sender, System.EventArgs e)
        {
            Navigation.PopAsync();
        }
        void Save_Tapped(object sender, System.EventArgs e)
        {
            Navigation.PopAsync();
        }

    }
}
