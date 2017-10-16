﻿using libermedical.Helpers;
using libermedical.Models;
using libermedical.Services;
using libermedical.ViewModels.Base;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace libermedical.ViewModels
{
    public class AddDocumentViewModel : ViewModelBase
    {
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
        public AddDocumentViewModel()
        {
            Document = new Document
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Today,
                AddDate = DateTime.Today,
                NurseId = JsonConvert.DeserializeObject<User>(Settings.CurrentUser).Id,
            };
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            if (initData != null)
            {
                if (initData is Patient)
                {
                    var patient = initData as Patient;
                    Document.Patient = patient;
                    Document.PatientId = patient.Id;
                }
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await CoreMethods.PopPageModel();
                });
            }
        }
        public ICommand AddCommand
        {
            get
            {
                return new Command(async () =>
                {
                    var storageService = new StorageService<Document>();                    
                    await storageService.AddAsync(Document);
                    await CoreMethods.PopPageModel();
                });
            }
        }
        public ICommand AddDocumentCommand
        {
            get
            {
                return new Command(async() =>
                {
                    await CrossMedia.Current.Initialize();

                    var pickerOptions = new PickMediaOptions();
                    var file = await CrossMedia.Current.PickPhotoAsync(pickerOptions);
                    if (file != null)
                    {
                        var profilePicture = ImageSource.FromStream(() => file.GetStream());
                        var typeNavigation = "normal";
                        Document.AttachmentPath = file.Path;
                    }
                });
            }
        }
    }
}
