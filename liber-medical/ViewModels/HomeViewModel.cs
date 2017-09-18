using System.Windows.Input;
using libermedical.ViewModels.Base;
using Xamarin.Forms;
using Plugin.Media;
using Plugin.Media.Abstractions;
using libermedical.Models;
using System.Threading.Tasks;
using System;
using libermedical.Services;
using System.Collections.Generic;

namespace libermedical.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        //type of photo, most of the time ordonannces but we can also have document
        //so we initialize typeDoc to "ordonnace"
        private string typeDoc = "ordonnance";
		private string _documentPath = string.Empty;
        public HomeViewModel()
        {
            SubscribeToMessages();
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

                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions());
                //if photo ok
                if (file != null)
                {
                    var profilePicture = ImageSource.FromStream(() => file.GetStream());
                    var typeNavigation = "normal";

                    await CoreMethods.PushPageModel<PatientListViewModel>(new string[] { "HomeSelectPatient", typeNavigation, typeDoc }, true);

                    //var page = FreshPageModelResolver.ResolvePageModel<PatientListViewModel>();
                    //var basicNavContainer = new FreshNavigationContainer(page, "HomeSelectPatient");
                    //await CoreMethods.PushNewNavigationServiceModal(basicNavContainer, new FreshBasePageModel[] { page.GetModel() });
                }
            }
            else if (action == "Bibliothèque photo")
            {
                await CrossMedia.Current.Initialize();

				var pickerOptions = new PickMediaOptions();

                var file = await CrossMedia.Current.PickPhotoAsync(pickerOptions);
                if (file != null)
                {
                    var profilePicture = ImageSource.FromStream(() => file.GetStream());
                    var typeNavigation = "normal";
					_documentPath = file.Path;
                    await CoreMethods.PushPageModel<PatientListViewModel>(new string[] { "HomeSelectPatient", typeNavigation, typeDoc },true);

                    //var page = FreshPageModelResolver.ResolvePageModel<PatientListViewModel>();
                    //var basicNavContainer = new FreshNavigationContainer(page, "HomeSelectPatient");
                    //await CoreMethods.PushNewNavigationServiceModal(basicNavContainer, new FreshBasePageModel[] { page.GetModel() });

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
				{ Directory="Docs",Name=DateTime.Now.Ticks.ToString() });
                if (file != null)
                {
                    //if document we change typeDoc to document
                    if (action == "Document") { typeDoc = "document"; }

                    var profilePicture = ImageSource.FromStream(() => file.GetStream());
                    var typeNavigation = "fast";
					_documentPath = file.Path;
                    await CoreMethods.PushPageModel<PatientListViewModel>(new string[] { "HomeSelectPatient", typeNavigation, typeDoc }, true);

                    //var page = FreshPageModelResolver.ResolvePageModel<PatientListViewModel>();
                    //var basicNavContainer = new FreshNavigationContainer(page, "HomeSelectPatient");
                    //await CoreMethods.PushNewNavigationServiceModal(basicNavContainer, new FreshBasePageModel[] { page.GetModel() });
                }
            }
        });

        private void SubscribeToMessages()
        {
            MessagingCenter.Subscribe<PatientListViewModel, Patient>(this, Events.HomePageSetPatientForOrdonnance,
                async (sender, patient) =>
                {
                    if (patient != null)
                    {
                        await AddPrescription(patient);
                    }
                });
        }

        private async Task AddPrescription(Patient patient)
        {
            if(typeDoc== "ordonnance")
            {
                var ordannance = new Ordonnance()
                {
                    Reference = DateTime.Now.Ticks,
                    First_Care_At = App.ConvertToUnixTimestamp(DateTime.Now),
                    Patient = patient,
                    Status = Enums.StatusEnum.valid,
                    Id = DateTime.Now.Ticks.ToString(),
                    CreatedAt = DateTimeOffset.Now,
                    Attachments = new List<string>() { _documentPath },
                    Frequencies = new List<Frequency>()
                };
                await new StorageService<Ordonnance>().AddAsync(ordannance);
            }
            else
            {
                var document = new Document()
                {
                    Reference = DateTime.Now.Ticks,
                    AddDate = DateTime.Now,
                    Patient = patient,
                    FilePath = _documentPath,
                    Id = DateTime.Now.Ticks.ToString(),
                    CreatedAt = DateTimeOffset.Now,
                };
                await new StorageService<Document>().AddAsync(document);
            }
            
        }
    }
}
