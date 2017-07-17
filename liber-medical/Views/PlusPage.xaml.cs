using System;
using System.Collections.Generic;
using libermedical.CustomControls;
using Xamarin.Forms;

namespace libermedical.Views
{
    public partial class PlusPage : BasePage
    {
        public PlusPage() : base(0, 0)
        {
            InitializeComponent();
        }

        async void AddActionSheetSimpleTapped(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Contacter mon conseiller via:", "Annuler", null, "Appel vocal", "SMS");
        }
        void AccountTapped(object sender, System.EventArgs e)
        {
            Navigation.PushModalAsync(new MyAccountPage());
        }
    }
}
