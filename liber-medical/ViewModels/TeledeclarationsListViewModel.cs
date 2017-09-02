using libermedical.Models;
using libermedical.Pages;
using libermedical.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace libermedical.ViewModels
{
	public class TeledeclarationsListViewModel: ViewModelBase
	{
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
            }
        }

        public TeledeclarationsListViewModel()
        {
            BindData();
        }

        private async void BindData()
        {
            Teledeclarations = new ObservableCollection<Teledeclaration>
            {
                new Teledeclaration {
                    Reference= 1,
                    AddDate= new DateTime(2017, 04, 02),
                    TotalAccount= 92.76,
                    Status = "Traité"
                },
                new Teledeclaration {
                    Reference= 1,
                    AddDate= new DateTime(2017, 11, 08),
                    TotalAccount= 67.64,
                    Status = "Traité"
                },
                new Teledeclaration {
                    Reference= 1,
                    AddDate= new DateTime(2017, 04, 02),
                    TotalAccount= 92.76,
                    Status = "Traité"
                },
                new Teledeclaration {
                    Reference= 1,
                    AddDate= new DateTime(2017, 04, 02),
                    TotalAccount= 92.76,
                    Status = "Traité"
                },
                new Teledeclaration {
                    Reference= 1,
                    AddDate= new DateTime(2017, 04, 02),
                    TotalAccount= 92.76,
                    Status = "Traité"
                },
                new Teledeclaration {
                    Reference= 1,
                    AddDate= new DateTime(2017, 05, 24),
                    TotalAccount= 92.76,
                    Status = "Traité"
                },
                new Teledeclaration {
                    Reference= 1,
                    AddDate= new DateTime(2016, 07, 02),
                    TotalAccount= 92.76,
                    Status = "Traité"
                },
                new Teledeclaration {
                    Reference= 1,
                    AddDate= new DateTime(2017, 02, 11),
                    TotalAccount= 92.76,
                    Status = "Traité"
                },
                new Teledeclaration {
                    Reference= 1,
                    AddDate= new DateTime(2017, 03, 30),
                    TotalAccount= 92.76,
                    Status = "Traité"
                },
                new Teledeclaration {
                    Reference= 1,
                    AddDate= new DateTime(2017, 10, 22),
                    TotalAccount= 12.64,
                    Status = "Traité"
                },
                new Teledeclaration {
                    Reference= 1,
                    AddDate= new DateTime(2017, 08, 02),
                    TotalAccount= 145.32,
                    Status = "Traité"
                }
            };
        }

        public ICommand BillTappedCommand => new Command( 
            async ()=> await Application.Current.MainPage.Navigation.PushModalAsync(new SecuriseBillsPage()));
    }
}
