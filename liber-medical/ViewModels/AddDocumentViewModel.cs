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

namespace libermedical.ViewModels
{
	public class AddDocumentViewModel : ViewModelBase
	{
		private string _createdDate = "Date de premier soin";
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
				else if (initData is Document)
				{
					Document = initData as Document;
					CreatedDate = Document.AddDate.ToString("dd-MM-yyyy");
					ImagePath = Document.AttachmentPath;
					Label = Document.Label;
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
					Document.Label = Label;
					await storageService.AddAsync(Document);
					await CoreMethods.PopPageModel();
				});
			}
		}
		public ICommand AddDocumentCommand
		{
			get
			{
				return new Command(async () =>
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
						var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions());
						if (file != null)
						{
							var profilePicture = ImageSource.FromStream(() => file.GetStream());
							var typeNavigation = "normal";
							Document.AttachmentPath = file.Path;
						}

					}
					else if (action == "Bibliothèque photo")
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
					}

					ImagePath = Document.AttachmentPath;

				});
			}
		}
	}
}
