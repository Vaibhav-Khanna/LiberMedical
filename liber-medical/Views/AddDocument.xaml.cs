using System;
using System.Collections.Generic;
using libermedical.CustomControls;
using Xamarin.Forms;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace libermedical.Views
{
    public partial class AddDocument : BasePage
    {
        public AddDocument() : base(-1, 0, false)
        {
            InitializeComponent();
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
    }
}
