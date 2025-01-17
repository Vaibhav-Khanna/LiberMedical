using libermedical.Helpers;
using libermedical.Models;
using libermedical.Services;
using libermedical.ViewModels.Base;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Windows.Input;
using Xamarin.Forms;
using Acr.UserDialogs;
using System.Diagnostics;
using System.Threading.Tasks;
using libermedical.Request;
using libermedical.PopUp;
using libermedical.Enums;

namespace libermedical.ViewModels
{
    public class AddDocumentViewModel : ViewModelBase
    {
        private bool _isNew;
        private string _createdDate = DateTime.UtcNow.ToString("dd-MM-yyyy");

        public string CreatedDate
        {
            get { return _createdDate; }
            set
            {
                _createdDate = value;
                RaisePropertyChanged();
            }
        }

        private string _imagePath;
        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                _imagePath = value;
                RaisePropertyChanged();
            }
        }

        private string _label;
        public string Label
        {
            get { return _label; }
            set
            {
                _label = value;
                RaisePropertyChanged();
            }
        }

        private Document _document;
        public Document Document
        {
            get { return _document; }
            set
            {
                _document = value;
                RaisePropertyChanged();
            }
        }

        private string _optionText;
        public string OptionText
        {
            get { return _optionText; }
            set
            {
                _optionText = value;
                RaisePropertyChanged();
            }
        }

        private bool _canEdit;
        public bool CanEdit
        {
            get { return _canEdit; }
            set
            {
                _canEdit = value;
                RaisePropertyChanged();
            }
        }

        private bool _isRunning = false;
        public bool IsRunning
        {
            get { return _isRunning; }
            set
            {
                _isRunning = value;
                RaisePropertyChanged();
            }
        }

        public AddDocumentViewModel()
        {
            Document = new Document
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
                AddDate = DateTime.UtcNow,
                NurseId = JsonConvert.DeserializeObject<User>(Settings.CurrentUser).Id,
            };

            SubscribeMessage();

        }

        private void SubscribeMessage()
        {
            MessagingCenter.Unsubscribe<DetailsPatientListViewModel, string>(this, Events.DocumentPathFromPatientDetail);
            MessagingCenter.Subscribe<DetailsPatientListViewModel, string>(this, Events.DocumentPathFromPatientDetail, (sender, args) =>
            {
                ImagePath = args;
                if (Document != null)
                    Document.AttachmentPath = ImagePath;
            });
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            IsRunning = false;

            if (initData != null)
            {
                if (initData is Patient)
                {
                    var patient = initData as Patient;
                    Document.Patient = patient;
                    Document.PatientId = patient.Id;
                    OptionText = "Enregistrer";
                    CanEdit = true;
                    _isNew = true;
                }
                else if (initData is Document)
                {
                    Document = initData as Document;
                    CreatedDate = Document.AddDate.ToString("dd-MM-yyyy");
                    ImagePath = GetDocumentPath(Document.AttachmentPath);
                    Label = Document.Label;
                    OptionText = "Modifier";
                    _isNew = false;
                    CanEdit = false;

                    if (Document.Status == DocumentStatusEnum.valid.ToString())
                    {
                        OptionText = "";
                    }

                    if (Document.UpdatedAt == null)
                    {
                        OptionText = "Enregistrer";
                        CanEdit = true;
                        _isNew = true;
                    }
                }
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (CanEdit)
                    {
                        CanEdit = false;
                        OptionText = "Modifier";
                    }
                    else
                        await CoreMethods.PopPageModel(true, false);
                });
            }
        }

        public ICommand AddCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (OptionText == "Modifier")
                        {
                            if (Document.Status != DocumentStatusEnum.valid.ToString())
                            {
                                OptionText = "Enregistrer";
                                CanEdit = true;
                            }
                        }
                        else if (OptionText == "Enregistrer")
                        {
                            var storageService = new StorageService<Document>();
                            await storageService.DeleteItemAsync(typeof(Document).Name + "_" + Document.Id);
                            Document.AttachmentPath = ImagePath;
                            Document.Label = Label;
                            Document.IsSynced = false;
                            Document.Status = DocumentStatusEnum.waiting.ToString();

                            if (_isNew && Document.UpdatedAt != null)
                                Document.UpdatedAt = DateTimeOffset.UtcNow;

                            await storageService.AddAsync(Document);

                            if (App.IsConnected())
                            {
                                UserDialogs.Instance.ShowLoading("Chargement...");
                                await new StorageService<Document>().PushDocument(Document, _isNew && Document.UpdatedAt != null);
                                await DownlaodDocuments();
                                UserDialogs.Instance.HideLoading();
                            }

                            await CoreMethods.PopPageModel(true, false);

                            await ToastService.Show("Informations enregistrées avec succès ");
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                    finally
                    {

                    }
                });
            }
        }

        private async Task DownlaodDocuments()
        {
            if (App.IsConnected())
            {
                var request = new GetListRequest(10, 0);

                var documents = await App.DocumentsManager.GetListAsync(request);

                documents = await App.DocumentsManager.GetListAsync(new GetListRequest(documents.total, 0));

                //Updating records in local cache
                if (documents != null && documents.rows != null)
                    await new StorageService<Document>().InvalidateSyncedItems();

                await new StorageService<Document>().AddManyAsync(documents.rows);
            }
        }


        public ICommand AddDocumentCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (CanEdit)
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
                            var permission = await App.AskForCameraPermission();

                            if (permission)
                            {

                                await CrossMedia.Current.Initialize();
                                IsRunning = true;
                                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                                { Directory = "Docs", Name = DateTime.Now.Ticks.ToString(), CompressionQuality = 70, SaveToAlbum = false });

                                if (file != null)
                                {

                                    var profilePicture = ImageSource.FromStream(() => file.GetStream());
                                    Document.AttachmentPath = file.Path;

                                }
                                IsRunning = false;

                            }

                        }
                        else if (action == "Bibliothèque photo")
                        {
                            await CrossMedia.Current.Initialize();

                            if (await App.AskForPhotoPermission())
                            {

                                IsRunning = true;
                                var pickerOptions = new PickMediaOptions() { CompressionQuality = 30 };
                                var file = await CrossMedia.Current.PickPhotoAsync(pickerOptions);
                                if (file != null)
                                {

                                    var profilePicture = ImageSource.FromStream(() => file.GetStream());
                                    Document.AttachmentPath = file.Path;

                                }
                                IsRunning = false;

                            }
                        }

                        ImagePath = Document.AttachmentPath;
                    }
                    else
                    {
                        if (Document.AttachmentPath != null)
                        {
                            if (Document.AttachmentPath.Contains(".pdf"))
                                await CoreMethods.PushPageModel<SecuriseBillsViewModel>(Document, true);
                            else
                                await CoreMethods.PushPageModel<OrdonnanceViewViewModel>(Document, true);
                        }
                    }

                });
            }
        }

        private string GetDocumentPath(string path)
        {
            if (!string.IsNullOrEmpty(path))
                if (path.StartsWith("Ordonnance/") || path.StartsWith("PatientDocuments/"))
                {
                    return $"{Constants.RestUrl}file?path={System.Net.WebUtility.UrlEncode(path)}&token={Settings.Token}";
                }
                else
                    return path;
            else
                return string.Empty;
        }

    }
}
