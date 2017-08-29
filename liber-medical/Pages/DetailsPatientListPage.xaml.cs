﻿using System;
using System.Collections.ObjectModel;
using libermedical.Models;
using Xamarin.Forms;
using libermedical.CustomControls;

namespace libermedical.Pages
{
	public partial class DetailsPatientListPage : BasePage
    {        

        public DetailsPatientListPage() : base(-1, 64, false)
        {           
            InitializeComponent();

        }

        async void Back_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        async void Edit_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new PatientDetailModify());
        }

        
        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
    }
}
