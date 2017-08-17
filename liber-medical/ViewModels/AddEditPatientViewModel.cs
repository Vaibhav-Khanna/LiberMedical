using System;
using System.Diagnostics;
using System.Windows.Input;
using FreshMvvm;
using libermedical.Models;
using libermedical.Services;
using Xamarin.Forms;

namespace libermedical.ViewModels
{
	public class AddEditPatientViewModel:FreshBasePageModel
	{
		private readonly IStorageService<Patient> _storageService;
		public Patient PatientProperty { get; set; }

		public AddEditPatientViewModel(IStorageService<Patient> storageService)
		{
			_storageService = storageService;
		}

		public override void Init(object initData)
		{
			base.Init(initData);
			PatientProperty = new Patient();
		}


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
			await CoreMethods.PopPageModel(true);

		});
	}
}
