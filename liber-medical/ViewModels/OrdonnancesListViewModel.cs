using libermedical.Models;
using libermedical.Request;
using libermedical.Services;
using libermedical.ViewModels.Base;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using libermedical.Helpers;
using Xamarin.Forms;
using Acr.UserDialogs;
using libermedical.PopUp;

namespace libermedical.ViewModels
{
    
    public class OrdonnancesListViewModel : ListViewModelBase<Ordonnance>
    {
        int _initCount = 0;
        public int MaxCount { get; set; }
        public IStorageService<Ordonnance> _ordonnanceStorage;

        private ObservableCollection<Ordonnance> _ordonnances;
        public ObservableCollection<Ordonnance> Ordonnances
        {
            get { return _ordonnances; }
            set
            {
                _ordonnances = value;
                RaisePropertyChanged();
            }
        }

        string refreshtext = string.Empty;
        public string RefreshText 
        {
            get { return refreshtext; }
            set
            {
                refreshtext = value;
                RaisePropertyChanged();
            }
        }

        string filteracctivetext = string.Empty;
        public string FilterActiveText
        {
            get { return filteracctivetext; }
            set
            {
                filteracctivetext = value;
                RaisePropertyChanged();
            }
        }

        string noresulttext = string.Empty;
        public string NoResultText
        {
            get { return noresulttext; }
            set
            {
                noresulttext = value;
                RaisePropertyChanged();
            }
        }

        public OrdonnancesListViewModel(IStorageService<Ordonnance> storageService) : base(storageService)
        {
            _ordonnanceStorage = storageService;
           
            MessagingCenter.Subscribe<HomeViewModel>(this,"Showhomemessage", async (obj) => 
            {
                await ToastService.Show("Votre ordonnance a bien été enregistrée !");
            });

            CachedList();

            BindData(20);
        }

        public async Task BindData(int count)
        {            
            _initCount = _initCount + count;

            MaxCount = await new StorageService<Ordonnance>().DownloadOrdonnances(_initCount);
           
            var list = await _ordonnanceStorage.GetList();
            if (list != null && list.Count() != 0)
            {
                list = list.DistinctBy((arg) => arg.Id);
                list = list.OrderByDescending((arg) => arg.CreatedAt);
            }
        
            Ordonnances = new ObservableCollection<Ordonnance>(list);

            var left_sync = Ordonnances.Where( (Ordonnance arg) => !arg.IsSynced ).Count();

            if (left_sync > 0)
            {
                if (left_sync != 1)
                    RefreshText = left_sync + " fichiers en cours de synchronisation…";
                else
                    RefreshText = left_sync + " fichier en cours de synchronisation…";
            }
            else
            {
                RefreshText = null;
            }

        }

        private async Task DownlaodDocuments()
        {
            if (App.IsConnected())
            {
                var request = new GetListRequest(200, 0);
                var documents =
                    new ObservableCollection<Document>((await App.DocumentsManager.GetListAsync(request)).rows);

                //Updating records in local cache
                await new StorageService<Document>().InvalidateSyncedItems();
                await new StorageService<Document>().AddManyAsync(documents.ToList());
            }
        }

        protected override async Task TapCommandFunc(Cell cell)
        {
            var ctx = cell.BindingContext;
            await CoreMethods.PushPageModel<DetailsPatientListViewModel>(ctx, true);
        }

        public async void CreatePrescription(string filePath)
        {
            await CoreMethods.PushPageModel<OrdonnanceCreateEditViewModel>(filePath, true);
        }

        public Command DeleteOrdo => new Command(async (obj) =>
       {
            
          Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Chargement...");

          await App.OrdonnanceManager.DeleteItemAsync((string)obj);

          await BindData(0);

          Acr.UserDialogs.UserDialogs.Instance.HideLoading();

          await ToastService.Show("L’ordonnance a été supprimée avec succès");

      });


        public ICommand SelectItemCommand => new Command(async (item) =>
        {
            await CoreMethods.PushPageModel<OrdonnanceCreateEditViewModel>(item, true);
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
                if (action2 != null && action2 != "Annuler")
                {
                    await CrossMedia.Current.Initialize();
                    if (action2 == "Appareil photo")
                    {
                        if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                        {
                            await CoreMethods.DisplayAlert("L'appareil photo n'est pas disponible", null, "OK");
                            return;
                        }
                        var perm = await App.AskForCameraPermission();

                        if (perm)
                        {
                            await CrossMedia.Current.Initialize();
                            var file = await CrossMedia.Current.TakePhotoAsync(
                                new StoreCameraMediaOptions
                                {
                                    Directory = "Docs",
                                    Name = DateTime.Now.Ticks.ToString(),
                                    CompressionQuality = 30,
                                    RotateImage = false,
                                    SaveToAlbum = false
                                });
                            if (file != null)
                            {
                                filePath = file.Path;
                            }
                        }
                    }
                    else if (action2 == "Bibliothèque photo")
                    {
                        if (await App.AskForPhotoPermission())
                        {
                            var pickerOptions = new PickMediaOptions() { CompressionQuality = 30, RotateImage = false };
                            var file = await CrossMedia.Current.PickPhotoAsync(pickerOptions);
                            if (file != null)
                            {
                                filePath = file.Path;
                            }
                        }
                    }

                    if (string.IsNullOrWhiteSpace(filePath))
                        return;

                    var ordonnance = new Ordonnance
                    {
                        Id = Guid.NewGuid().ToString(),
                        Attachments = new List<string> { filePath },
                        Frequencies = new List<Frequency>(),
                        First_Care_At = 0
                    };

                    Filter _filters = null;
                  

                    if (action == "Ordonnance rapide")
                    {
                        await CoreMethods.PushPageModel<PatientListViewModel>(
                            new string[] { "OrdonanceSelectPatient", "normal", "ordonnance" }, true);

                        MessagingCenter.Unsubscribe<PatientListViewModel, Patient>(this, Events.OrdonnancePageSetPatientForOrdonnance);
                        MessagingCenter.Subscribe<PatientListViewModel, Patient>(this,
                            Events.OrdonnancePageSetPatientForOrdonnance, async (sender, patient) =>
                            {
                                if (patient != null && ordonnance != null)
                                {
                                    ordonnance.PatientId = patient.Id;
                                    ordonnance.Patient = patient;
                                    ordonnance.PatientName = $"{patient.FirstName} {patient.LastName}";
                                    ordonnance.IsSynced = false;
                                    ordonnance.UpdatedAt = null;
                                    ordonnance.First_Care_At = 0;                      

                                    var storageService = new StorageService<Ordonnance>();

                                    await storageService.AddAsync(ordonnance);

                                    if (App.IsConnected())
                                    {
                                        Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Chargement...");
                                        storageService.PushOrdonnance(ordonnance, true);                                       
                                        Acr.UserDialogs.UserDialogs.Instance.HideLoading();
                                    }
                                   
                                    await BindData(0);

                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        ToastService.Show("Votre ordonnance a bien été enregistrée !");
                                    });

                                    MessagingCenter.Send(this, Events.UpdatePrescriptionFilters, _filters);

                                    MessagingCenter.Unsubscribe<PatientListViewModel, Patient>(this, Events.OrdonnancePageSetPatientForOrdonnance);
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


        public async Task CachedList()
        {
            
            var list = await _ordonnanceStorage.GetList();

            if (list != null && list.Count() != 0)
            {
                list = list.DistinctBy((arg) => arg.Id);
                list = list.OrderByDescending((arg) => arg.CreatedAt);
            }

            Ordonnances = new ObservableCollection<Ordonnance>(list);

            var left_sync = Ordonnances.Where((Ordonnance arg) => !arg.IsSynced).Count();

            if (left_sync > 0)
            {
                if (left_sync!=1)
                RefreshText = left_sync + " fichiers en cours de synchronisation…";
                else
                RefreshText = left_sync + " fichier en cours de synchronisation…";
            }
            else
            {
                RefreshText = null;
            }
        }


    }
}
