using System;
using System.Collections.ObjectModel;
using libermedical.Enums;
using libermedical.Models;
using Xamarin.Forms;

namespace libermedical.Pages
{
	public partial class DetailsPatientListPage
    {


        public DetailsPatientListPage() : base(-1, 64, false)
        {
            InitializeComponent();
        }

       
        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
    }
}
