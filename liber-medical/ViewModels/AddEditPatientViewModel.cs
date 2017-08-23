using System;
using System.Diagnostics;
using System.Windows.Input;
using FreshMvvm;
using libermedical.Models;
using libermedical.Services;
using Xamarin.Forms;
using System.Collections.Generic;
using PropertyChanged;

namespace libermedical.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class AddEditPatientViewModel : FreshBasePageModel
    {
        private readonly IStorageService<Patient> _storageService;
        public Patient PatientProperty { get; set; }
        public string PhoneNo { get; set; }

        public AddEditPatientViewModel(IStorageService<Patient> storageService)
        {
            _storageService = storageService;
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            PatientProperty = new Patient();
            PatientProperty.Id = DateTime.Now.Ticks.ToString();
            PatientProperty.PhoneNumbers = new List<PhoneNumber>() { new PhoneNumber() { Number = "1234567", PatientId = PatientProperty.Id } };
        }

        public ICommand AddPhoneCommand => new Command( () =>
        {
            PatientProperty.PhoneNumbers.Add(new PhoneNumber() { Number=PhoneNo,PatientId=PatientProperty.Id });            
            PhoneNo = string.Empty;
        });

        public ICommand CancelCommand => new Command(async () =>
        {
            await CoreMethods.PopPageModel(true);
        });


        public ICommand SaveCommand => new Command(async () =>
        {
            try
            {
                await _storageService.AddAsync(PatientProperty);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            await CoreMethods.PopPageModel(PatientProperty);

        });
    }
}
