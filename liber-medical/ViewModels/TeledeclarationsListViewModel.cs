using libermedical.Models;
using libermedical.Pages;
using libermedical.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using libermedical.Enums;
using System.Collections.Generic;
using System.Linq;
using libermedical.Request;
using libermedical.Services;
using System.Threading.Tasks;

namespace libermedical.ViewModels
{
	public class TeledeclarationsListViewModel : ListViewModelBase<Teledeclaration>
	{
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
			BindData();
		}

		private void ApplyFilter(Filter filter)
		{
			if (filter != null && filter.IsActivated)
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
							   _teledeclarationsAll.Where(x => x.Status == status && x.CreatedAt >= filter.StartDate &&
														  x.CreatedAt <= filter.EndDate).ToList();
						}
						else
						{
							foundItems =
							   _teledeclarationsAll.Where(x => x.Status == status).ToList();
						}
						filteredItems.AddRange(foundItems);
					}

				}
				else
				{
					if (filter.EnableDateSearch)
					{
						foundItems =
							   _teledeclarationsAll.Where(x => x.CreatedAt >= filter.StartDate &&
														  x.CreatedAt <= filter.EndDate).ToList();
						filteredItems.AddRange(foundItems);
					}
				}
				Teledeclarations = new ObservableCollection<Teledeclaration>(filteredItems);
			}
			else
			{
				Teledeclarations = _teledeclarationsAll;
			}
		}

		private async void BindData()
		{
			if (App.IsConnected())
			{
				var request = new GetListRequest(20, 0);
				Teledeclarations =
					new ObservableCollection<Teledeclaration>((await App.TeledeclarationsManager.GetListAsync(request)).rows);

				//Updating records in local cache
				await _teledeclarationsStorage.DeleteAllAsync();
				await _teledeclarationsStorage.AddManyAsync(Teledeclarations.ToList());
			}
			else
			{
				Teledeclarations = new ObservableCollection<Teledeclaration>(await _teledeclarationsStorage.GetList());
			}
			_teledeclarationsAll = Teledeclarations;
			MessagingCenter.Subscribe<FilterPage, Filter>(this, Events.UpdatePrescriptionFilters, (sender, filter) =>
		   {
			   _filter = filter;
			   ApplyFilter(filter);
		   });
		}

		protected override async Task TapCommandFunc(Cell cell)
		{
			//var ctx = cell.BindingContext;
			//await CoreMethods.PushPageModelWithNewNavigation<TeledeclarationSecureActionViewModel>(ctx);
		}

		public ICommand BillTappedCommand => new Command(
			async () => await Application.Current.MainPage.Navigation.PushModalAsync(new SecuriseBillsPage()));

		public ICommand TeledeclarationTappedCommand
		{
			get
			{
				return new Command(
				async (args) =>
				{
                    await CoreMethods.PushPageModel<TeledeclarationSecureActionViewModel>(null,true,true);
					MessagingCenter.Send(this, Events.UpdateTeledeclarationDetailsPage, args as Teledeclaration);
				});
			}
		}


		public ICommand FilterTappedCommand => new Command(
			async () =>
			{
				await Application.Current.MainPage.Navigation.PushModalAsync(new FilterPage("Teledeclarations", _filter));
			});

	}
}
