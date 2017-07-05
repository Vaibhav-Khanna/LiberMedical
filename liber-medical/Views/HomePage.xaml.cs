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

        async void OnActionSheetSimpleClicked(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("Que souhaitez vous ajouter?", "Annuler", null, "Ordonnance rapide", "Documents");
            if (action == "Ordonnance rapide")
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsPickPhotoSupported)
                {
                    await DisplayAlert("No Camera", ":( No camera available.", "Got It");
                    return;
                }

                var mediaFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
                {
                    AllowCropping = true
                });

                // profilePicture.Source = ImageSource.FromStream(() => mediaFile.GetStream());


            }
            if (action == "Documents") { await Navigation.PushAsync(new LoginPage()); }
        }

    }
}
