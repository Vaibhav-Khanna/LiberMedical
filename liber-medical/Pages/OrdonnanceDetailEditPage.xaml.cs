using System;
using libermedical.Enums;
using Xamarin.Forms;
using libermedical.ViewModels;

namespace libermedical.Pages
{
    public partial class OrdonnanceDetailEditPage
    {
        public OrdonnanceDetailEditPage() : base(-1, 64, false)
        {
            InitializeComponent();

            MyDatePicker.Date = DateTime.Today;
            MyDatePicker.DateSelected += MyDatePickerOnDateSelected;

            MyEditor.Text = "Ecrivez quelque chose";
            MyEditor.TextColor = Color.Gray;
        }

        private void MyDatePickerOnDateSelected(object sender, DateChangedEventArgs dateChangedEventArgs)
        {
            (this.BindingContext as OrdonnanceDetailEditViewModel).Ordonnance.FirstCareAt = dateChangedEventArgs.NewDate;
        }

        async void Cancel_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void StatusCell_Tapped(object sender, EventArgs e)
        {
            var status = await DisplayActionSheet("Sélectionnez un statut", "Annuler", null, "En attente", "Traité", "Refusé");
            if (status != null && status != "Annuler")
            {
                switch (status)
                {
                    case "En attente":
                        (this.BindingContext as OrdonnanceDetailEditViewModel).Ordonnance.Status = StatusEnum.waiting;
                        break;
                    case "Traité":
                        (this.BindingContext as OrdonnanceDetailEditViewModel).Ordonnance.Status = StatusEnum.valid;
                        break;
                    case "Refusé":
                        (this.BindingContext as OrdonnanceDetailEditViewModel).Ordonnance.Status = StatusEnum.refused;
                        break;
                }

                StatusLabel.Text = "Statut: " + status;
            }
        }

        private void DatePicker_Tapped(object sender, EventArgs e)
        {
            MyDatePicker.Focus();
        }

        private void Patient_Tapped(object sender, EventArgs e)
        {
            (BindingContext as OrdonnanceDetailEditViewModel).SelectPatientCommand.Execute(sender);
        }

        private void Editor_Focused(object sender, FocusEventArgs e)
        {
            if (MyEditor.Text.Equals("Ecrivez quelque chose"))
            {
                MyEditor.Text = "";
                MyEditor.TextColor = Color.Black;
            }
        }

        private void Editor_Unfocused(object sender, FocusEventArgs e)
        {
            if (MyEditor.Text.Equals(""))
            {
                MyEditor.Text = "Ecrivez quelque chose";
                MyEditor.TextColor = Color.Gray;
            }
            else
            {
                (this.BindingContext as OrdonnanceDetailEditViewModel).Ordonnance.Comments = MyEditor.Text;
            }
        }
    }
}
