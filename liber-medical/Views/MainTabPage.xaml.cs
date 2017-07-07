using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace libermedical.Views
{
    public partial class MainTabPage : TabbedPage
    {
        public MainTabPage()
        {
            BindingContext = this.CurrentPage;
            this.CurrentPageChanged += CurrentPageHasChanged;
            InitializeComponent();
        }


        protected void CurrentPageHasChanged(object sender, EventArgs e)
        {

            if (this.CurrentPage.Title == "Accueil")
            { this.Title = "LiberMedical"; }
            else { this.Title = this.CurrentPage.Title; }

        }
    }
}
