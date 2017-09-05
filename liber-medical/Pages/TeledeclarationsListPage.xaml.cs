using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using libermedical.CustomControls;
using libermedical.Models;
using Xamarin.Forms;

namespace libermedical.Pages
{
    public partial class TeledeclarationsListPage : BasePage
    {
        public ObservableCollection<Teledeclaration> teledeclarations { get; set; }
        public TeledeclarationsListPage() : base(0, 64)
        {
            BindingContext = this;
            

            InitializeComponent();
        }

		void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
		{
			((ListView)sender).SelectedItem = null;
		}
       

        async void Bill_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new SecuriseBillsPage());
        }
        void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            //((ListView)sender).SelectedItem = null;
        }
    }
}
