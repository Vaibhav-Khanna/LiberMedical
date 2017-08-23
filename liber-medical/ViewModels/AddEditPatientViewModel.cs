using System;
using System.Diagnostics;
using System.Windows.Input;
using FreshMvvm;
using libermedical.Models;
using libermedical.Services;
using Xamarin.Forms;
using System.Collections.Generic;
using PropertyChanged;
using System.Collections.ObjectModel;

namespace libermedical.ViewModels
{
	//[AddINotifyPropertyChangedInterface]
	public class AddEditPatientViewModel : FreshBasePageModel
	{
		private readonly IStorageService<Patient> _storageService;
		public Patient PatientProperty { get; set; }


		private ObservableCollection<PhoneNumber> _phones;

		public ObservableCollection<PhoneNumber> Phones
		{
			get { return _phones; }
			set
			{
				_phones = value;
				RaisePropertyChanged();
			}
		}

		private string _phoneNo;
		public string PhoneNo
		{
			get { return _phoneNo; }
			set { _phoneNo = value; RaisePropertyChanged(); }
		}

		public AddEditPatientViewModel(IStorageService<Patient> storageService)
		{
			_storageService = storageService;
		}

		public override void Init(object initData)
		{
			base.Init(initData);
			if (initData == null)
			{
				PatientProperty = new Patient();
				PatientProperty.Id = DateTime.Now.Ticks.ToString();
				Phones = new ObservableCollection<PhoneNumber>();

			}
			else
			{
				PatientProperty = initData as Patient;
				Phones = new ObservableCollection<PhoneNumber>(PatientProperty.PhoneNumbers);
			}
		}

		public ICommand AddPhoneCommand => new Command(() =>
	   {
		   if (!string.IsNullOrEmpty(PhoneNo))
			   Phones.Add(new PhoneNumber() { Number = PhoneNo, PatientId = PatientProperty.Id });
		   PhoneNo = string.Empty;
	   });

		public ICommand DeletePhoneCommand => new Command((args) =>
	   {
		   Phones.Remove(args as PhoneNumber);
	   });

		public ICommand CancelCommand => new Command(async () =>
		{
			await CoreMethods.PopPageModel(true);
		});


		public ICommand SaveCommand => new Command(async () =>
		{
			try
			{
				PatientProperty.PhoneNumbers = new List<PhoneNumber>(Phones);
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
