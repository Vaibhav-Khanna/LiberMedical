﻿using System;
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
        public ObservableCollection<Document> documents { get; set; }

        public DetailsPatientListPage(string typeDocument) : base(-1, 64, false)
        {

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

            documents = new ObservableCollection<Document>
                {
                    new Document {
                        Reference= 1,
                        AddDate= new DateTime(2017, 06, 07,10,30,00),
                    },
                    new Document {
                        Reference= 2,
                        AddDate= new DateTime(2017, 06, 15,10,30,00),
                    },
                    new Document {
                        Reference= 3,
                        AddDate= new DateTime(2017, 06, 18,10,30,00),
                    },
                    new Document {
                        Reference= 4,
                        AddDate= new DateTime(2017, 06, 29,10,30,00),
                    },
                    new Document {
                        Reference= 5,
                        AddDate= new DateTime(2017, 07, 01,10,30,00),
                    },
                    new Document {
                        Reference= 6,
                        AddDate= new DateTime(2017, 07, 02,10,30,00),
                    },
                    new Document {
                        Reference= 7,
                        AddDate= new DateTime(2017, 07, 03,10,30,00),
                    }
            };
            BindingContext = this;
            InitializeComponent();

        }

        async void Back_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        async void Edit_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new PatientDetailModify());
        }

        void Ordonnances_Tapped(object sender, System.EventArgs e)
        {
            stackDocument.IsVisible = false;
            stackOrdonnance.IsVisible = true;
            BoxViewOrdonnances.IsVisible = true;
            BoxViewDocuments.IsVisible = false;
            Label.Text = "+ Ajoutez une ordonnance";

        }

        void Documents_Tapped(object sender, System.EventArgs e)
        {
            stackDocument.IsVisible = true;
            stackOrdonnance.IsVisible = false;
            BoxViewDocuments.IsVisible = true;
            BoxViewOrdonnances.IsVisible = false;

            Label.Text = "+ Ajoutez un document";

        }
        async void AddOrdonnance_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new OrdonnanceDetailEditPage());
        }
        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
    }
}
