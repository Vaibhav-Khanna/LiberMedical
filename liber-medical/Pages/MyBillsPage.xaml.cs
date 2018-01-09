using System;
using System.Collections.Generic;
using libermedical.Models;
using libermedical.ViewModels;
using Xamarin.Forms;

namespace libermedical.Pages
{
    public partial class MyBillsPage 
    {
        public MyBillsPage() : base(-1,0, false)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
        }

        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            (BindingContext as MyBillsPageModel).OpenBill.Execute(e.Item as Invoice);
            listView.SelectedItem = null;
        }
    }
}
