using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using libermedical.Models;
using libermedical.Services;
using libermedical.ViewModels.Base;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace libermedical.ViewModels
{
    public class OrdonnanceDetailEditViewModel : ViewModelBase
    {
        public string PatientLabel { get; set; } = "Choisissez un patient";
        public Ordonnance Ordonnance { get; set; }

        public ObservableCollection<Cotation> cotation { get; set; }

        public OrdonnanceDetailEditViewModel()
        {
            Ordonnance = new Ordonnance
            {
                CreatedAt = DateTime.Today,
                Attachments = new List<string>()
            };

            cotation = new ObservableCollection<Cotation>
            {
                new Cotation {
                    FirstCode=1,
                    SecondCode= "AMI",
                    ThirdCode= 2
                },
                new Cotation {
                    FirstCode=2,
                    SecondCode= "AMI",
                    ThirdCode= 1
                },
                new Cotation {
                    FirstCode=3,
                    SecondCode= "AMK",
                    ThirdCode= 5
                }
            };

            MessagingCenter.Subscribe<PatientListViewModel, Patient>(this, Events.OrdonnancePageSetPatientForOrdonnance, async (sender, patient) => {

                if (patient != null)
                {
                    Ordonnance.PatientId = patient.Id;
                    PatientLabel = patient.FullName;
                }
            });
        }

        public ICommand SelectPatientCommand => new Command(async () =>
        {
            await CoreMethods.PushPageModel<PatientListViewModel>(new string[] { "OrdonanceSelectPatient", "normal", "ordonnance" }, true);
        });

        public ICommand SaveCommand => new Command(async () =>
        {
            Ordonnance.Id = Guid.NewGuid().ToString();
            await new StorageService<Ordonnance>().AddAsync(Ordonnance);
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
    }
}
