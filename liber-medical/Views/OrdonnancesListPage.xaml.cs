using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using libermedical.CustomControls;
using libermedical.Models;
using Xamarin.Forms;

namespace libermedical.Views
{
    public partial class OrdonnancesListPage : BasePage
    {
        public ObservableCollection<Ordonnance> ordonnances { get; set; }

        public OrdonnancesListPage() : base(0, 0)
        {

            BindingContext = this;
            ordonnances = new ObservableCollection<Ordonnance>
            {
                new Ordonnance {
                    Reference= 1,
                    Patient= new Patient {FirstName= "Fred",LastName="Pearson"},
                    AddDate= new DateTime(2017, 04, 02),
                    Status = "Traité"
                },
                new Ordonnance {
                    Reference= 2,
                    Patient= new Patient {FirstName= "Jonanthan",LastName="Vaughn"},
                    AddDate= new DateTime(2017, 11, 10),
                    Status = "Traité"
                },
                new Ordonnance {
                    Reference= 3,
                    Patient= new Patient {FirstName= "Alexander",LastName="Zimmerman"},
                    AddDate= new DateTime(2017, 02, 06),
                    Status = "Refusé"
                },
                new Ordonnance {
                    Reference= 4,
                    Patient= new Patient {FirstName= "Keith",LastName="Kelley"},
                    AddDate= new DateTime(2017, 09, 17),
                    Status = "Refusé"
                },
                new Ordonnance {
                    Reference= 5,
                    Patient= new Patient {FirstName= "Justin",LastName="Sims"},
                    AddDate= new DateTime(2017, 03, 04),
                    Status = "Traité"
                },
                new Ordonnance {
                    Reference= 6,
                    Patient= new Patient {FirstName= "Franklin",LastName="Howard"},
                    AddDate= new DateTime(2017, 09, 29),
                    Status = "En attente"
                },
                new Ordonnance {
                    Reference= 7,
                    Patient= new Patient {FirstName= "Franklin",LastName="Howard"},
                    AddDate= new DateTime(2017, 09, 29),
                    Status = "Refusé"
                },
                new Ordonnance {
                    Reference= 8,
                    Patient= new Patient {FirstName= "Mickael",LastName="Green"},
                    AddDate= new DateTime(2017, 09, 29),
                    Status = "En attente"
                },
                new Ordonnance {
                    Reference= 9,
                    Patient= new Patient {FirstName= "Paul",LastName="Howard"},
                    AddDate= new DateTime(2017, 09, 29),
                    Status = "Traité"
                },
                new Ordonnance {
                    Reference= 10,
                    Patient= new Patient {FirstName= "Justin",LastName="Howard"},
                    AddDate= new DateTime(2017, 10, 29),
                    Status = "En attente"
                },
                new Ordonnance {
                    Reference= 11,
                    Patient= new Patient {FirstName= "John",LastName="Obama"},
                    AddDate= new DateTime(2017, 09, 29),
                    Status = "En attente"
                }
            };


            InitializeComponent();
        }


    }

}
