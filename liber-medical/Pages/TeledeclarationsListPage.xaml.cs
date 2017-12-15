using System;
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

                foundItems = (BindingContext as TeledeclarationsListViewModel).Teledeclarations.Where(x => x.Label.ToLower().Contains(e.NewTextValue.ToLower()));

                (BindingContext as TeledeclarationsListViewModel).Teledeclarations = new ObservableCollection<Teledeclaration>(foundItems);

                if(!foundItems.Any() && (BindingContext as TeledeclarationsListViewModel)._teledeclarationsAll.Count < (BindingContext as TeledeclarationsListViewModel).MaxCount)
                {
                    
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(e.OldTextValue))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        searchBar.Unfocus();
                        TeledeclarationsList.Focus();
                        TeledeclarationsList.SetBinding(ListView.ItemsSourceProperty, "Teledeclarations");
                        (BindingContext as TeledeclarationsListViewModel).Teledeclarations = (BindingContext as TeledeclarationsListViewModel).Teledeclarations;
                    });
                }

                (BindingContext as TeledeclarationsListViewModel).Teledeclarations = (BindingContext as TeledeclarationsListViewModel).Teledeclarations;
            }
        }

        bool isExecuting = false;

        private async void TeledeclarationsList_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (isExecuting)
                return;

            isExecuting = true;

            if ((BindingContext as TeledeclarationsListViewModel).Teledeclarations.Count < (BindingContext as TeledeclarationsListViewModel).MaxCount)
            {
                if (string.IsNullOrWhiteSpace(searchBar.Text))
                    indicator.IsVisible = true;
                else
                    indicator.IsVisible = false;
                
                var currentItem = e.Item as Teledeclaration;
                var lastItem = (BindingContext as TeledeclarationsListViewModel).Teledeclarations[(BindingContext as TeledeclarationsListViewModel).Teledeclarations.Count - 1];
              
                if (currentItem == lastItem && string.IsNullOrWhiteSpace(searchBar.Text))
                    await (BindingContext as TeledeclarationsListViewModel).BindData(20);
            }
            else
            {
                indicator.IsVisible = false;
            }

            isExecuting = false;
        }
    }
}