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
using System.Diagnostics;
using Acr.UserDialogs;
using libermedical.PopUp;

namespace libermedical.ViewModels
{

	public class OrdonnanceCreateEditViewModel : ViewModelBase
	{
		private string[] _frequenciesAll = new string[] { "Matin", "Midi", "Après-midi", "Soir" };
		private bool _isNew;		
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

        bool showbutton = false;
        public bool ShowButton
        {
            get { return showbutton; }
            set
            {
                showbutton = value;
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

        public Command DeleteImage => new Command((obj) =>
        {
            if (CanEdit)
            {
                if (Attachments.Contains((string)obj))
                    Attachments.Remove((string)obj);

                if (Ordonnance.Attachments.Contains((string)obj))
                    Ordonnance?.Attachments.Remove((string)obj);
            }        
        });


       public Command DeleteFrequency => new Command((obj) =>
       {            
            if (CanEdit)
            {
                if (Frequencies.Contains(((obj as Frequency))))
                    Frequencies.Remove((obj as Frequency));

                if (Ordonnance.Frequencies.Contains((obj as Frequency)))
                    Ordonnance?.Frequencies.Remove((obj as Frequency));

                //Frequencies = new ObservableCollection<Frequency>(Frequencies.ToList());
                //RaisePropertyChanged(nameof(Frequencies));
            } 
       });

		public OrdonnanceCreateEditViewModel()
		{
			Attachments = new ObservableCollection<string>();
            Frequencies = new ObservableCollection<Frequency>();

			Ordonnance = new Ordonnance
			{
				Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow,
				Attachments = new List<string>(),
				Frequencies = new List<Frequency>(),
                First_Care_At = App.ConvertToUnixTimestamp(DateTime.UtcNow)
			};

			SubscribeMessages();

            MessagingCenter.Unsubscribe<PatientListViewModel, Patient>(this, Events.OrdonnancePageSetPatientForOrdonnance);
			MessagingCenter.Subscribe<PatientListViewModel, Patient>(this, Events.OrdonnancePageSetPatientForOrdonnance, (sender, patient) =>
			{
                if (patient != null && Ordonnance!=null && PatientLabel!=null)
				{
					Ordonnance.Patient = patient;
					Ordonnance.PatientId = patient.Id;
					Ordonnance.PatientName = patient.Fullname;
					PatientLabel = patient.Fullname;
				}
			});

            MessagingCenter.Unsubscribe<DetailsPatientListViewModel, Patient>(this, Events.PatientDetailsPageSetPatientToOrdonnance);
			MessagingCenter.Subscribe<DetailsPatientListViewModel, Patient>(this, Events.PatientDetailsPageSetPatientToOrdonnance, (sender, patient) =>
			{
                if (patient != null && Ordonnance != null && PatientLabel != null)
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
                    {
                        Ordonnance.Attachments.Add((string)initData);
                        Attachments.Add((string)initData);
                    }
                    else
                        Ordonnance.Attachments = new List<string>();
                    Creating = true;
                    SaveLabel = "Enregistrer";
                    ShowButton = true;
                    CanEdit = true;
                    _isNew = true;
                }
                else
                {
                    SaveLabel = "Modifier";
                    var ordonnance = initData as Ordonnance;

                    //if (ordonnance.Id != null)
                        //Ordonnance = await new StorageService<Ordonnance>().GetItemAsync($"Ordonnance_{ordonnance.Id}");

                    if (ordonnance != null)
                        Ordonnance = ordonnance;

                    Ordonnance.Patient = ordonnance.Patient;
                    Ordonnance.PatientId = ordonnance.PatientId;
                    Ordonnance.PatientName = ordonnance.PatientName;
                    PatientLabel = Ordonnance?.PatientName;

                    if (Ordonnance.Status == Enums.StatusEnum.valid.ToString())
                        ShowButton = false;
                    else
                        ShowButton = true;

                    Frequencies = Ordonnance.Frequencies != null ? new ObservableCollection<Frequency>(Ordonnance.Frequencies) : new ObservableCollection<Frequency>();
                    Attachments = Ordonnance.Attachments != null ? new ObservableCollection<string>(Ordonnance.Attachments) : new ObservableCollection<string>();
                                
                    _isNew = false;
                    Creating = false;
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
            if (CanEdit)
            {
                if (Ordonnance.Status != Enums.StatusEnum.valid.ToString())
                    await CoreMethods.PushPageModel<PatientListViewModel>(new[] { "OrdonanceSelectPatient", "normal", "ordonnance" }, true);
            }
		});

		public ICommand SaveCommand => new Command(async () =>
		{
			try
			{
				if (CanEdit)
				{
                    var stored = await new StorageService<Ordonnance>().GetItemAsync($"Ordonnance_{Ordonnance.Id}");

                    if (!_isNew && stored == null)
                    {
                        MessagingCenter.Send(this, "RefreshOrdoList");
                        await CoreMethods.PopPageModel(null, true);
                        return;
                    }
                        

                    if (Ordonnance.Patient == null || string.IsNullOrWhiteSpace(Ordonnance.PatientName))
                    {
                        await CoreMethods.DisplayAlert("Veuillez d'abord sélectionner un patient", "", "Ok");
                        return;
                    }

                    if(Attachments?.Count()<=0)
                    {
                        await CoreMethods.DisplayAlert("S'il vous plaît ajouter un atleast une pièce jointe", "", "Ok");
                        return;
                    }


                    if (Frequencies == null || Frequencies?.Count() <= 0)
                    {
                        await CoreMethods.DisplayAlert( "Veuillez ajouter au moins une fréquence", "", "Ok");
                        return;
                    }

                    UserDialogs.Instance.ShowLoading("Chargement...");
					var storageService = new StorageService<Ordonnance>();

					await storageService.DeleteItemAsync(typeof(Ordonnance).Name + "_" + Ordonnance.Id);
					
                    Ordonnance.Attachments = Attachments?.ToList();
					Ordonnance.Frequencies = Frequencies?.ToList();
					Ordonnance.IsSynced = false;

                    if (!_isNew && Ordonnance.UpdatedAt != null)
                        Ordonnance.UpdatedAt = DateTimeOffset.UtcNow;

					await storageService.AddAsync(Ordonnance);
                    	
					if (App.IsConnected())
					{
                        storageService.PushOrdonnance(Ordonnance, _isNew && Ordonnance.UpdatedAt == null );
					}

					await CoreMethods.PopPageModel(null, true);

					UserDialogs.Instance.HideLoading();

                    await ToastService.Show("Votre ordonnance a bien été enregistrée !");
                               
                    MessagingCenter.Send(this,"RefreshOrdoList");
				}
				else
				{
                    if (Ordonnance.Status != Enums.StatusEnum.valid.ToString())
                    {
                        CanEdit = true;
                        SaveLabel = "Enregistrer";
                    }
				}

			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
			finally
			{

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

                    var per = await App.AskForCameraPermission();
                    if (per)
                    {
                        await CrossMedia.Current.Initialize();
                        var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                        { Directory = "Docs", Name = DateTime.Now.Ticks.ToString(), CompressionQuality = 70,SaveToAlbum = false });
                        if (file != null)
                        {
                            Attachments.Add(file.Path);
                            Ordonnance.Attachments.Add(file.Path);
                        }
                    }
				}
				else if (action == "Bibliothèque photo")
				{
                    if (await App.AskForPhotoPermission())
                    {
                        var pickerOptions = new PickMediaOptions() { CompressionQuality = 30 };
                        var file = await CrossMedia.Current.PickPhotoAsync(pickerOptions);
                        if (file != null)
                        {
                            Attachments.Add(file.Path);
                            Ordonnance.Attachments.Add(file.Path);
                        }
                    }
				}
				MessagingCenter.Send(this, Events.UpdateAttachmentsViewCellHeight, Ordonnance.Attachments);

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
				var action = await CoreMethods.DisplayActionSheet("Choisissez la fréquence d'administration", "Annuler", null, availableFrequencies);

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

                await CoreMethods.PushPageModel<OrdonnanceFrequence2ViewModel>(new Tuple<Frequency,bool>(frequency,true), true);
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
            await CoreMethods.PushPageModel<OrdonnanceFrequence2ViewModel>(new Tuple<Frequency,bool>((frequency as Frequency), CanEdit), true);
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
            MessagingCenter.Unsubscribe<OrdonnanceFrequence2ViewModel, Frequency>(this, Events.UpdateFrequencies);
            MessagingCenter.Subscribe<OrdonnanceFrequence2ViewModel, Frequency>(this, Events.UpdateFrequencies, async (sender, args) =>
			{

				if (args != null)
				{
					var frequency = args as Frequency;

                    if (Ordonnance.Frequencies != null)
                    {
                        if (Ordonnance.Frequencies.Contains(frequency))
                        {

                        }
                        else
                        {
                            
                            Ordonnance.Frequencies.Add(frequency);
                        }
                    }

                    if (Frequencies.Contains(frequency))
                    {
                      
                    }
                    else
                    {
                        Frequencies.Add(frequency);
                        await ToastService.Show("Fréquence ajoutée avec succès"); 
                        MessagingCenter.Send(this, Events.UpdateFrequenciesViewCellHeight, Frequencies);
                    }

                   
				}
               	
			});
		}
	}

}
