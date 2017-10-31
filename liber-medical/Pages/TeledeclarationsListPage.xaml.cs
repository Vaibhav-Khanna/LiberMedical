﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using libermedical.Models;
using libermedical.ViewModels;
using Xamarin.Forms;

namespace libermedical.Pages
{
    public partial class TeledeclarationsListPage
    {
        private ObservableCollection<Teledeclaration> _filteredItems { get; set; }
        public TeledeclarationsListPage()
        {
            InitializeComponent();
        }


        private void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }


        async void Handle_Refreshing(object sender, System.EventArgs e)
        {
            await (BindingContext as TeledeclarationsListViewModel).BindData(20);
            TeledeclarationsList.EndRefresh();
        }


        private async void Bill_Tapped(object sender, EventArgs e)
        {
            (BindingContext as TeledeclarationsListViewModel).OpenInvoiceToSecuriseCommand.Execute(null);
        }

        private void Handle_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //((ListView)sender).SelectedItem = null;
        }

        void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                IEnumerable<Teledeclaration> foundItems;

                if (_filteredItems != null)
                {
                    foundItems =
                        _filteredItems.Where(x => x.Label.ToLower().Contains(e.NewTextValue.ToLower()));
                }
                else
                {
                    foundItems =
                        (BindingContext as TeledeclarationsListViewModel).Teledeclarations.Where(x => x.Label.ToLower().Contains(e.NewTextValue.ToLower()));
                }
                TeledeclarationsList.ItemsSource = foundItems;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(e.OldTextValue))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        searchBar.Unfocus();
                        TeledeclarationsList.Focus();
                    });
                }

                TeledeclarationsList.ItemsSource = _filteredItems ?? (BindingContext as TeledeclarationsListViewModel).Teledeclarations;
            }
        }

        private async void TeledeclarationsList_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if ((BindingContext as TeledeclarationsListViewModel).Teledeclarations.Count < (BindingContext as TeledeclarationsListViewModel).MaxCount)
            {
                var currentItem = e.Item as Teledeclaration;
                var lastItem = (BindingContext as TeledeclarationsListViewModel).Teledeclarations[(BindingContext as TeledeclarationsListViewModel).Teledeclarations.Count - 1];
                if (currentItem == lastItem)
                    await (BindingContext as TeledeclarationsListViewModel).BindData(20);
            }
        }
    }
}