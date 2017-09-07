using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using libermedical.Enums;
using libermedical.Models;
using Xamarin.Forms;

namespace libermedical.Pages
{
	public partial class OrdonnancesListPage
    {
        public ObservableCollection<Ordonnance> Ordonnances { get; set; }
        private ObservableCollection<Ordonnance> _filteredItems { get; set; }

        private Filter _filter;

        public OrdonnancesListPage()
        {
            BindingContext = this;
            Ordonnances = new ObservableCollection<Ordonnance>
            {
                new Ordonnance
                {
                    Reference = 1,
                    Patient = new Patient {FirstName = "Fred", LastName = "Pearson"},
                    FirstCareAt = new DateTime(2017, 04, 02),
                    Status = StatusEnum.valid
                },
                new Ordonnance
                {
                    Reference = 2,
                    Patient = new Patient {FirstName = "Jonanthan", LastName = "Vaughn"},
                    FirstCareAt = new DateTime(2017, 11, 10),
                    Status = StatusEnum.valid
                },
                new Ordonnance
                {
                    Reference = 3,
                    Patient = new Patient {FirstName = "Alexander", LastName = "Zimmerman"},
                    FirstCareAt = new DateTime(2017, 02, 06),
                    Status = StatusEnum.refused
                },
                new Ordonnance
                {
                    Reference = 4,
                    Patient = new Patient {FirstName = "Keith", LastName = "Kelley"},
                    FirstCareAt = new DateTime(2017, 09, 17),
                    Status = StatusEnum.refused
                },
                new Ordonnance
                {
                    Reference = 5,
                    Patient = new Patient {FirstName = "Justin", LastName = "Sims"},
                    FirstCareAt = new DateTime(2017, 03, 04),
                    Status = StatusEnum.valid
                },
                new Ordonnance
                {
                    Reference = 6,
                    Patient = new Patient {FirstName = "Franklin", LastName = "Howard"},
                    FirstCareAt = new DateTime(2017, 09, 29),
                    Status = StatusEnum.waiting
                },
                new Ordonnance
                {
                    Reference = 8,
                    Patient = new Patient {FirstName = "Mickael", LastName = "Green"},
                    FirstCareAt = new DateTime(2017, 09, 29),
                    Status = StatusEnum.waiting
                },
                new Ordonnance
                {
                    Reference = 9,
                    Patient = new Patient {FirstName = "Paul", LastName = "Howard"},
                    FirstCareAt = new DateTime(2017, 09, 29),
                    Status = StatusEnum.valid
                },
                new Ordonnance
                {
                    Reference = 10,
                    Patient = new Patient {FirstName = "Justin", LastName = "Howard"},
                    FirstCareAt = new DateTime(2017, 10, 29),
                    Status = StatusEnum.waiting
                },
                new Ordonnance
                {
                    Reference = 11,
                    Patient = new Patient {FirstName = "John", LastName = "Obama"},
                    FirstCareAt = new DateTime(2017, 09, 29),
                    Status = StatusEnum.waiting
                }
            };

            InitializeComponent();

            MyListView.ItemsSource = Ordonnances;

            MessagingCenter.Subscribe<FilterPage, Filter>(this, Events.UpdatePrescriptionFilters, (sender, filter) =>
            {
                _filter = filter;

                ApplyFilter(filter);
            });
        }

        private void ApplyFilter(Filter filter)
        {
            if (filter != null)
            {
                List<Ordonnance> filteredItems = new List<Ordonnance>();
                foreach (var status in filter.Statuses)
                {
                    var foundItems =
                        Ordonnances.Where(x => x.Status == status && x.FirstCareAt >= filter.StartDate &&
                                                                     x.FirstCareAt <= filter.EndDate);
                    filteredItems.AddRange(foundItems);
                }

                _filteredItems = new ObservableCollection<Ordonnance>(filteredItems);
                MyListView.ItemsSource = _filteredItems;
            }
            else
            {
                _filteredItems = null;

                MyListView.ItemsSource = Ordonnances;
            }
        }
        
        async void Filter_Clicked(object sender, EventArgs e)
        {
			await Navigation.PushModalAsync(new FilterPage("Ordonnance",_filter));
        }

        async void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
            }

            var item = e.SelectedItem as Ordonnance;
            await Navigation.PushModalAsync(new OrdonnanceDetailPage(item));

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
                        _filteredItems.Where(x => x.Patient.FullName.ToLower().Contains(e.NewTextValue.ToLower()));
                }
                else
                {
                    foundItems =
                        Ordonnances.Where(x => x.Patient.FullName.ToLower().Contains(e.NewTextValue.ToLower()));
                }
                MyListView.ItemsSource = foundItems;
            }
            else
            {
                MyListView.ItemsSource = _filteredItems ?? Ordonnances;
            }
        }
    }
}
