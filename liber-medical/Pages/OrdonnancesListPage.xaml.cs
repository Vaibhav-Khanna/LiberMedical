using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using libermedical.CustomControls;
using libermedical.Enums;
using libermedical.Models;
using libermedical.Utility;
using libermedical.ViewModels;
using Plugin.Media;
using Xamarin.Forms;

namespace libermedical.Pages
{
    public partial class OrdonnancesListPage : BasePage
    {
        public ObservableCollection<Ordonnance> Ordonnances { get; set; }
        private ObservableCollection<Ordonnance> _filteredItems { get; set; }

        private Filter _filter;

        public OrdonnancesListPage() : base(0, 0)
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
        
        async void Filter_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new FilterPage(_filter));
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

        async void Add_Clicked(object sender, System.EventArgs e)
        {
            var action = await DisplayActionSheet(null, "Annuler", null, "Ordonnance rapide", "Ordonnance classique");
            var typeDoc = "ordonnance";
            string typeNavigation = "";

            if (action != null && action != "Annuler")
            {
                if (action == "Ordonnance rapide")
                {
                    //init du plugin photo 
                    await CrossMedia.Current.Initialize();

                    if (UtilityClass.CameraAvailable())
                    {
                        var file = await CrossMedia.Current.TakePhotoAsync(
                            new Plugin.Media.Abstractions.StoreCameraMediaOptions()
                            {
                                AllowCropping = true
                            });

                        if (file != null)
                        {
                            var profilePicture = ImageSource.FromStream(() => file.GetStream());

                            if (action == "Ordonnance rapide")
                            {
                                typeNavigation = "fast";
                            }
                            if (action == "Ordonnance classique")
                            {
                                typeNavigation = "normal";
                            }

                            await Navigation.PushModalAsync(new PatientListPage(typeNavigation, typeDoc));
                        }
                    }
                    else
                    {
                        await DisplayAlert("No Camera", ":( No camera available.", "OK");
                    }
                }
                else
                {
                    (BindingContext as OrdonnancesListViewModel).CreatePrescription();
                    //await Navigation.PushModalAsync(new OrdonnanceDetailEditPage());
                }
            }
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
