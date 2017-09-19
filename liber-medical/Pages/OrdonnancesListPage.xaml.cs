using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using libermedical.Models;
using libermedical.ViewModels;
using Xamarin.Forms;

namespace libermedical.Pages
{
	public partial class OrdonnancesListPage
    {
        private ObservableCollection<Ordonnance> _filteredItems { get; set; }

        private Filter _filter;

        public OrdonnancesListPage()
        {
            InitializeComponent();

            DoAsyncActions();

            MessagingCenter.Subscribe<FilterPage, Filter>(this, Events.UpdatePrescriptionFilters, (sender, filter) =>
            {
                _filter = filter;

                ApplyFilter(filter);
            });
        }

        private async void DoAsyncActions()
        {
            while ((BindingContext as OrdonnancesListViewModel)?.Ordonnances == null)
            {
                await Task.Delay(100);
            }

            MyListView.ItemsSource = (BindingContext as OrdonnancesListViewModel).Ordonnances;
        }

        private void ApplyFilter(Filter filter)
        {
            if (filter != null)
            {
                List<Ordonnance> filteredItems = new List<Ordonnance>();
                foreach (var status in filter.Statuses)
                {
                    var foundItems =
                        (BindingContext as OrdonnancesListViewModel).Ordonnances.Where(x => x.Status == status && x.FirstCareAt >= filter.StartDate &&
                                                                     x.FirstCareAt <= filter.EndDate);
                    filteredItems.AddRange(foundItems);
                }

                _filteredItems = new ObservableCollection<Ordonnance>(filteredItems);
                MyListView.ItemsSource = _filteredItems;
            }
            else
            {
                _filteredItems = null;

                MyListView.ItemsSource = (BindingContext as OrdonnancesListViewModel).Ordonnances;
            }
        }
        
        async void Filter_Clicked(object sender, EventArgs e)
        {
			await Navigation.PushModalAsync(new FilterPage("Ordonnance",_filter));
        }

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
            }

            var item = e.SelectedItem as Ordonnance;
            (BindingContext as OrdonnancesListViewModel).SelectItemCommand.Execute(item);

            // disable the visual selection state.
            ((ListView)sender).SelectedItem = null;
        }
        
        private void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                IEnumerable<Ordonnance> foundItems;

                if (_filteredItems != null)
                {
                    foundItems =
                        _filteredItems.Where(x => x.PatientName.ToLower().Contains(e.NewTextValue.ToLower()));
                }
                else
                {
                    foundItems =
                        (BindingContext as OrdonnancesListViewModel).Ordonnances.Where(x => x.PatientName.ToLower().Contains(e.NewTextValue.ToLower()));
                }
                MyListView.ItemsSource = foundItems;
            }
            else
            {
                MyListView.ItemsSource = _filteredItems ?? (BindingContext as OrdonnancesListViewModel).Ordonnances;
            }
        }
    }
}
