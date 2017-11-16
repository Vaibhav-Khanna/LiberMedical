using System;
using System.Threading.Tasks;
using libermedical.Enums;
using libermedical.Models;
using Xamarin.Forms;
using libermedical.ViewModels;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Diagnostics;

namespace libermedical.Pages
{
    public partial class OrdonnanceCreateEditPage
    {
        private Ordonnance _ordonnance = null;
        public OrdonnanceCreateEditPage() : base(-1, 67, false)
        {
            InitializeComponent();
            SubscribeMessages();
            DoAsyncActions();
            
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            (BindingContext as OrdonnanceCreateEditViewModel).DeleteImage.Execute((sender as MenuItem).CommandParameter);
        }

        private async void DoAsyncActions()
        {
            while (this.BindingContext == null)
            {
                await Task.Delay(100);
            }

            _ordonnance = (BindingContext as OrdonnanceCreateEditViewModel).Ordonnance;
            //if ((BindingContext as OrdonnanceCreateEditViewModel).SaveLabel == "Enregistrer")
                //VisualiserSection.IsVisible = false;
            
            MyDatePicker.Date = _ordonnance.FirstCareAt;

            MyDatePicker.DateSelected += MyDatePickerOnDateSelected;
            SelectedDate.Text = $"Premier soin: {_ordonnance.FirstCareAt.ToString("dd-MM-yyyy")}";
            if (_ordonnance.Frequencies != null)
                UpdateFrequenciesViewCellHeight(new ObservableCollection<Frequency>(_ordonnance.Frequencies));
            else
                UpdateFrequenciesViewCellHeight(new ObservableCollection<Frequency>());

            if (string.IsNullOrEmpty(_ordonnance.Comments))
            {
                MyEditor.Text = "Ecrivez quelque chose";
                MyEditor.TextColor = Color.Gray;
            }
            else
            {
                MyEditor.Text = _ordonnance.Comments;
            }
            StatusLabel.Text = "Statut: " + _ordonnance.StatusString;
            if (!(BindingContext as OrdonnanceCreateEditViewModel).Creating)
            {
                StatusLabel.Text = "Statut: " + _ordonnance.StatusString;
            }

            if((BindingContext as OrdonnanceCreateEditViewModel).Ordonnance?.Status == Enums.StatusEnum.refused.ToString())
            {
                statusIcon.Source = "cancel.png";
                StatusLabel.TextColor = Color.Red;
                StatusLabel.Text = "Refusé : " + (BindingContext as OrdonnanceCreateEditViewModel).Ordonnance?.RefusedReason;
            }
            else if((BindingContext as OrdonnanceCreateEditViewModel).Ordonnance?.Status == Enums.StatusEnum.valid.ToString())
            {
                statusIcon.Source = "send.png";
                StatusLabel.Text = "Traité le " + (BindingContext as OrdonnanceCreateEditViewModel).Ordonnance?.StatusChangedAt.ToString("dd/MM/yyyy");
            }

        }

        private void MyDatePickerOnDateSelected(object sender, DateChangedEventArgs dateChangedEventArgs)
        {
            (this.BindingContext as OrdonnanceCreateEditViewModel).Ordonnance.First_Care_At = App.ConvertToUnixTimestamp(dateChangedEventArgs.NewDate);
            SelectedDate.Text = $"Premier soin: {(sender as DatePicker).Date.ToString("dd-MM-yyyy")}";
        }

        void Handle_Unfocused(object sender, Xamarin.Forms.FocusEventArgs e)
        {
            (this.BindingContext as OrdonnanceCreateEditViewModel).Ordonnance.First_Care_At = App.ConvertToUnixTimestamp((sender as DatePicker).Date);
            SelectedDate.Text = $"Premier soin: {(sender as DatePicker).Date.ToString("dd-MM-yyyy")}";
        }


        async void Cancel_Tapped(object sender, EventArgs e)
        {
            //await Navigation.PopModalAsync();
        }



        private void DatePicker_Tapped(object sender, EventArgs e)
        {
            if ((this.BindingContext as OrdonnanceCreateEditViewModel).CanEdit)
            {
                MyDatePicker.Focus();
            }
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

        private void FrequencesListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
            }

            (BindingContext as OrdonnanceCreateEditViewModel).ModifyFrequenceTappedCommand
                .Execute(e.SelectedItem as Frequency);

            // disable the visual selection state.
            ((ListView)sender).SelectedItem = null;
        }

        private void AttachmentsListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItem == null)
                {
                    return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
                }

             (BindingContext as OrdonnanceCreateEditViewModel).ViewAttachment
                 .Execute(e.SelectedItem as string);

                // disable the visual selection state.
                ((ListView)sender).SelectedItem = null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

           
        }

        private void SubscribeMessages()
        {
            MessagingCenter.Subscribe<OrdonnanceCreateEditViewModel, ObservableCollection<Frequency>>(this, Events.UpdateFrequenciesViewCellHeight, (sender, args) =>
            {
                UpdateFrequenciesViewCellHeight(args);
            });

            MessagingCenter.Subscribe<OrdonnanceCreateEditViewModel, List<string>>(this, Events.UpdateAttachmentsViewCellHeight, (sender, args) =>
            {
                UpdateAttachmentsViewCellHeight(args);
            });

        }

        private void UpdateFrequenciesViewCellHeight(ObservableCollection<Frequency> frequencies)
        {
            Device.BeginInvokeOnMainThread(() =>
                {
                    if (frequencies != null)
                        FrequencesListView.ItemsSource = frequencies;
                    FrequencesListView.HeightRequest = frequencies.Count * 43;

                });
        }

        private void UpdateAttachmentsViewCellHeight(List<string> attachments)
        {
            Device.BeginInvokeOnMainThread(() =>
                {
                    if (attachments != null)
                        //AttachmentsListView.ItemsSource = attachments;
                        AttachmentsListView.HeightRequest = attachments.Count * 43;

                });
        }
    }
}
