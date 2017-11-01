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
using System.Linq;
using libermedical.Helpers;
using Newtonsoft.Json;
using Acr.UserDialogs;

namespace libermedical.ViewModels
{
	public class AddEditPatientViewModel : FreshBasePageModel
	{
		private readonly IStorageService<Patient> _storageService;
		public Patient PatientProperty { get; set; }
		private bool _isNew;

		private ObservableCollection<string> _phones;

		public ObservableCollection<string> Phones
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
			set
			{
				_phoneNo = value;
				RaisePropertyChanged();
			}
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
				PatientProperty.PhoneNumbers = new List<string>();
                PatientProperty.Id = DateTime.UtcNow.Ticks.ToString();
				Phones = new ObservableCollection<string>();
				_isNew = true;

			}
			else
			{
				PatientProperty = initData as Patient;
				Phones = new ObservableCollection<string>(PatientProperty.PhoneNumbers);
				_isNew = false;
			}
		}

		public ICommand AddPhoneCommand => new Command(() =>
		{
			if (!string.IsNullOrEmpty(PhoneNo))
				Phones.Add(PhoneNo);
			PhoneNo = string.Empty;
		});

		public ICommand DeletePhoneCommand => new Command((args) =>
		{
			if (PatientProperty.PhoneNumbers != null)
				PatientProperty.PhoneNumbers.Remove(args as string);
			Phones.Remove(args as string);
		});

		public ICommand CancelCommand => new Command(async () =>
		{
			await CoreMethods.PopPageModel(true);
		});


		public ICommand SaveCommand => new Command(async () =>
		{
			try
			{
				bool isNew = false;
				if (!string.IsNullOrEmpty(PhoneNo))
					PatientProperty.PhoneNumbers = new List<string>() { PhoneNo };

				if (Phones.Count > 0)
					PatientProperty.PhoneNumbers = Phones.ToList();

				if (ValidateForm())
				{
					isNew = PatientProperty.CreatedAt == null ? true : false;
                    PatientProperty.CreatedAt = PatientProperty.CreatedAt == null ? DateTimeOffset.UtcNow : PatientProperty.CreatedAt;
					
                    if (!_isNew && PatientProperty.UpdatedAt != null)
                        PatientProperty.UpdatedAt = DateTimeOffset.UtcNow;
				
                    PatientProperty.IsSynced = false;
					PatientProperty.NurseId = JsonConvert.DeserializeObject<User>(Settings.CurrentUser).Id;
					await _storageService.DeleteItemAsync(typeof(Patient).Name + "_" + PatientProperty.Id);
					await _storageService.AddAsync(PatientProperty);
					if (App.IsConnected())
					{
						UserDialogs.Instance.ShowLoading("Processing...");
                        await new StorageService<Patient>().PushPatient(PatientProperty, _isNew && PatientProperty.UpdatedAt != null);
					}
					await CoreMethods.PopPageModel(PatientProperty);

				}
				else
				{
					await CoreMethods.DisplayAlert("Liber Medical", "Veuillez remplir les informations", "Ok");
				}



			}
			catch (Exception e)
			{
				Debug.WriteLine(e.Message);
			}
			finally
			{
				UserDialogs.Instance.HideLoading();
			}

		});

		private bool ValidateForm()
		{
			if (!string.IsNullOrEmpty(PatientProperty.FirstName) && !string.IsNullOrEmpty(PatientProperty.LastName) && PatientProperty.PhoneNumbers.Count > 0)
				return true;
			return false;
		}
	}
}
