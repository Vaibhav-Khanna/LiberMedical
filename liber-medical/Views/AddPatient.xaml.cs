using System;
using System.Collections.Generic;
using libermedical.CustomControls;
using Xamarin.Forms;

namespace libermedical.Views
{
    public partial class AddPatient : BasePage
    {

        private string pageBeforeType;

        public AddPatient(string pageBeforeType) : base(-1, 0, false)
        {
            this.pageBeforeType = pageBeforeType;
            InitializeComponent();
        }
        void Cancel_Tapped(object sender, System.EventArgs e)
        {
            if (this.pageBeforeType == "modal")
            { Navigation.PopModalAsync(); }
            else if (this.pageBeforeType == "navigation")
            { Navigation.PopAsync(); }

        }
        void Save_Tapped(object sender, System.EventArgs e)
        {
            if (this.pageBeforeType == "modal")
            { Navigation.PopModalAsync(); }
            else if (this.pageBeforeType == "navigation")
            { Navigation.PopAsync(); }
        }

    }
}
