using System;
using libermedical.CustomControls;
using libermedical.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace libermedical.Pages
{
	public partial class AddDocumentPage : BasePage
	{
		public AddDocumentPage() : base(-1, 0, false)
		{
			InitializeComponent();

		}

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                (BindingContext as AddDocumentViewModel).PropertyChanged -= Handle_PropertyChanged;
                (BindingContext as AddDocumentViewModel).PropertyChanged += Handle_PropertyChanged;
            }
        }


        void Handle_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "CanEdit")
            {
                if((BindingContext as AddDocumentViewModel).CanEdit)
                {
                    SelectedDate.TextColor = Color.Black;
                    name.TextColor = Color.Black;
                    name.Opacity = 1;
                }
                else
                {
                    SelectedDate.TextColor = Color.Gray;
                    name.TextColor = Color.Black;
                    name.Opacity = 0.5;
                }
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if ((BindingContext as AddDocumentViewModel).CanEdit)
            {
                SelectedDate.TextColor = Color.Black;
                name.TextColor = Color.Black;
                name.Opacity = 1;
            }
            else
            {
                SelectedDate.TextColor = Color.Gray;
                name.TextColor = Color.Black;
                name.Opacity = 0.5;
            }

        }

        async void Cancel_Tapped(object sender, System.EventArgs e)
		{
			await Navigation.PopModalAsync();
        }
		async void Save_Tapped(object sender, System.EventArgs e)
		{

			await Navigation.PopModalAsync();
		}
		async void Document_Tapped(object sender, System.EventArgs e)
		{
			await CrossMedia.Current.Initialize();


			var pickerOptions = new PickMediaOptions();

			var file = await CrossMedia.Current.PickPhotoAsync(pickerOptions);
			if (file != null)
			{
				var profilePicture = ImageSource.FromStream(() => file.GetStream());
				var typeNavigation = "normal";

			}
		}

		private void DatePicker_Tapped(object sender, EventArgs e)
		{
			
		}

        private void MyDatePickerOnDateSelected(object sender, DateChangedEventArgs dateChangedEventArgs)
        {
			(this.BindingContext as AddDocumentViewModel).Document.AddDate = dateChangedEventArgs.NewDate;
			SelectedDate.Text = $"Date : {(sender as DatePicker).Date.ToString("dd-MM-yyyy")}";
		}

		void Handle_Unfocused(object sender, Xamarin.Forms.FocusEventArgs e)
		{
			(this.BindingContext as AddDocumentViewModel).Document.AddDate = (sender as DatePicker).Date;
			SelectedDate.Text = $"Date : {(sender as DatePicker).Date.ToString("dd-MM-yyyy")}";
		}
	}
}
