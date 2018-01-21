using FreshMvvm;
using libermedical.Models;
using libermedical.Request;
using libermedical.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.Generic;
using Acr.UserDialogs;
using libermedical.PopUp;

namespace libermedical.ViewModels
{
    public class DetailsPatientListViewModel : FreshBasePageModel
    {
        private bool _showStackDocument;
        private bool _showStackOrdonnance;
        private bool _showBoxViewOrdonnances;
        private bool _showBoxViewDocuments;

        public bool ShowStackDocument { get { return _showStackDocument; } set { _showStackDocument = value; RaisePropertyChanged(); } }
        public bool ShowStackOrdonnance { get { return _showStackOrdonnance; } set { _showStackOrdonnance = value; RaisePropertyChanged(); } }
        public bool ShowBoxViewOrdonnances { get { return _showBoxViewOrdonnances; } set { _showBoxViewOrdonnances = value; RaisePropertyChanged(); } }
        public bool ShowBoxViewDocuments { get { return _showBoxViewDocuments; } set { _showBoxViewDocuments = value; RaisePropertyChanged(); } }

        public Patient Patient { get; set; }

        private ObservableCollection<Ordonnance> _ordonnances;
        public ObservableCollection<Ordonnance> Ordonnances { get { return _ordonnances; } set { _ordonnances = value; RaisePropertyChanged(); } }

        private ObservableCollection<Document> _documents;
        public ObservableCollection<Document> Documents { get { return _documents; } set { _documents = value; RaisePropertyChanged(); } }

        private string _bottomTitle;
        public string BottomTitle { get { return _bottomTitle; } set { _bottomTitle = value; RaisePropertyChanged(); } }
        public override void Init(object initData)
        {
            base.Init(initData);
            this.Patient = (Patient)initData;
        }
        public DetailsPatientListViewModel()
        {
            ShowStackOrdonnance = ShowBoxViewOrdonnances = true;
            BottomTitle = "+ Ajouter une ordonnance";

        }
       
        private async Task BindData()
        {
            var list = (await new StorageService<Ordonnance>().GetList()).Where(x => x.PatientId == Patient.Id);

            if(list != null&& list.Any())
            {
                list = list.OrderByDescending((arg) => arg.CreatedAt);

                var count = list.Count();

                for (int i = 0; i < list.Count(); i++)
                {
                    list.ElementAt(i).Index = (count-i);
                }
            }

            Ordonnances = new ObservableCollection<Ordonnance>(list);

            var Dlist = (await new StorageService<Document>().GetList()).Where(x => x.PatientId == Patient.Id);
            if (Dlist != null && Dlist.Any())
            {
                Dlist = Dlist.OrderByDescending((arg) => arg.CreatedAt);
            }


            Documents = new ObservableCollection<Document>(Dlist);
        }

        public Command EditPatient
        {
            get
            {
                return new Command(async () =>
                {
                    await CoreMethods.PushPageModel<AddEditPatientViewModel>(Patient);
                });
            }
        }

        public ICommand OrdonnancesCommand
        {
            get
            {
                return new Command(() =>
                {
                    ShowStackDocument = false;
                    ShowStackOrdonnance = true;
                    ShowBoxViewOrdonnances = true;
                    ShowBoxViewDocuments = false;
                    BottomTitle = "+ Ajouter une ordonnance";
                });
            }
        }

        public ICommand DocumentsCommand
        {
            get
            {
                return new Command(() =>
                {
                    ShowStackDocument = true;
                    ShowStackOrdonnance = false;
                    ShowBoxViewDocuments = true;
                    ShowBoxViewOrdonnances = false;
                    BottomTitle = "+ Ajouter un document";
                });
            }
        }

        public Command AddOrdonnanceCommand
        {
            get
            {
                return new Command(async () =>
                {
                    string Ordo_action = null;

                    if (BottomTitle == "+ Ajouter une ordonnance")
                    {
                        Ordo_action =
                                       await CoreMethods.DisplayActionSheet(null, "Annuler", null, "Ordonnance rapide",
                                       "Ordonnance classique");

                        if (string.IsNullOrWhiteSpace(Ordo_action) || Ordo_action == "Annuler")
                            return;
                    }

                    //else
                    //{
                    var _documentPath = string.Empty;
                    var action = await CoreMethods.DisplayActionSheet(null, "Annuler", null, "Appareil photo", "Bibliothèque photo");

                    if (action == "Appareil photo")
                    {
                        await CrossMedia.Current.Initialize();

                        if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                        {
                            await CoreMethods.DisplayAlert("L'appareil photo n'est pas disponible", null, "OK");
                            return;
                        }

                        var permission = await App.AskForCameraPermission();
                        if (permission)
                        {
                            await CrossMedia.Current.Initialize();
                            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions() { SaveToAlbum = false, Directory = "Docs", Name = DateTime.Now.Ticks.ToString(), CompressionQuality = 30 });
                            if (file != null)
                            {

                                if (BottomTitle == "+ Ajouter une ordonnance")
                                {
                                    if (Ordo_action == "Ordonnance rapide")
                                    {
                                        await AddQuickPrescription(file.Path);
                                    }
                                    else
                                    {
                                        await CoreMethods.PushPageModel<OrdonnanceCreateEditViewModel>(file.Path, true);
                                        MessagingCenter.Send(this, Events.PatientDetailsPageSetPatientToOrdonnance, Patient);
                                    }
                                }
                                else
                                {
                                    _documentPath = file.Path;
                                    await CoreMethods.PushPageModel<AddDocumentViewModel>(Patient,true,false);
                                    MessagingCenter.Send(this, Events.DocumentPathFromPatientDetail, _documentPath);
                                }
                            }
                        }
                    }
                    else if (action == "Bibliothèque photo")
                    {
                        await CrossMedia.Current.Initialize();

                        var pickerOptions = new PickMediaOptions() { CompressionQuality = 30 };

                        if (await App.AskForPhotoPermission())
                        {
                            var file = await CrossMedia.Current.PickPhotoAsync(pickerOptions);
                            if (file != null)
                            {
                                if (BottomTitle == "+ Ajouter une ordonnance")
                                {
                                    if (Ordo_action == "Ordonnance rapide")
                                    {
                                        await AddQuickPrescription(file.Path);
                                    }
                                    else
                                    {
                                        await CoreMethods.PushPageModel<OrdonnanceCreateEditViewModel>(file.Path, true);
                                        MessagingCenter.Send(this, Events.PatientDetailsPageSetPatientToOrdonnance, Patient);
                                    }
                                }
                                else
                                {                                   
                                    _documentPath = file.Path;
                                    await CoreMethods.PushPageModel<AddDocumentViewModel>(Patient,true,false);
                                    MessagingCenter.Send(this, Events.DocumentPathFromPatientDetail, _documentPath);
                                }
                            }
                        }
                    }

                    //}

                });
            }
        }

        private async Task AddQuickPrescription(string documentPath)
        {
            var ordannance = new Ordonnance()
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                IsSynced = false,
                UpdatedAt = null,
                First_Care_At = 0,
                Attachments = new List<string>() { documentPath },
                Frequencies = new List<Frequency>(),
                Patient = Patient,
                PatientId = Patient.Id,
                PatientName = Patient.Fullname
            };

            await new StorageService<Ordonnance>().AddAsync(ordannance);

            if (App.IsConnected())
            {
                UserDialogs.Instance.ShowLoading("Chargement...");
                new StorageService<Ordonnance>().PushOrdonnance(ordannance, true);
                UserDialogs.Instance.HideLoading();
            }

            BindData();

            await ToastService.Show("Votre ordonnance a bien été enregistrée !");

        }


        private Ordonnance _selectedOrdonnance;
        public Ordonnance SelectedOrdonnance
        {
            get { return _selectedOrdonnance; }
            set
            {
                _selectedOrdonnance = value;
                RaisePropertyChanged();
                if (_selectedOrdonnance != null)
                    OrdonnanceSelectCommand.Execute(null);
            }
        }

        public ICommand OrdonnanceSelectCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await CoreMethods.PushPageModel<OrdonnanceCreateEditViewModel>(SelectedOrdonnance, true);

                });
            }
        }

        private Document _selectedDocument;
        public Document SelectedDocument
        {
            get { return _selectedDocument; }
            set
            {
                _selectedDocument = value;
                RaisePropertyChanged();
                if (_selectedDocument != null)
                    DocumentSelectCommand.Execute(null);
            }
        }

        private ICommand DocumentSelectCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await CoreMethods.PushPageModel<AddDocumentViewModel>(SelectedDocument,true,false);
                });
            }
        }

        public Command DeleteOrdo => new Command(async (obj) =>
        {            
            Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Chargement...");

            var response = await App.OrdonnanceManager.DeleteItemAsync((string)obj);

            if(response)
            {
                await new StorageService<Ordonnance>().DeleteItemAsync(typeof(Ordonnance).Name + "_" + (string)obj);
            }

            await BindData();

            Acr.UserDialogs.UserDialogs.Instance.HideLoading();

            await ToastService.Show("L’ordonnance a été supprimée avec succès");
        });


        public ICommand CallCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (Patient.PhoneNumbers != null && Patient.PhoneNumbers.Any())
                    {
                        var contactPhone = await CoreMethods.DisplayActionSheet("Numéro(s) de téléphone", "Annuler", null, Patient.PhoneNumbers.ToArray());
                        if (contactPhone != "Annuler")
                            Device.OpenUri(new System.Uri($"tel:{contactPhone}"));
                    }
                });
            }
        }

		protected override void ViewIsAppearing(object sender, System.EventArgs e)
		{
			base.ViewIsAppearing(sender, e);
            BindData();	
		}

    }
}
