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

			MessagingCenter.Subscribe<FilterPage, Filter>(this, Events.UpdatePrescriptionFilters, (sender, filter) =>
			{
				_filter = filter;

				ApplyFilter(filter);
			});
		}


        async void Handle_Refreshing(object sender, System.EventArgs e)
        {
            await (BindingContext as OrdonnancesListViewModel).BindData();
            MyListView.IsRefreshing = false;
        }
      

		private void ApplyFilter(Filter filter)
		{
			if (filter != null && filter.IsActivated)
			{
				List<Ordonnance> filteredItems = new List<Ordonnance>();
				List<Ordonnance> foundItems = new List<Ordonnance>();

				if (filter.Statuses.Count > 0)
				{

					foreach (var status in filter.Statuses)
					{
						if (filter.EnableDateSearch)
						{
							foundItems =
							   (BindingContext as OrdonnancesListViewModel).Ordonnances.Where(x => x.Status == status && x.FirstCareAt >= filter.StartDate &&
																							  x.FirstCareAt <= filter.EndDate).ToList();
						}
						else
						{
							foundItems =
							   (BindingContext as OrdonnancesListViewModel).Ordonnances.Where(x => x.Status == status).ToList();
						}
						filteredItems.AddRange(foundItems);
					}
				}
				else
				{
					if (filter.EnableDateSearch)
					{
						foundItems =
						   (BindingContext as OrdonnancesListViewModel).Ordonnances.Where( x=>x.FirstCareAt >= filter.StartDate &&
																						  x.FirstCareAt <= filter.EndDate).ToList();
						filteredItems.AddRange(foundItems);
					}
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
			await Navigation.PushModalAsync(new FilterPage("Ordonnance", _filter));
		}

		void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem == null)
			{
				return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
			}

			var item = e.SelectedItem as Ordonnance;
			(BindingContext as OrdonnancesListViewModel).SelectItemCommand.Execute(item);

			//// disable the visual selection state.
			((ListView)sender).SelectedItem = null;
		}

       

		private void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
		{
            if (!string.IsNullOrWhiteSpace(e.NewTextValue))
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
                if (!string.IsNullOrWhiteSpace(e.OldTextValue))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        searchBar.Unfocus();
                        MyListView.Focus();
                    });
                }

				MyListView.ItemsSource = _filteredItems ?? (BindingContext as OrdonnancesListViewModel).Ordonnances;
			}
		}



	}
}
