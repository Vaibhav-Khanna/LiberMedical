using System;
using System.Threading.Tasks;
using libermedical.Enums;
using libermedical.Models;
using Xamarin.Forms;
using libermedical.ViewModels;
using System.Collections.ObjectModel;

namespace libermedical.Pages
{
	public partial class OrdonnanceCreateEditPage
	{
		public OrdonnanceCreateEditPage() : base(-1, 64, false)
		{
			InitializeComponent();
			SubscribeMessages();
            DoAsyncActions();
			
			//TODO subscribe to changes of frequencies
		}

		private async void DoAsyncActions()
		{
			while (this.BindingContext == null)
			{
				await Task.Delay(100);
			}

			var ordonnance = (BindingContext as OrdonnanceCreateEditViewModel).Ordonnance;
			if ((BindingContext as OrdonnanceCreateEditViewModel).SaveLabel == "Enregistrer")
				TableLayout.Root.Remove(VisualiserSection);
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

			Device.BeginInvokeOnMainThread(() =>
				{
				UpdateFrequenciesViewCellHeight(new ObservableCollection<Frequency>(ordonnance.Frequencies));

					AttachmentsListView.ItemsSource = ordonnance.Attachments;
					if (ordonnance.Attachments.Count == 0)
					{
						AttachmentsViewCell.Height = 0;
					}
					else
					{
						AttachmentsViewCell.Height = ordonnance.Attachments.Count * 40 + 10;
					}
				});


		}

		private void MyDatePickerOnDateSelected(object sender, DateChangedEventArgs dateChangedEventArgs)
		{
			(this.BindingContext as OrdonnanceCreateEditViewModel).Ordonnance.First_Care_At = App.ConvertToUnixTimestamp(dateChangedEventArgs.NewDate);
			SelectedDate.Text = $"Date : {(sender as DatePicker).Date.ToString("dd-MM-yyyy")}";

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

		}

		private void UpdateFrequenciesViewCellHeight(ObservableCollection<Frequency> frequencies)
		{
			Device.BeginInvokeOnMainThread(() =>
				{
					FrequencesListView.ItemsSource = frequencies;
					FrequenciesViewCell.Height = frequencies.Count * 43;

				});
		}
	}
}
