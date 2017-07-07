using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using libermedical.CustomControls;
using libermedical.Models;
using Xamarin.Forms;

namespace libermedical.Views
{
    public partial class DetailsPatientListPage : BasePage
    {

        public ObservableCollection<Ordonnance> ordonnances { get; set; }

        public DetailsPatientListPage() : base(-1, 64)
        {

            BindingContext = this;
            ordonnances = new ObservableCollection<Ordonnance>
            {
                new Ordonnance {
                    Reference= 1,
                    AddDate= new DateTime(2017, 06, 07,10,30,00),
                    Status = "Traité"
                },
                new Ordonnance {
                    Reference= 2,
                    AddDate= new DateTime(2017, 06, 15,10,30,00),

                    Status = "Traité"
                },
                new Ordonnance {
                    Reference= 3,
                    AddDate= new DateTime(2017, 06, 18,10,30,00),
                    Status = "Refusé"
                },
                new Ordonnance {
                    Reference= 4,
                    AddDate= new DateTime(2017, 06, 29,10,30,00),
                    Status = "Refusé"
                },
                new Ordonnance {
                    Reference= 5,
                    AddDate= new DateTime(2017, 07, 01,10,30,00),
                    Status = "Traité"
                },
                new Ordonnance {
                    Reference= 6,
                    AddDate= new DateTime(2017, 07, 02,10,30,00),
                    Status = "En attente"
                },
                new Ordonnance {
                    Reference= 7,
                    AddDate= new DateTime(2017, 07, 03,10,30,00),
                    Status = "En attente"
                }
            };

            InitializeComponent();
        }

        void Back_Tapped(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new PatientsListPage());
        }

        void Edit_Tapped(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new LoginPage());
        }
    }
}
