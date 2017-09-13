using System;
using System.Collections.Generic;
using System.Windows.Input;
using libermedical.Models;
using libermedical.Services;
using libermedical.ViewModels.Base;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace libermedical.ViewModels
{
    public class OrdonnanceCreateViewModel : ViewModelBase
    {
        public string PatientLabel { get; set; } = "Choisissez un patient";
        public Ordonnance Ordonnance { get; set; }

        public OrdonnanceCreateViewModel()
        {
            Ordonnance = new Ordonnance
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Today,
                Attachments = new List<string>(),
                Frequencies = new List<Frequency>()
            };

            MessagingCenter.Subscribe<PatientListViewModel, Patient>(this, Events.OrdonnancePageSetPatientForOrdonnance, async (sender, patient) => {

                if (patient != null)
                {
                    Ordonnance.PatientId = patient.Id;
                    PatientLabel = patient.Fullname;
                }
            });
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            if (initData != null)
            {
                Ordonnance.Attachments.Add(initData as string);
            }
        }

        public ICommand SelectPatientCommand => new Command(async () =>
        {
            await CoreMethods.PushPageModel<PatientListViewModel>(new string[] { "OrdonanceSelectPatient", "normal", "ordonnance" }, true);
        });

        public ICommand SaveCommand => new Command(async () =>
        {
            await new StorageService<Ordonnance>().AddAsync(Ordonnance);

            //TODO: Display success toast

            await CoreMethods.PopPageModel(null, true);
        });

        public ICommand AddAttachmentCommand => new Command(async () =>
        {
            var action = await CoreMethods.DisplayActionSheet(null, "Annuler", null, "Appareil photo", "Bibliothèque photo");

            await CrossMedia.Current.Initialize();
            if (action == "Appareil photo")
            {
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await CoreMethods.DisplayAlert("L'appareil photo n'est pas disponible", null, "OK");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions());
                if (file != null)
                {
                    Ordonnance.Attachments.Add(file.Path);
                }
            }
            else if (action == "Bibliothèque photo")
            {
                var file = await CrossMedia.Current.PickPhotoAsync();
                if (file != null)
                {
                    Ordonnance.Attachments.Add(file.Path);
                }
            }
        });

        public ICommand FrequenceTappedCommand => new Command(async () =>
        {
            var frequency = new Frequency();
            Ordonnance.Frequencies.Add(frequency);
            await CoreMethods.PushPageModel<OrdonnanceFrequenceViewModel>(frequency, true);
        });
    }
}
