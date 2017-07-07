using System;
using System.Collections.Generic;
using libermedical.CustomControls;
using Xamarin.Forms;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace libermedical.Views
{
    public partial class HomePage : BasePage
    {
        public HomePage() : base(0, 0)
        {
            InitializeComponent();
        }

        async void AddActionSheetSimpleTapped(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Que souhaitez vous ajouter?", "Annuler", null, "Ordonnance rapide", "Documents");
            if (action == "Ordonnance rapide")
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("No Camera", ":( No camera available.", "OK");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions()
                {
                    AllowCropping = true
                });
                if (file != null)
                {
                    var profilePicture = ImageSource.FromStream(() => file.GetStream());
                    var typeNavigation = "normal";
                    await Navigation.PushAsync(new PatientsListPage(typeNavigation));
                }

            }
            if (action == "Documents")
            {
                await CrossMedia.Current.Initialize();


                var pickerOptions = new PickMediaOptions();

                var file = await CrossMedia.Current.PickPhotoAsync(pickerOptions);
                if (file != null)
                {
                    var profilePicture = ImageSource.FromStream(() => file.GetStream());
                    var typeNavigation = "normal";
                    await Navigation.PushAsync(new PatientsListPage(typeNavigation));
                }
            }


        }

        async void FastTapped(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions()
            {
                AllowCropping = true
            });
            if (file != null)
            {
                var profilePicture = ImageSource.FromStream(() => file.GetStream());
                var typeNavigation = "fast";
                await Navigation.PushAsync(new PatientsListPage(typeNavigation));
            }
        }
    }
}
