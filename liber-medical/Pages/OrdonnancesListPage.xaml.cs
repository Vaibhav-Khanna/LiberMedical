using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using libermedical.Models;
using libermedical.Services;
using libermedical.ViewModels;
using Xamarin.Forms;

namespace libermedical.Pages
{
    public partial class OrdonnancesListPage
    {
        private int _previousCount = 0;
        private ObservableCollection<Ordonnance> _filteredItems { get; set; }

        private Filter _filter;

        public OrdonnancesListPage()
        {
            InitializeComponent();

            MessagingCenter.Unsubscribe<StorageService<BaseDTO>>(this, "RefreshOrdoList");
            MessagingCenter.Subscribe<StorageService<BaseDTO>>(this,"RefreshOrdoList", (obj) => 
            {
                if(MyListView!=null)
                 MyListView.BeginRefresh();
            });

            MessagingCenter.Subscribe<LibermedicalTabbedNavigation,int>(this,"PageChanged",(arg1, arg2) => 
            {
                if(arg2==2)
                {
                    MyListView.BeginRefresh();
                }
            });

            MessagingCenter.Subscribe<OrdonnanceCreateEditViewModel>(this, "RefreshOrdoList", (arg1 ) =>
            {
                MyListView.BeginRefresh();
            });


            MessagingCenter.Subscribe<FilterPage, Filter>(this, Events.UpdatePrescriptionFilters, (sender, filter) =>
            {
                _filter = filter;
                ApplyFilter(filter);
            });

            MessagingCenter.Subscribe<OrdonnancesListViewModel, Filter>(this, Events.UpdatePrescriptionFilters, (sender, filter) =>
            {
                _filter = filter;
                ApplyFilter(filter);
                MyListView.ItemsSource = (BindingContext as OrdonnancesListViewModel).Ordonnances;
            });
        }


        async void Handle_Refreshing(object sender, System.EventArgs e)
        {
            if(App.IsConnected())
                await App.SyncData();
          
            await (BindingContext as OrdonnancesListViewModel).BindData(0);
            MyListView.ItemsSource = (BindingContext as OrdonnancesListViewModel).Ordonnances;
            MyListView.IsRefreshing = false;
            isExecuting = false;
        }
              

        private void ApplyFilter(Filter filter)
        {
            if (filter != null)
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
                                (BindingContext as OrdonnancesListViewModel).Ordonnances.Where(x => x.Status == status.ToString() && ((DateTimeOffset)x.CreatedAt).Date >= filter.StartDate.Value.Date &&
                                                                                               ((DateTimeOffset)x.CreatedAt).Date <= filter.EndDate.Value.Date).ToList();
                        }
                        else
                        {
                            foundItems =
                                (BindingContext as OrdonnancesListViewModel).Ordonnances.Where(x => x.Status == status.ToString()).ToList();
                        }
                        filteredItems.AddRange(foundItems);
                    }
                }
                else
                {
                    

                    if (filter.EnableDateSearch)
                    {
                        foundItems =
                            (BindingContext as OrdonnancesListViewModel).Ordonnances.Where(x => ((DateTimeOffset)x.CreatedAt).Date >= filter.StartDate.Value.Date &&
                                                                                           ((DateTimeOffset)x.CreatedAt).Date <= filter.EndDate.Value.Date).ToList();
                        filteredItems.AddRange(foundItems);
                    }
                    else
                    {
                        _filteredItems = null;
                        MyListView.ItemsSource = (BindingContext as OrdonnancesListViewModel).Ordonnances;
                        return;
                    }
                }

                if (filteredItems.Any())
                    (BindingContext as OrdonnancesListViewModel).NoResultText = null;
                else
                    (BindingContext as OrdonnancesListViewModel).NoResultText = "Aucun résultat";


                _filteredItems = new ObservableCollection<Ordonnance>(filteredItems);
                MyListView.ItemsSource = _filteredItems;

                (BindingContext as OrdonnancesListViewModel).FilterActiveText = "Attention des filtres sont actifs";

            }
            else
            {
                (BindingContext as OrdonnancesListViewModel).FilterActiveText = null;
                (BindingContext as OrdonnancesListViewModel).NoResultText = null;

                _filteredItems = null;

                MyListView.ItemsSource = (BindingContext as OrdonnancesListViewModel).Ordonnances;
            }
        }

        void Handle_Remove_Clicked(object sender, System.EventArgs e)
        {
            var item = (sender as MenuItem).CommandParameter as Ordonnance;

            if(item!=null)
            {
                if(item.Status != Enums.StatusEnum.valid.ToString())
                {
                    (BindingContext as OrdonnancesListViewModel).DeleteOrdo.Execute(item.Id);
                }
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
                        MyListView.SetBinding(ListView.ItemsSourceProperty,"Ordonnances");
                        (BindingContext as OrdonnancesListViewModel).Ordonnances = (BindingContext as OrdonnancesListViewModel).Ordonnances;
                    });
                }

                MyListView.ItemsSource = _filteredItems ?? (BindingContext as OrdonnancesListViewModel).Ordonnances;
            }
        }

        bool isExecuting = false;

        private async void MyListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (isExecuting)
                return;
            
            isExecuting = true;

            if ((BindingContext as OrdonnancesListViewModel).Ordonnances.Count < (BindingContext as OrdonnancesListViewModel).MaxCount)
            {
                if (string.IsNullOrWhiteSpace(searchBar.Text))
                    indicator.IsVisible = true;
                else
                    indicator.IsVisible = false;
              
                var currentItem = e.Item as Ordonnance;
               
                var lastItem = (BindingContext as OrdonnancesListViewModel).Ordonnances[(BindingContext as OrdonnancesListViewModel).Ordonnances.Count - 1];

                if (currentItem == lastItem && string.IsNullOrWhiteSpace(searchBar.Text))
                {
                    await (BindingContext as OrdonnancesListViewModel).BindData(20);
                }

            }
            else
            {
                indicator.IsVisible = false;
            }

            isExecuting = false;
        }

    }
}
