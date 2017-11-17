﻿using FreshMvvm;
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
            BottomTitle = "+ Ajoutez une ordonnance";
            
        }
       
        private async void BindData()
        {
            var list = (await new StorageService<Ordonnance>().GetList()).Where(x => x.PatientId == Patient.Id);
            if(list != null&& list.Any())
            {
                list = list.OrderByDescending((arg) => arg.CreatedAt);
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
                    BottomTitle = "+ Ajoutez une ordonnance";
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
                    BottomTitle = "+ Ajoutez un document";
                });
            }
        }

        public Command AddOrdonnanceCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (BottomTitle == "+ Ajoutez une ordonnance")
                    {
                        await CoreMethods.PushPageModel<OrdonnanceCreateEditViewModel>(string.Empty, true);
                        MessagingCenter.Send(this, Events.PatientDetailsPageSetPatientToOrdonnance, Patient);
                    }
                    else
                    {
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
                                    var profilePicture = ImageSource.FromStream(() => file.GetStream());
                                    _documentPath = file.Path;
                                    await CoreMethods.PushPageModel<AddDocumentViewModel>(Patient);
                                    MessagingCenter.Send(this, Events.DocumentPathFromPatientDetail, _documentPath);
                                }
                            }
                        }
                        else if (action == "Bibliothèque photo")
                        {
                            await CrossMedia.Current.Initialize();

                            var pickerOptions = new PickMediaOptions() {  CompressionQuality = 30 };

                            if (await App.AskForPhotoPermission())
                            {
                                var file = await CrossMedia.Current.PickPhotoAsync(pickerOptions);
                                if (file != null)
                                {
                                    var profilePicture = ImageSource.FromStream(() => file.GetStream());
                                    _documentPath = file.Path;
                                    await CoreMethods.PushPageModel<AddDocumentViewModel>(Patient);
                                    MessagingCenter.Send(this, Events.DocumentPathFromPatientDetail, _documentPath);
                                }
                            }
                        }
                        
                    }
                });
            }
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
                    await CoreMethods.PushPageModel<AddDocumentViewModel>(SelectedDocument);
                });
            }
        }

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
