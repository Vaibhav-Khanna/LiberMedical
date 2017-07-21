using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using libermedical.CustomControls;
using libermedical.Models;
using Xamarin.Forms;

namespace libermedical.Views
{
    public partial class PatientsListPage : BasePage
    {
        public ObservableCollection<Patient> patients { get; set; }
        private string navigationAfter;
        private string typeDoc;
        public bool visibility;

        public PatientsListPage() : base(0, 0)
        {

            //ToolItemBarConstruction because we arrive from TabbedPage Directely

            var Item1 = new ToolbarItem
            {
                Icon = "plus.png",
                Order = ToolbarItemOrder.Primary,
            };
            Item1.Clicked += AddUser_Clicked;
            ToolbarItems.Add(Item1);
            Title = "Patients";



            patients = new ObservableCollection<Patient>
            {
                new Patient {
                    FirstName= "Bill",
                    LastName="Anderson"
                },
                new Patient {
                    FirstName= "Milton",
                    LastName="Aaron"
                },
                new Patient {
                    FirstName= "Reid",
                    LastName="Alex"
                },

                new Patient {
                    FirstName= "Fred",
                    LastName="Alpina"
                },

                new Patient {
                    FirstName= "Fred",
                    LastName="Hojberg"
                },

                new Patient {
                    FirstName= "Bruce",
                    LastName="Ballard"
                },
                new Patient {
                    FirstName= "Alex",
                    LastName="Bartley"
                },
                new Patient {
                    FirstName= "Michael",
                    LastName="Jordan"
                },
                new Patient {
                    FirstName= "Magic",
                    LastName="Johnson"
                },
                new Patient {
                    FirstName= "Bill",
                    LastName="Russell"
                },
                new Patient {
                    FirstName= "James",
                    LastName="Harden"
                },
                new Patient {
                    FirstName= "Russell",
                    LastName="Westbrook"
                },
                new Patient {
                    FirstName= "Kevin",
                    LastName="Durant"
                },
                new Patient {
                    FirstName= "Shaquil",
                    LastName="O neill"
                },
                new Patient {
                    FirstName= "Lebron",
                    LastName="James"
                },
                new Patient {
                    FirstName= "Derrick",
                    LastName="Rose"
                },
                new Patient {
                    FirstName= "Mike",
                    LastName="Dantoni"
                },
                new Patient {
                    FirstName= "Chris",
                    LastName="Paul"
                },
                new Patient {
                    FirstName= "Bill",
                    LastName="Murray"
                },
                new Patient {
                    FirstName= "Jason",
                    LastName="Kid"
                },
                new Patient {
                    FirstName= "John",
                    LastName="Stockton"
                }

            };
            BindingContext = this;
            InitializeComponent();

        }
        public PatientsListPage(string navigationType, string typeDoc) : base(-1, 0, false)
        {

            this.navigationAfter = navigationType;
            this.typeDoc = typeDoc;

            BindingContext = this;
            patients = new ObservableCollection<Patient>
            {
                new Patient {
                    FirstName= "Bill",
                    LastName="Anderson"
                },
                new Patient {
                    FirstName= "Milton",
                    LastName="Aaron"
                },
                new Patient {
                    FirstName= "Reid",
                    LastName="Alex"
                },

                new Patient {
                    FirstName= "Fred",
                    LastName="Alpina"
                },

                new Patient {
                    FirstName= "Fred",
                    LastName="Hojberg"
                },

                new Patient {
                    FirstName= "Bruce",
                    LastName="Ballard"
                },
                new Patient {
                    FirstName= "Alex",
                    LastName="Bartley"
                },
                new Patient {
                    FirstName= "Michael",
                    LastName="Jordan"
                },
                new Patient {
                    FirstName= "Magic",
                    LastName="Johnson"
                },
                new Patient {
                    FirstName= "Bill",
                    LastName="Russell"
                },
                new Patient {
                    FirstName= "James",
                    LastName="Harden"
                },
                new Patient {
                    FirstName= "Russell",
                    LastName="Westbrook"
                },
                new Patient {
                    FirstName= "Kevin",
                    LastName="Durant"
                },
                new Patient {
                    FirstName= "Shaquil",
                    LastName="O neill"
                },
                new Patient {
                    FirstName= "Lebron",
                    LastName="James"
                },
                new Patient {
                    FirstName= "Derrick",
                    LastName="Rose"
                },
                new Patient {
                    FirstName= "Mike",
                    LastName="Dantoni"
                },
                new Patient {
                    FirstName= "Chris",
                    LastName="Paul"
                },
                new Patient {
                    FirstName= "Bill",
                    LastName="Murray"
                },
                new Patient {
                    FirstName= "Jason",
                    LastName="Kid"
                }/*
                new Patient {
                    FirstName= "John",
                    LastName="Stockton"
                }*/

            };
            InitializeComponent();

        }

        async void AddUser_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new AddPatient()));
        }

        async void Back_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        void PatientTapped(object sender, System.EventArgs e)
        {
            //if fast mode
            if (this.navigationAfter == "fast")
            {

                if (this.typeDoc == "ordonnance")
                {
                    //save ordonnance
                    Navigation.PopModalAsync();
                }
                if (this.typeDoc == "document")
                {
                    //save document
                    Navigation.PushModalAsync(new NavigationPage(new AddDocument()));
                }



            }
            //if normal mode it's an ordoannce 
            else if (this.navigationAfter == "normal")
            {
                Navigation.PushAsync(new OrdonnanceDetailEditPage());
            }

            // if patient list navigation from icon menu

            else
            {
                var typeDocument = "ordonnances";
                Navigation.PushModalAsync(new NavigationPage(new DetailsPatientListPage(typeDocument)));
            }
        }

        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
    }
}