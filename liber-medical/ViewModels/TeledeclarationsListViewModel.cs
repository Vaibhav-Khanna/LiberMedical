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

namespace libermedical.ViewModels
{
	public class TeledeclarationsListViewModel : ViewModelBase
	{
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
					TeledeclarationTappedCommand.Execute(null);
			}
		}

		public TeledeclarationsListViewModel()
		{
			BindData();
		}

		private void ApplyFilter(Filter filter)
		{
			if (filter != null)
			{
				List<Teledeclaration> filteredItems = new List<Teledeclaration>();
				foreach (var status in filter.Statuses)
				{
					var foundItems =
						_teledeclarationsAll.Where(x => x.Status == status && x.AddDate >= filter.StartDate &&
																	 x.AddDate <= filter.EndDate);
					filteredItems.AddRange(foundItems);
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
			Teledeclarations = new ObservableCollection<Teledeclaration>
			{
				new Teledeclaration {
					Reference= 1,
					AddDate= new DateTime(2017, 04, 02),
					TotalAccount= 92.76,
					Status = StatusEnum.waiting
				},
				new Teledeclaration {
					Reference= 1,
					AddDate= new DateTime(2017, 11, 08),
					TotalAccount= 67.64,
					Status = StatusEnum.waiting
				},
				new Teledeclaration {
					Reference= 1,
					AddDate= new DateTime(2017, 04, 02),
					TotalAccount= 92.76,
					Status = StatusEnum.refused
				},
				new Teledeclaration {
					Reference= 1,
					AddDate= new DateTime(2017, 04, 02),
					TotalAccount= 92.76,
					Status = StatusEnum.waiting
				},
				new Teledeclaration {
					Reference= 1,
					AddDate= new DateTime(2017, 04, 02),
					TotalAccount= 92.76,
					Status = StatusEnum.refused
				},
				new Teledeclaration {
					Reference= 1,
					AddDate= new DateTime(2017, 05, 24),
					TotalAccount= 92.76,
					Status = StatusEnum.refused
				},
				new Teledeclaration {
					Reference= 1,
					AddDate= new DateTime(2016, 07, 02),
					TotalAccount= 92.76,
					Status = StatusEnum.waiting
				},
				new Teledeclaration {
					Reference= 1,
					AddDate= new DateTime(2017, 02, 11),
					TotalAccount= 92.76,
					Status = StatusEnum.waiting
				},
				new Teledeclaration {
					Reference= 1,
					AddDate= new DateTime(2017, 03, 30),
					TotalAccount= 92.76,
					Status = StatusEnum.waiting
				},
				new Teledeclaration {
					Reference= 1,
					AddDate= new DateTime(2017, 10, 22),
					TotalAccount= 12.64,
					Status = StatusEnum.waiting
				},
				new Teledeclaration {
					Reference= 1,
					AddDate= new DateTime(2017, 08, 02),
					TotalAccount= 145.32,
					Status = StatusEnum.waiting
				}
			};
			_teledeclarationsAll = Teledeclarations;
			MessagingCenter.Subscribe<FilterPage, Filter>(this, Events.UpdatePrescriptionFilters, (sender, filter) =>
		   {
			   _filter = filter;

			   ApplyFilter(filter);
		   });
		}

		public ICommand BillTappedCommand => new Command(
			async () => await Application.Current.MainPage.Navigation.PushModalAsync(new SecuriseBillsPage()));

		public ICommand TeledeclarationTappedCommand => new Command(
			async () => await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new TeledeclarationSecureActionPage())));

		public ICommand FilterTappedCommand => new Command(
			async () =>
			{
				await Application.Current.MainPage.Navigation.PushModalAsync(new FilterPage("Teledeclarations", _filter));
			});

	}
}
