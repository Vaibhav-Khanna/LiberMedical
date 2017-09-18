using System;
using System.Threading.Tasks;
using libermedical.Enums;
using Xamarin.Forms;
using libermedical.ViewModels;

namespace libermedical.Pages
{
    public partial class OrdonnanceCreateEditPage
    {
        public OrdonnanceCreateEditPage() : base(-1, 64, false)
        {
            InitializeComponent();

            DoAsyncActions();
        }

        private async void DoAsyncActions()
        {
            while (this.BindingContext == null)
            {
                await Task.Delay(100);
            }

            var ordonnance = (BindingContext as OrdonnanceCreateEditViewModel).Ordonnance;

            MyDatePicker.Date = ordonnance.FirstCareAt;
            MyDatePicker.DateSelected += MyDatePickerOnDateSelected;

            if (string.IsNullOrEmpty(ordonnance.Comments))
            {
                MyEditor.Text = "Ecrivez quelque chose";
                MyEditor.TextColor = Color.Gray;
            }
            else
            {
                MyEditor.Text = ordonnance.Comments;
            }

            if (!(BindingContext as OrdonnanceCreateEditViewModel).Creating)
            {
                StatusLabel.Text = "Statut: " + ordonnance.StatusString;
            }

            FrequencesListView.ItemsSource = ordonnance.Frequencies;
            if (ordonnance.Frequencies.Count == 0)
            {
                FrequenciesViewCell.Height = 0;
            }
            else
            {
                FrequenciesViewCell.Height = ordonnance.Frequencies.Count * 40 + 10;
            }
        }

        private void MyDatePickerOnDateSelected(object sender, DateChangedEventArgs dateChangedEventArgs)
        {
            (this.BindingContext as OrdonnanceDetailEditViewModel).Ordonnance.First_Care_At = App.ConvertToUnixTimestamp(dateChangedEventArgs.NewDate);
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
                        (this.BindingContext as OrdonnanceCreateEditViewModel).Ordonnance.Status = StatusEnum.waiting;
                        break;
                    case "Traité":
                        (this.BindingContext as OrdonnanceCreateEditViewModel).Ordonnance.Status = StatusEnum.valid;
                        break;
                    case "Refusé":
                        (this.BindingContext as OrdonnanceCreateEditViewModel).Ordonnance.Status = StatusEnum.refused;
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
            (BindingContext as OrdonnanceCreateEditViewModel).SelectPatientCommand.Execute(sender);
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
                (this.BindingContext as OrdonnanceCreateEditViewModel).Ordonnance.Comments = MyEditor.Text;
            }
        }

        private void Cell_OnTapped(object sender, EventArgs e)
        {
            DisplayAlert("Tapped", "", "OK");
        }
    }
}
