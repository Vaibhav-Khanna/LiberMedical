using libermedical.CustomControls;
using libermedical.ViewModels;
using Syncfusion.SfPicker.XForms;
using System.Collections;
using Xamarin.Forms;

namespace libermedical.Pages
{
    public partial class OrdonnanceCotationPage : BasePage
    {

        public OrdonnanceCotationPage() : base(-1, 0, false)
        {
            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Picker.IsVisible = false;
        }

        void Handle_OnChanged(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            if (Switch.IsToggled)
            {
                Picker.IsVisible = true;
                //Footer.IsVisible = true;
                //(this.BindingContext as OrdonnanceCotationViewModel).ShouldEnableAdd = true;
            }
            else
            {
                Picker.IsVisible = false;
                //Footer.IsVisible = false;
                //(this.BindingContext as OrdonnanceCotationViewModel).ShouldEnableAdd = false;
            }
        }

        private void picker_SelectionChanged(object sender, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        {
            var newValue = e.NewValue as IList;
			if (newValue != null && newValue.Count == 3)
			{
				((SfPicker)sender).HeaderText = $"{newValue[0]} {newValue[1]} {newValue[2]}";
				(this.BindingContext as OrdonnanceCotationViewModel).Selected = newValue; 
			}

        }
        async void Cancel_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
