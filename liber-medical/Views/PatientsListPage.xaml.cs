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



        public PatientsListPage() : base(0, 0)
        {
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
                },
                new Patient {
                    FirstName= "John",
                    LastName="Stockton"
                }

            };

            InitializeComponent();

        }
        public PatientsListPage(string navigationType) : base(0, 0)
        {

            this.navigationAfter = navigationType;
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

        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            if (this.navigationAfter == "fast")
            {
                Navigation.PushAsync(new MainTabPage());

            }
        }
    }
}