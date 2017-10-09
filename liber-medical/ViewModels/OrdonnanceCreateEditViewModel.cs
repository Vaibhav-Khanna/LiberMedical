using System;
using System.Collections.Generic;
using System.Windows.Input;
using libermedical.Models;
using libermedical.Services;
using libermedical.ViewModels.Base;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace libermedical.ViewModels
{
    public class OrdonnanceCreateEditViewModel : ViewModelBase
    {
        private ObservableCollection<Frequency> _frequencies;
        public ObservableCollection<Frequency> Frequencies
        {
            get { return _frequencies; }
            set
            {
                _frequencies = value;
                RaisePropertyChanged();
            }
        }
        public string PatientLabel { get; set; } = "Choisissez un patient";
        public Ordonnance Ordonnance { get; set; }

        public bool Creating;
        public string SaveLabel { get; set; } = "Enregistrer";

        public OrdonnanceCreateEditViewModel()
        {
            Ordonnance = new Ordonnance
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Today,
                Attachments = new List<string>(),
                Frequencies = new List<Frequency>(),
                First_Care_At = App.ConvertToUnixTimestamp(DateTime.Now)
            };

            MessagingCenter.Subscribe<PatientListViewModel, Patient>(this, Events.OrdonnancePageSetPatientForOrdonnance, (sender, patient) =>
            {
                if (patient != null)
                {
                    Ordonnance.Patient = patient;
                    Ordonnance.PatientId = patient.Id;
                    Ordonnance.PatientName = patient.Fullname;
                    PatientLabel = patient.Fullname;
                }
            });
        }

        public override async void Init(object initData)
        {
            base.Init(initData);
            if (initData != null)
            {
                if (initData is string)
                {
                    Ordonnance.Attachments.Add((string)initData);
                    Creating = true;
                    SaveLabel = "Enregistrer";
                }
                else
                {
                    var ordonnance = initData as Ordonnance;
                    Ordonnance = await new StorageService<Ordonnance>().GetItemAsync($"Ordonnance_{ordonnance.Id}");
                    Ordonnance.Patient = ordonnance.Patient;
                    Ordonnance.PatientId = ordonnance.PatientId;
                    Ordonnance.PatientName = ordonnance.PatientName;
                    PatientLabel = Ordonnance?.PatientName;
                    Frequencies = new ObservableCollection<Frequency>(Ordonnance.Frequencies);
                    Creating = false;
                    SaveLabel = "Modifier";
                }
            }
        }

        public ICommand SelectPatientCommand => new Command(async () =>
        {
            await CoreMethods.PushPageModel<PatientListViewModel>(new[] { "OrdonanceSelectPatient", "normal", "ordonnance" }, true);
        });

        public ICommand SaveCommand => new Command(async () =>
        {
            var storageService = new StorageService<Ordonnance>();
            await storageService.DeleteItemAsync(typeof(Ordonnance).Name + "_" + Ordonnance.Id);
            Ordonnance.IsSynced = false;
            await storageService.AddAsync(Ordonnance);

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

        public ICommand AddFrequenceTappedCommand => new Command(async () =>
        {
            //var frequency = new Frequency();
            //Ordonnance.Frequencies.Add(frequency);
            await CoreMethods.PushPageModel<OrdonnanceFrequenceViewModel>(null, true);
            SubscribeMessage();

        });

        public ICommand ModifyFrequenceTappedCommand => new Command(async frequency =>
        {
            await CoreMethods.PushPageModel<OrdonnanceFrequenceViewModel>(frequency, true);
        });

        private void SubscribeMessage()
        {
            MessagingCenter.Subscribe<OrdonnanceFrequence2ViewModel, Frequency>(this, Events.UpdateFrequencies, ((sender, args) =>
            {

                if (args != null)
                {
                    var frequency = args as Frequency;
                    if (Ordonnance.Frequencies != null)
                        Ordonnance.Frequencies.Add(frequency);
                    Frequencies = Ordonnance.Frequencies != null ? new ObservableCollection<Frequency>(Ordonnance.Frequencies) : new ObservableCollection<Frequency>(new List<Frequency>() { frequency });
                }

                MessagingCenter.Unsubscribe<OrdonnanceFrequence2ViewModel, Frequency>(this, Events.UpdateFrequencies);
            }));
        }
    }
}
