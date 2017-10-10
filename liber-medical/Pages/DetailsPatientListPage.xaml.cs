﻿using System;
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (stackOrdonnance.SelectedItem != null)
                stackOrdonnance.SelectedItem = null;
        }
    }
}
