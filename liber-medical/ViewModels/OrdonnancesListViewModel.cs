using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using libermedical.Models;
using libermedical.Services;
using libermedical.ViewModels.Base;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Windows.Input;
using libermedical.Enums;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace libermedical.ViewModels
{
    public class OrdonnancesListViewModel : ListViewModelBase<Ordonnance>
    {
        private IStorageService<Ordonnance> _ordonnanceStorage;
        public ObservableCollection<Ordonnance> Ordonnances { get; set; }

        public OrdonnancesListViewModel(IStorageService<Ordonnance> storageService) : base(storageService)
        {
            _ordonnanceStorage = storageService;
            BindData();
        }

        private async void BindData()
        {
            //Ordonnances = new ObservableCollection<Ordonnance>(await _ordonnanceStorage.GetList());

            Ordonnances = new ObservableCollection<Ordonnance>
            {
                new Ordonnance
                {
                    Reference = 1,
                    Patient = new Patient {FirstName = "Fred", LastName = "Pearson"},
                    FirstCareAt = new DateTime(2017, 04, 02),
                    Status = StatusEnum.valid
                },
                new Ordonnance
                {
                    Reference = 2,
                    Patient = new Patient {FirstName = "Jonanthan", LastName = "Vaughn"},
                    FirstCareAt = new DateTime(2017, 11, 10),
                    Status = StatusEnum.valid
                },
                new Ordonnance
                {
                    Reference = 3,
                    Patient = new Patient {FirstName = "Alexander", LastName = "Zimmerman"},
                    FirstCareAt = new DateTime(2017, 02, 06),
                    Status = StatusEnum.refused
                },
                new Ordonnance
                {
                    Reference = 4,
                    Patient = new Patient {FirstName = "Keith", LastName = "Kelley"},
                    FirstCareAt = new DateTime(2017, 09, 17),
                    Status = StatusEnum.refused
                },
                new Ordonnance
                {
                    Reference = 5,
                    Patient = new Patient {FirstName = "Justin", LastName = "Sims"},
                    FirstCareAt = new DateTime(2017, 03, 04),
                    Status = StatusEnum.valid
                },
                new Ordonnance
                {
                    Reference = 6,
                    Patient = new Patient {FirstName = "Franklin", LastName = "Howard"},
                    FirstCareAt = new DateTime(2017, 09, 29),
                    Status = StatusEnum.waiting
                },
                new Ordonnance
                {
                    Reference = 8,
                    Patient = new Patient {FirstName = "Mickael", LastName = "Green"},
                    FirstCareAt = new DateTime(2017, 09, 29),
                    Status = StatusEnum.waiting
                },
                new Ordonnance
                {
                    Reference = 9,
                    Patient = new Patient {FirstName = "Paul", LastName = "Howard"},
                    FirstCareAt = new DateTime(2017, 09, 29),
                    Status = StatusEnum.valid
                },
                new Ordonnance
                {
                    Reference = 10,
                    Patient = new Patient {FirstName = "Justin", LastName = "Howard"},
                    FirstCareAt = new DateTime(2017, 10, 29),
                    Status = StatusEnum.waiting
                },
                new Ordonnance
                {
                    Reference = 11,
                    Patient = new Patient {FirstName = "John", LastName = "Obama"},
                    FirstCareAt = new DateTime(2017, 09, 29),
                    Status = StatusEnum.waiting
                }
            };
        }

        protected override async Task TapCommandFunc(Cell cell)
        {
            var ctx = cell.BindingContext;
            await CoreMethods.PushPageModel<DetailsPatientListViewModel>(ctx, true);
        }

        public async void CreatePrescription(string filePath)
        {
            await CoreMethods.PushPageModel<OrdonnanceCreateViewModel>(filePath, true);
        }

        public ICommand SelectItemCommand => new Command(async (item) =>
        {
            await CoreMethods.PushPageModel<OrdonnanceDetailViewModel>(item, true);
        });

        public ICommand AddCommand => new Command(async () =>
        {
            var action =
                await CoreMethods.DisplayActionSheet(null, "Annuler", null, "Ordonnance rapide",
                    "Ordonnance classique");

            string filePath = "";

            if (action != null && action != "Annuler")
            {
                var action2 =
                    await CoreMethods.DisplayActionSheet(null, "Annuler", null, "Appareil photo", "Bibliothèque photo");
                if (action2 != null && action != "Annuler")
                {
                    await CrossMedia.Current.Initialize();
                    if (action2 == "Appareil photo")
                    {
                        if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                        {
                            await CoreMethods.DisplayAlert("L'appareil photo n'est pas disponible", null, "OK");
                            return;
                        }

                        var file = await CrossMedia.Current.TakePhotoAsync(
                            new StoreCameraMediaOptions
                            {
                                AllowCropping = true
                            });
                        if (file != null)
                        {
                            filePath = file.Path;
                        }
                    }
                    else if (action2 == "Bibliothèque photo")
                    {
                        var file = await CrossMedia.Current.PickPhotoAsync();
                        if (file != null)
                        {
                            filePath = file.Path;
                        }
                    }

                    var ordonnance = new Ordonnance
                    {
                        Id = Guid.NewGuid().ToString(),
                        Attachments = new List<string> { filePath },
                        Frequencies = new List<Frequency>()
                    };

                    if (action == "Ordonnance rapide")
                    {
                        await CoreMethods.PushPageModel<PatientListViewModel>(
                            new string[] { "OrdonanceSelectPatient", "normal", "ordonnance" }, true);

                        MessagingCenter.Subscribe<PatientListViewModel, Patient>(this,
                            Events.OrdonnancePageSetPatientForOrdonnance, async (sender, patient) =>
                            {
                                if (patient != null)
                                {
                                    ordonnance.PatientId = patient.Id;

                                    await new StorageService<Ordonnance>().AddAsync(ordonnance);

                                    //display toast
                                    //pop to root
                                }
                            });
                    }
                    else if (action == "Ordonnance classique")
                    {
                        CreatePrescription(filePath);
                    }
                }
            }
        });
    }
}
