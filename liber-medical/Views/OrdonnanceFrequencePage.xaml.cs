using System;
using System.Collections.Generic;
using libermedical.CustomControls;
using Xamarin.Forms;

namespace libermedical.Views
{
    public partial class OrdonnanceFrequencePage : BasePage
    {
        public OrdonnanceFrequencePage() : base(-1, 0, false)
        {
            InitializeComponent();
        }
        async void Cancel_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }

        async void Frequence_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new OrdonnanceFrequence2Page());
        }
    }
}
