using System.Windows.Input;
using libermedical.ViewModels.Base;
using Xamarin.Forms;
using Plugin.Media;
using Plugin.Media.Abstractions;
using libermedical.Pages;

namespace libermedical.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        //type of photo, most of the time ordonannces but we can also have document
        //so we initialize typeDoc to "ordonnace"
        private string typeDoc = "ordonnance";
        public HomeViewModel()
        {

        }
        public ICommand AssistCommand => new Command(async () =>
        {
            var action = await CoreMethods.DisplayActionSheet(null, "Annuler", null, "Appel vocal", "E-mail", "SMS");
            switch (action)
            {
                case "Appel vocal":
                    Device.OpenUri(new System.Uri("tel:+33559311824"));
                    break;

                case "E-mail":
                    Device.OpenUri(new System.Uri("mailto:contact@libermedical.com"));
                    break;

                case "SMS":
                    Device.OpenUri(new System.Uri("sms:+33559311824"));
                    break;
            }

        });

        //This concern only ordonances  in normal process
        public ICommand AddOrdonnanceCommand => new Command(async () =>
        {
            var action = await CoreMethods.DisplayActionSheet(null, "Annuler", null, "Appareil photo", "Bibliothèque photo");
            if (action == "Appareil photo")
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await CoreMethods.DisplayAlert("L'appareil photo n'est pas disponible", null, "OK");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                { });
                //if photo ok
                if (file != null)
                {
                    var profilePicture = ImageSource.FromStream(() => file.GetStream());
                    var typeNavigation = "normal";
                    await CoreMethods.PushPageModelWithNewNavigation<PatientListViewModel>(null);
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
                    await CoreMethods.PushPageModel<PatientListViewModel, PatientListPage>(null);
                }
            }
        });

        //This concern  ordonances or documents in fast process
        public ICommand FastCommand => new Command(async () =>
        {
            var action = await CoreMethods.DisplayActionSheet(null, "Annuler", null, "Ordonnance", "Document");

            if ((action == "Ordonnance") || (action == "Document"))
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await CoreMethods.DisplayAlert("L'appareil photo n'est pas disponible", null, "OK");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                { });
                if (file != null)
                {
                    //if document we change typeDoc to document
                    if (action == "Document") { typeDoc = "document"; }

                    var profilePicture = ImageSource.FromStream(() => file.GetStream());
                    var typeNavigation = "fast";
                    await CoreMethods.PushPageModelWithNewNavigation<PatientListViewModel>(null);
                }
            }
        });


    }
}
