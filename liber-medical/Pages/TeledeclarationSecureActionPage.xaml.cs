using System;
using System.Collections.Generic;
using libermedical.CustomControls;
using libermedical.ViewModels;
using Xamarin.Forms;

namespace libermedical.Pages
{
    public partial class TeledeclarationSecureActionPage : BasePage
    {
        public TeledeclarationSecureActionPage() : base(-1, 0, false)
        {
            InitializeComponent();
        }

        async void Back_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        async void Detail_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new TeledeclarationDetailPage());
        }

        protected override void OnDisappearing()
        {
            stack.Opacity = 0;
            stack.IsVisible = false; 
            base.OnDisappearing();
            stack.Opacity = 0;
            stack.IsVisible = false; 
        }
    }
}
