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
        //type of photo, most of the time ordonannces but we can also have document
        //so we initialize typeDoc to "ordonnace"
        private string typeDoc = "ordonnance";

        public HomePage() : base(0, 0)
        {
            InitializeComponent();
        }
        //This concern only ordonances  in normal process
        async void AddOrdonnanceTapped(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet(null, "Annuler", null, "Appareil photo", "Bibliothèque photo");
            if (action == "Appareil photo")
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("L'appareil photo n'est pas disponible", null, "OK");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    AllowCropping = true
                });
                //if photo ok
                if (file != null)
                {
                    var profilePicture = ImageSource.FromStream(() => file.GetStream());
                    var typeNavigation = "normal";
                    await Navigation.PushModalAsync(new PatientsListPage(typeNavigation, typeDoc));
                }
            }
            if (action == "Bibliothèque photo")
            {
                await CrossMedia.Current.Initialize();

                var pickerOptions = new PickMediaOptions();

                var file = await CrossMedia.Current.PickPhotoAsync(pickerOptions);
                if (file != null)
                {
                    var profilePicture = ImageSource.FromStream(() => file.GetStream());
                    var typeNavigation = "normal";
                    await Navigation.PushAsync(new PatientsListPage(typeNavigation, typeDoc));
                }
            }
        }

        //This concern  ordonances or documents in fast process
        async void FastTapped(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet(null, "Annuler", null, "Ordonnance", "Document");

            if ((action == "Ordonnance") || (action == "Document"))
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("L'appareil photo n'est pas disponible", null, "OK");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    AllowCropping = true
                });
                if (file != null)
                {
                    //if document we change typeDoc to document
                    if (action == "Document") { typeDoc = "document"; }

                    var profilePicture = ImageSource.FromStream(() => file.GetStream());
                    var typeNavigation = "fast";
                    await Navigation.PushModalAsync(new PatientsListPage(typeNavigation, typeDoc));
                }
            }
        }
        async void AssistTapped(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet(null, "Annuler", null, "Appel vocal", "E-mail", "SMS");
        }
    }
}
