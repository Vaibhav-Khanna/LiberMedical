using System;
using System.Collections.Generic;
using System.Windows.Input;
using libermedical.Models;
using libermedical.Services;
using libermedical.ViewModels.Base;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using libermedical.Enums;
using System.Linq;
using libermedical.Managers;

namespace libermedical.ViewModels
{
	public class OrdonnanceCreateEditViewModel : ViewModelBase
	{
		private string[] _frequenciesAll = new string[] { "Matin", "Midi", "Après-midi", "Soir" };
		private bool _isNew;
		private bool _isEditing;
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


		private ObservableCollection<Frequency> _frequencies;
		public ObservableCollection<Frequency> Frequencies
		{
			get { return _frequencies; }
			set
			{
				_frequencies = value;
				RaisePropertyChanged();
			}
		}

        private ObservableCollection<string> _attachments;
        public ObservableCollection<string> Attachments
        {
            get { return _attachments; }
            set
            {
                _attachments = value;
                RaisePropertyChanged();
            }
        }
        
        public string PatientLabel { get; set; } = "Choisissez un patient";

		private Ordonnance _ordonnance;
		public Ordonnance Ordonnance
		{
			get { return _ordonnance; }
			set
			{
				_ordonnance = value;
				RaisePropertyChanged();
			}
		}
		public bool Creating;
		public string SaveLabel { get; set; } = "Enregistrer";

		public OrdonnanceCreateEditViewModel()
		{
			Ordonnance = new Ordonnance
			{
				Id = Guid.NewGuid().ToString(),
				CreatedAt = DateTime.Today,
				Attachments = new List<string>(),
				Frequencies = new List<Frequency>(),
				First_Care_At = App.ConvertToUnixTimestamp(DateTime.Now)
			};

			SubscribeMessages();

			MessagingCenter.Subscribe<PatientListViewModel, Patient>(this, Events.OrdonnancePageSetPatientForOrdonnance, (sender, patient) =>
			{
				if (patient != null)
				{
					Ordonnance.Patient = patient;
					Ordonnance.PatientId = patient.Id;
					Ordonnance.PatientName = patient.Fullname;
					PatientLabel = patient.Fullname;
				}
			});
			MessagingCenter.Subscribe<DetailsPatientListViewModel, Patient>(this, Events.PatientDetailsPageSetPatientToOrdonnance, (sender, patient) =>
			{
				if (patient != null)
				{
					Ordonnance.Patient = patient;
					Ordonnance.PatientId = patient.Id;
					Ordonnance.PatientName = patient.Fullname;
					PatientLabel = patient.Fullname;
				}
			});
		}

		public override async void Init(object initData)
		{
			base.Init(initData);
			if (initData != null)
			{
				if (initData is string)
				{
					if (!string.IsNullOrEmpty((string)initData))
						Ordonnance.Attachments.Add((string)initData);
					else
						Ordonnance.Attachments = new List<string>();
					Creating = true;
					SaveLabel = "Enregistrer";
					CanEdit = true;
					_isNew = true;
				}
				else
				{
					SaveLabel = "Modifier";
					var ordonnance = initData as Ordonnance;
					if (ordonnance.Id != null)
						Ordonnance = await new StorageService<Ordonnance>().GetItemAsync($"Ordonnance_{ordonnance.Id}");
					Ordonnance.Patient = ordonnance.Patient;
					Ordonnance.PatientId = ordonnance.PatientId;
					Ordonnance.PatientName = ordonnance.PatientName;
					PatientLabel = Ordonnance?.PatientName;
					Frequencies = Ordonnance.Frequencies != null ? new ObservableCollection<Frequency>(Ordonnance.Frequencies) : new ObservableCollection<Frequency>();
                    Attachments = Ordonnance.Attachments != null ? new ObservableCollection<string>(Ordonnance.Attachments) : new ObservableCollection<string>();
					_isNew = false;
                    Creating = false;
					_isEditing = true;
				}
				MessagingCenter.Send(this, Events.UpdateFrequenciesViewCellHeight, Ordonnance.Frequencies);
                MessagingCenter.Send(this, Events.UpdateAttachmentsViewCellHeight, Ordonnance.Attachments);

            }
		}
		public ICommand ViewOrdonnance => new Command(async () =>
		{
			if (Ordonnance.Attachments != null && Ordonnance.Attachments.Count > 0)
			{
				if (Ordonnance.Attachments[0].Contains(".pdf"))
					await CoreMethods.PushPageModel<SecuriseBillsViewModel>(Ordonnance, true);
				else
					await CoreMethods.PushPageModel<OrdonnanceViewViewModel>(Ordonnance, true);
			}
		});

        public ICommand ViewAttachment => new Command(async (args) =>
        {
            if (args != null)
            {
                if (args.ToString().Contains(".pdf"))
                    await CoreMethods.PushPageModel<SecuriseBillsViewModel>(Ordonnance, true);
                else
                    await CoreMethods.PushPageModel<OrdonnanceViewViewModel>(args, true);
            }
        });

        public ICommand CloseCommand => new Command(async () =>
		{
			await CoreMethods.PopPageModel(null, true);
		});

		public ICommand SelectPatientCommand => new Command(async () =>
		{
			if (!_isEditing)
				await CoreMethods.PushPageModel<PatientListViewModel>(new[] { "OrdonanceSelectPatient", "normal", "ordonnance" }, true);
		});

		public ICommand SaveCommand => new Command(async () =>
		{
			if (CanEdit)
			{
                var storageService = new StorageService<Ordonnance>();
				await storageService.DeleteItemAsync(typeof(Ordonnance).Name + "_" + Ordonnance.Id);
				Ordonnance.IsSynced = false;
				await storageService.AddAsync(Ordonnance);

				//TODO: Display success toast

				await CoreMethods.PopPageModel(null, true);

                if (App.IsConnected())
                {
                    await storageService.SyncOrdonnances();
                }
            }
			else
			{
				CanEdit = true;
				SaveLabel = "Enregistrer";
			}
		});

		public ICommand AddAttachmentCommand => new Command(async () =>
		{
			if (CanEdit)
			{
				var action = await CoreMethods.DisplayActionSheet(null, "Annuler", null, "Appareil photo", "Bibliothèque photo");

				await CrossMedia.Current.Initialize();
				if (action == "Appareil photo")
				{
					if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
					{
						await CoreMethods.DisplayAlert("L'appareil photo n'est pas disponible", null, "OK");
						return;
					}

					var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions());
					if (file != null)
					{
                        Attachments.Add(file.Path);
                        Ordonnance.Attachments.Add(file.Path);
                        FileUpload.UploadFile(file.Path, "Ordonnances", Ordonnance.Id);
                    }
				}
				else if (action == "Bibliothèque photo")
				{
					var file = await CrossMedia.Current.PickPhotoAsync();
					if (file != null)
					{
                        Attachments.Add(file.Path);
						Ordonnance.Attachments.Add(file.Path);
                        FileUpload.UploadFile(file.Path, "Ordonnances", Ordonnance.Id);
                    }
				}
				MessagingCenter.Send(this,Events.UpdateAttachmentsViewCellHeight,Ordonnance.Attachments);
                
			}
		});

		public ICommand AddFrequenceTappedCommand => new Command(async () =>
		{


			if (CanEdit)
			{
				var frequency = new Frequency();
				string[] availableFrequencies = null;
				if (Frequencies != null)
					availableFrequencies = _frequenciesAll.Except(Frequencies.Select(x => x.PeriodString).ToArray()).ToArray();
				else
					availableFrequencies = _frequenciesAll;
				var action = await CoreMethods.DisplayActionSheet("Choisissez la fréquence d'administration", "Annuler", null,availableFrequencies);

				switch (action)
				{
					case "Matin":
						frequency.Period = PeriodEnum.morning;
						break;
					case "Midi":
						frequency.Period = PeriodEnum.lunch;
						break;
					case "Après-midi":
						frequency.Period = PeriodEnum.afternoon;
						break;
					case "Soir":
						frequency.Period = PeriodEnum.evening;
						break;

				}

				if (action == "Annuler")
					return;

				await CoreMethods.PushPageModel<OrdonnanceFrequence2ViewModel>(frequency, true);
				SubscribeFrequencyMessages();
				MessagingCenter.Send(this, Events.EnableCotationsEditMode, true);
			}


		});

		public ICommand EditCommand => new Command(async =>
		{
			CanEdit = true;
		});

		public ICommand ModifyFrequenceTappedCommand => new Command(async frequency =>
		{
			await CoreMethods.PushPageModel<OrdonnanceFrequence2ViewModel>(frequency, true);
			if (CanEdit)
				MessagingCenter.Send(this, Events.EnableCotationsEditMode, true);

		});

		private void SubscribeMessages()
		{
			MessagingCenter.Unsubscribe<OrdonnancesListViewModel, Ordonnance>(this, Events.UpdateOrdonnanceDetails);
			MessagingCenter.Subscribe<OrdonnancesListViewModel, Ordonnance>(this, Events.UpdateOrdonnanceDetails,
				async (sender, ordonnance) =>
				{
					if (ordonnance != null)
					{
						Ordonnance = ordonnance;
						if (ordonnance.Id != null)
							Ordonnance = await new StorageService<Ordonnance>().GetItemAsync($"Ordonnance_{ordonnance.Id}");
						Ordonnance.Patient = ordonnance.Patient;
						Ordonnance.PatientId = ordonnance.PatientId;
						Ordonnance.PatientName = ordonnance.PatientName;
						PatientLabel = Ordonnance?.PatientName;
						Frequencies = Ordonnance.Frequencies != null ? new ObservableCollection<Frequency>(Ordonnance.Frequencies) : new ObservableCollection<Frequency>();
						Creating = false;
						SaveLabel = "Modifier";
						MessagingCenter.Send(this, Events.UpdateFrequenciesViewCellHeight, Frequencies);
					}
				});

			MessagingCenter.Unsubscribe<DetailsPatientListViewModel, Ordonnance>(this, Events.UpdateOrdonnanceDetails);
			MessagingCenter.Subscribe<DetailsPatientListViewModel, Ordonnance>(this, Events.UpdateOrdonnanceDetails,
				(sender, ordonnance) =>
				{
					if (ordonnance != null)
					{
						Ordonnance = ordonnance;
					}
				});
		}

		private void SubscribeFrequencyMessages()
		{
			MessagingCenter.Subscribe<OrdonnanceFrequence2ViewModel, Frequency>(this, Events.UpdateFrequencies, ((sender, args) =>
			{

				if (args != null)
				{
					var frequency = args as Frequency;
					if (Ordonnance.Frequencies != null)
						Ordonnance.Frequencies.Add(frequency);
					Frequencies = Ordonnance.Frequencies != null ? new ObservableCollection<Frequency>(Ordonnance.Frequencies) : new ObservableCollection<Frequency>(new List<Frequency>() { frequency });

					MessagingCenter.Send(this, Events.UpdateFrequenciesViewCellHeight, Frequencies);
				}

				MessagingCenter.Unsubscribe<OrdonnanceFrequence2ViewModel, Frequency>(this, Events.UpdateFrequencies);
			}));
		}
	}
}
