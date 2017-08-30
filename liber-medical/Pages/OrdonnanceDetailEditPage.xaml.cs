using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using libermedical.CustomControls;
using libermedical.Enums;
using libermedical.Models;
using Xamarin.Forms;

namespace libermedical.Pages
{
    public partial class OrdonnanceDetailEditPage : BasePage
    {
        public Ordonnance Ordonnance { get; set; }

        public ObservableCollection<Cotation> cotation { get; set; }

        public OrdonnanceDetailEditPage() : base(-1, 64, false)
        {
            Ordonnance = new Ordonnance
            {
                AddDate = DateTime.Today
            };

            BindingContext = this;
            cotation = new ObservableCollection<Cotation>
                {
                new Cotation {
                    FirstCode=1,
                    SecondCode= "AMI",
                    ThirdCode= 2
                },
                new Cotation {
                    FirstCode=2,
                    SecondCode= "AMI",
                    ThirdCode= 1
                },
                new Cotation {
                    FirstCode=3,
                    SecondCode= "AMK",
                    ThirdCode= 5
                }
            };

            InitializeComponent();

            MyDatePicker.Date = DateTime.Today;
            MyDatePicker.DateSelected += MyDatePickerOnDateSelected;
        }

        private void MyDatePickerOnDateSelected(object sender, DateChangedEventArgs dateChangedEventArgs)
        {
            //
        }

        async void Frequence_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new OrdonnanceFrequencePage());
        }

        async void Cancel_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        async void Save_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void StatusCell_Tapped(object sender, EventArgs e)
        {
            var status = await DisplayActionSheet("Select status", "Annuler", null, "En attente", "Traité", "Refusé");
            if (status != null && status != "Annuler")
            {
                switch (status)
                {
                    case "En attente":
                        Ordonnance.Status = StatusEnum.Attente;
                        break;
                    case "Traité":
                        Ordonnance.Status = StatusEnum.Traite;
                        break;
                    case "Refusé":
                        Ordonnance.Status = StatusEnum.Refuse;
                        break;
                }

                StatusLabel.Text = "Statut: " + status;
            }
        }

        private void DatePicker_Tapped(object sender, EventArgs e)
        {
            MyDatePicker.Focus();
        }
    }
}
