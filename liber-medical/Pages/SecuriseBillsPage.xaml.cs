using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using libermedical.CustomControls;
using libermedical.Enums;
using libermedical.Models;
using Xamarin.Forms;

namespace libermedical.Pages
{
    public partial class SecuriseBillsPage : BasePage
    {
        public ObservableCollection<Teledeclaration> Teledeclarations { get; set; }

        public SecuriseBillsPage() : base(-1, 0, false)
        {
            BindingContext = this;
            InitializeComponent();
        }
        async void Back_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        async void Bill_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new TeledeclarationSecureActionPage());
        }
    }
}
