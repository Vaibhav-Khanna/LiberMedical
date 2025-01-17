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
        public int MaxCount { get; set; } = 0;
        private IStorageService<Teledeclaration> _teledeclarationsStorage;
        public Filter _filter;
        public ObservableCollection<Teledeclaration> _teledeclarationsAll;
        public List<Teledeclaration> FilteredItems;

        string filteracctivetext = string.Empty;
        public string FilterActiveText
        {
            get { return filteracctivetext; }
            set
            {
                filteracctivetext = value;
                RaisePropertyChanged();
            }
        }

        string noresulttext = string.Empty;
        public string NoResultText
        {
            get { return noresulttext; }
            set
            {
                noresulttext = value;
                RaisePropertyChanged();
            }
        }

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
           
            MessagingCenter.Unsubscribe<FilterPage, Filter>(this, Events.UpdateTeledeclarationsFilters);
            MessagingCenter.Subscribe<FilterPage, Filter>(this, Events.UpdateTeledeclarationsFilters, (sender, filter) =>
            {
                _filter = filter;
                ApplyFilter(filter);
            });
                      
        }

        //private async Task DownlaodDocuments()
        //{
        //    if (App.IsConnected())
        //    {
        //        var request = new GetListRequest(600, 0);
        //        var documents =
        //            new ObservableCollection<Document>((await App.DocumentsManager.GetListAsync(request)).rows);

        //        //Updating records in local cache
        //        await new StorageService<Document>().InvalidateSyncedItems();
        //        await new StorageService<Document>().AddManyAsync(documents.ToList());
        //    }
        //}



        public void ApplyFilter(Filter filter)
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
                         
                        NoResultText = null;

                        FilterActiveText = null;
                       
                        FilteredItems = Teledeclarations.ToList();

                        return;
                    }
                }

                if (filteredItems.Any())
                    NoResultText = null;
                else
                    NoResultText = "Aucun résultat";

                FilterActiveText = "Attention des filtres sont actifs";

                FilteredItems = filteredItems;

                Teledeclarations = new ObservableCollection<Teledeclaration>(filteredItems);

            }
            else
            {
                FilterActiveText = null;
              
                if (!_teledeclarationsAll.Any())
                    NoResultText = "Aucun résultat";
                else
                    NoResultText = null;

                FilteredItems = _teledeclarationsAll.ToList();

                Teledeclarations = _teledeclarationsAll;
            }
        }

        public async Task BindData()
        {
            MaxCount = await new StorageService<Teledeclaration>().DownloadTeledeclarations();

            var list = await _teledeclarationsStorage.GetList();

            if (list != null && list.Count() != 0)
            {
                list = list.OrderByDescending((arg) => arg.CreatedAt);
                list = list.DistinctBy( (arg) => arg.Id); 
            }

            Teledeclarations = new ObservableCollection<Teledeclaration>(list);

            _teledeclarationsAll = Teledeclarations;

            ApplyFilter(_filter);         
        }

      
        bool isOpening = false;



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

        public async override void Init(object initData)
        {
            base.Init(initData);

            var list = await _teledeclarationsStorage.GetList();

            if (list != null && list.Count() != 0)
            {
                list = list.OrderByDescending((arg) => arg.CreatedAt);
                list = list.DistinctBy( (arg) => arg.Id); 
            }

            Teledeclarations = new ObservableCollection<Teledeclaration>(list);

            _teledeclarationsAll = Teledeclarations;

            IsRefreshing = true;

            await BindData();

            IsRefreshing = false;
        }

        public override void ReverseInit(object returnedData)
        {
            base.ReverseInit(returnedData);

            if(returnedData is string)
            {

            }
        }

        protected override Task TapCommandFunc(Cell cell)
        {
            throw new NotImplementedException();
        }

        bool isopening = false;

        public ICommand FilterTappedCommand => new Command(
            async () =>
            {
               if (isopening)
                    return;
               
               isopening = true;

            await Application.Current.MainPage.Navigation.PushModalAsync(new FilterPage("Teledeclarations", _filter), Device.RuntimePlatform == Device.iOS );
             
                isopening = false;
            });

        public ICommand OpenInvoiceToSecuriseCommand
        {
            get
            {
                return new Command(
                async (args) =>
                {

                    UserDialogs.Instance.ShowLoading("ouverture");

                    await Task.Delay(500);

                    var request = new GetListRequest(20, 1, sortField: "createdAt", sortDirection: SortDirectionEnum.Desc);
                    var invoices = await App.InvoicesManager.GetListAsync(request);

                    if (invoices != null && invoices.rows != null && invoices.rows.Count > 0)
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
