﻿using libermedical.Models;
using libermedical.Pages;
using libermedical.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using libermedical.Enums;
using System.Collections.Generic;
using libermedical.Helpers;
using System.Linq;
using libermedical.Request;
using libermedical.Services;
using System.Threading.Tasks;
using Acr.UserDialogs;
using libermedical.PopUp;

namespace libermedical.ViewModels
{
    public class TeledeclarationsListViewModel : ListViewModelBase<Teledeclaration>
    {
        int _initCount = 0;
        public int MaxCount { get; set; }
        private IStorageService<Teledeclaration> _teledeclarationsStorage;
        private Filter _filter;
        private ObservableCollection<Teledeclaration> _teledeclarationsAll;
        private ObservableCollection<Teledeclaration> _teledeclarations;
        public ObservableCollection<Teledeclaration> Teledeclarations
        {
            get { return _teledeclarations; }
            set
            {
                _teledeclarations = value;
                RaisePropertyChanged();
            }
        }

        private Teledeclaration _selectedTeledeclaration;
        public Teledeclaration SelectedTeledeclaration
        {
            get { return _selectedTeledeclaration; }
            set
            {
                _selectedTeledeclaration = value;
                RaisePropertyChanged();

                if (value != null)
                    TeledeclarationTappedCommand.Execute(_selectedTeledeclaration);
            }
        }

        public TeledeclarationsListViewModel(IStorageService<Teledeclaration> storageService) : base(storageService)
        {
            _teledeclarationsStorage = storageService;
            BindData(20);
        }

        private async Task DownlaodDocuments()
        {
            if (App.IsConnected())
            {
                var request = new GetListRequest(600, 0);
                var documents =
                    new ObservableCollection<Document>((await App.DocumentsManager.GetListAsync(request)).rows);

                //Updating records in local cache
                await new StorageService<Document>().InvalidateSyncedItems();
                await new StorageService<Document>().AddManyAsync(documents.ToList());
            }
        }
        private void ApplyFilter(Filter filter)
        {
            if (filter != null)
            {
                List<Teledeclaration> filteredItems = new List<Teledeclaration>();
                List<Teledeclaration> foundItems = new List<Teledeclaration>();

                if (filter.Statuses.Count > 0)
                {
                    foreach (var status in filter.Statuses)
                    {
                        if (filter.EnableDateSearch)
                        {
                            foundItems =
                                _teledeclarationsAll.Where(x => x.Status == status.ToString() && x.CreatedAt.Value.Date >= filter.StartDate.Value.Date &&
                                                          x.CreatedAt.Value.Date <= filter.EndDate.Value.Date).ToList();
                        }
                        else
                        {
                            foundItems =
                                _teledeclarationsAll.Where(x => x.Status == status.ToString()).ToList();
                        }
                        filteredItems.AddRange(foundItems);
                    }

                }
                else
                {
                    if (filter.EnableDateSearch)
                    {
                        foundItems =
                               _teledeclarationsAll.Where(x => x.CreatedAt.Value.Date >= filter.StartDate.Value.Date &&
                                                          x.CreatedAt.Value.Date <= filter.EndDate.Value.Date).ToList();
                        filteredItems.AddRange(foundItems);
                    }
                    else
                    {
                        Teledeclarations = _teledeclarationsAll;
                        return;
                    }
                }
                Teledeclarations = new ObservableCollection<Teledeclaration>(filteredItems);
            }
            else
            {
                Teledeclarations = _teledeclarationsAll;
            }
        }

        public async Task BindData(int count)
        {
            _initCount = _initCount + count;

            MaxCount = await new StorageService<Teledeclaration>().DownloadTeledeclarations(_initCount);

            var list = await _teledeclarationsStorage.GetList();

            if (list != null && list.Count() != 0)
            {
                list = list.OrderByDescending((arg) => arg.CreatedAt);
                list = list.DistinctBy( (arg) => arg.Id); 
            }
            Teledeclarations = new ObservableCollection<Teledeclaration>(list);

            _teledeclarationsAll = Teledeclarations;

            MessagingCenter.Unsubscribe<FilterPage, Filter>(this, Events.UpdateTeledeclarationsFilters);
            MessagingCenter.Subscribe<FilterPage, Filter>(this, Events.UpdateTeledeclarationsFilters, (sender, filter) =>
           {
               _filter = filter;
               ApplyFilter(filter);
           });

           await DownlaodDocuments();

        }

        protected override async Task TapCommandFunc(Cell cell)
        {
            //var ctx = cell.BindingContext;
            //await CoreMethods.PushPageModelWithNewNavigation<TeledeclarationSecureActionViewModel>(ctx);
        }

        bool isOpening = false;

        public ICommand BillTappedCommand => new Command(
            async () => await Application.Current.MainPage.Navigation.PushModalAsync(new SecuriseBillsPage()));

        public ICommand TeledeclarationTappedCommand
        {
            get
            {
                return new Command(
                async (args) =>
                {
                    if (isOpening)
                        return;
                    
                    isOpening = true;

                    await CoreMethods.PushPageModel<TeledeclarationSecureActionViewModel>(null, true, true);
                    MessagingCenter.Send(this, Events.UpdateTeledeclarationDetailsPage, args as Teledeclaration);
                
                    isOpening = false;
                
                });
            }
        }

        protected override async void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);

            var list = await _teledeclarationsStorage.GetList();

            if (list != null && list.Count() != 0)
            {
                list = list.OrderByDescending((arg) => arg.CreatedAt);
            }

            Teledeclarations = new ObservableCollection<Teledeclaration>(list);
        }

        public ICommand FilterTappedCommand => new Command(
            async () =>
            {
                await Application.Current.MainPage.Navigation.PushModalAsync(new FilterPage("Teledeclarations", _filter));
            });

        public ICommand OpenInvoiceToSecuriseCommand
        {
            get
            {
                return new Command(
                async (args) =>
                {
                    
                    UserDialogs.Instance.ShowLoading("ouverture");
                    var request = new GetListRequest(20, 1,sortField:"createdAt",sortDirection: SortDirectionEnum.Desc);
                    var invoices = await App.InvoicesManager.GetListAsync(request);
                    if (invoices != null && invoices.rows!=null && invoices.rows.Count > 0)
                    {
                        var invoice = invoices.rows.First();
                        if (invoice.FilePath.Contains(".pdf"))
                            await CoreMethods.PushPageModel<SecuriseBillsViewModel>(invoice, true);
                        else
                            await CoreMethods.PushPageModel<OrdonnanceViewViewModel>(invoice, true);
                    }
                    UserDialogs.Instance.HideLoading();

                });
            }
        }
    }
}
