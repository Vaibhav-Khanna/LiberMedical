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
            if (!(BindingContext as TeledeclarationSecureActionViewModel).CanValidate)
            {
                label1.IsVisible = false;
                label2.IsVisible = false;
                label3.IsVisible = false;
                label1.Text = "";
                label2.Text = "";
                label3.Text = "";
                image1.Source = "";
                image2.Source = "";
            }
            (BindingContext as TeledeclarationSecureActionViewModel).CloseCommand.Execute(null);
        }

        async void Detail_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new TeledeclarationDetailPage());
        }

      
    }
}
