using System.Windows.Input;
using libermedical.ViewModels.Base;
using Xamarin.Forms;
using Plugin.Media;
using Plugin.Media.Abstractions;
using libermedical.Models;
using System.Threading.Tasks;
using System;
using libermedical.Services;
using System.Collections.Generic;
using libermedical.Helpers;
using Newtonsoft.Json;
using libermedical.Request;
using Acr.UserDialogs;
using Plugin.Messaging;
using libermedical.PopUp;
using Com.OneSignal;

namespace libermedical.ViewModels
{
	public class HomeViewModel : ViewModelBase
	{
		//type of photo, most of the time ordonannces but we can also have document
		//so we initialize typeDoc to "ordonnace"
		private string typeDoc = "ordonnance";
		private string _documentPath = string.Empty;
		private string AdvisorContact = string.Empty;
        private string _welcomeText;
        public string WelcomeText
        {
            get { return _welcomeText; }
            set
            {
                _welcomeText = value;
                RaisePropertyChanged();
            }
        }


        public HomeViewModel()
        {
            //SubscribeToMessages();

            WelcomeText = $"Bonjour {JsonConvert.DeserializeObject<User>(Settings.CurrentUser).Firstname}, que souhaitez-vous faire?";

            CheckForAdvisor();

            OneSignal.Current.RegisterForPushNotifications();

            Com.OneSignal.Abstractions.IdsAvailableCallback callback = new Com.OneSignal.Abstractions.IdsAvailableCallback(HandleIdsAvailableCallback);

            if (!string.IsNullOrEmpty(Settings.NotificationToken))
            {
                Update();
            }

            OneSignal.Current.IdsAvailable(callback);
        }

        async void Update()
        {
            var CurrentUser = JsonConvert.DeserializeObject<User>(Settings.CurrentUser);

            if (CurrentUser != null)
            {
                CurrentUser.OneSignalId = Settings.NotificationToken;

                var return_user = await App.UserManager.SaveOrUpdateAsync(CurrentUser.Id, CurrentUser, false);

                if (return_user != null)
                    Settings.CurrentUser = JsonConvert.SerializeObject(return_user);
            }
        }

        async void HandleIdsAvailableCallback(string playerID, string pushToken)
        {
            if (!string.IsNullOrWhiteSpace(playerID))
            {
                Settings.NotificationToken = playerID;

                var CurrentUser = JsonConvert.DeserializeObject<User>(Settings.CurrentUser);

                if (CurrentUser != null)
                {
                    CurrentUser.OneSignalId = playerID;

                    var return_user = await App.UserManager.SaveOrUpdateAsync(CurrentUser.Id,CurrentUser,false);

                    if(return_user!=null)
                    Settings.CurrentUser = JsonConvert.SerializeObject(return_user);
                }
            }
        }

		public ICommand AssistCommand => new Command(async () =>
		{
			var action = await CoreMethods.DisplayActionSheet(null, "Annuler", null, "Appel vocal", "E-mail", "SMS");
			
            switch (action)
			{
                case "Appel vocal":
                    if (CrossMessaging.Current.PhoneDialer.CanMakePhoneCall)
                        CrossMessaging.Current.PhoneDialer.MakePhoneCall(Settings.AdvisorContact);                   
                    break;
                case "SMS":
                    var smsMessenger = CrossMessaging.Current.SmsMessenger;
                    if (smsMessenger.CanSendSms)
                        smsMessenger.SendSms(Settings.AdvisorContact, "Bonjour, je vous informe que mon TLA est branché");
                    break;
                case "E-mail":
                    if (CrossMessaging.Current.EmailMessenger.CanSendEmail)
                        CrossMessaging.Current.EmailMessenger.SendEmail(Settings.AdvisorEmail);
                    break; 
			}

		});

		private async Task CheckForAdvisor()
		{
			var request = new GetListRequest(20, 0);

			var userInfo = await App.UserManager.GetAsync($"{JsonConvert.DeserializeObject<User>(Settings.CurrentUser).Id}/advisor");
		
            Settings.AdvisorContact = AdvisorContact = userInfo != null ? userInfo.Phone : string.Empty;
			
            Settings.AdvisorEmail = userInfo != null ? userInfo.Email : string.Empty;
		}

		//This concern only ordonances  in normal process
		public ICommand AddOrdonnanceCommand => new Command(async () =>
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
                    var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                    { Directory = "Docs", Name = DateTime.Now.Ticks.ToString(), CompressionQuality = 70, SaveToAlbum = false });
                    //if photo ok
                    if (file != null)
                    {
                        var profilePicture = ImageSource.FromStream(() => file.GetStream());
                      
                        _documentPath = file.Path;

                        CreatePrescription(_documentPath);

                    }
                }
			}
			else if (action == "Bibliothèque photo")
			{
				await CrossMedia.Current.Initialize();

				var pickerOptions = new PickMediaOptions() { CompressionQuality = 30 };

                if (await App.AskForPhotoPermission())
                {
                    var file = await CrossMedia.Current.PickPhotoAsync(pickerOptions);

                    if (file != null)
                    {
                       
                       
                      
                        _documentPath = file.Path;

                        CreatePrescription(_documentPath);

                    }
                }
			}
		});

        public async void CreatePrescription(string filePath)
        {
            await CoreMethods.PushPageModel<OrdonnanceCreateEditViewModel>(filePath, true);
        }

		//This concern  ordonances or documents in fast process
		public ICommand FastCommand => new Command(async () =>
		{
			var action = await CoreMethods.DisplayActionSheet(null, "Annuler", null, "Ordonnance", "Document");

			if ((action == "Ordonnance") || (action == "Document"))
			{
				await CrossMedia.Current.Initialize();

				if (!CrossMedia.Current.IsCameraAvailable && !CrossMedia.Current.IsTakePhotoSupported)
				{
					await CoreMethods.DisplayAlert("L'appareil photo n'est pas disponible", null, "OK");
					return;
				}

                var Photo_action = await CoreMethods.DisplayActionSheet(null, "Annuler", null, "Appareil photo", "Bibliothèque photo");

                bool permission;

                if (Photo_action == "Appareil photo")
                {
                    permission = await App.AskForCameraPermission();

                    if (permission)
                    {
                        await CrossMedia.Current.Initialize();
                        var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                        {
                            Directory = "Docs",
                            Name = DateTime.UtcNow.Ticks.ToString(),
                            CompressionQuality = 70,
                            SaveToAlbum = false
                        });

                        if (file != null)
                        {
                            _documentPath = file.Path;
                        }
                        else
                            return;
                    }
                    else
                        return;

                }
                else if (Photo_action == "Bibliothèque photo")
                {
                    permission = await App.AskForPhotoPermission();

                    if (permission)
                    {
                        await CrossMedia.Current.Initialize();
                      
                        var pickerOptions = new PickMediaOptions() { CompressionQuality = 30 };
                        var file = await CrossMedia.Current.PickPhotoAsync(pickerOptions);

                        if (file != null)
                        {
                            _documentPath = file.Path;
                        }
                        else
                            return;
                    }
                    else
                        return;
                }
                else
                    return;
                   

                if(permission)
                {                  
                        //if document we change typeDoc to document
                        if (action == "Document") 
                        { typeDoc = "document"; }
                        else
                        {
                            typeDoc = "ordonnance";
                        }

                     
                        var typeNavigation = "fast";
                         
                        await CoreMethods.PushPageModel<PatientListViewModel>(new string[] { "HomeSelectPatient", typeNavigation, typeDoc }, true);
                                      
                }
			}
		});
       

        public async override void ReverseInit(object returnedData)
        {
            base.ReverseInit(returnedData);

            if (returnedData is Patient)
            {
                await AddPrescription(returnedData as Patient);
            }
        }

        private async Task AddPrescription(Patient patient)
		{
			if (typeDoc == "ordonnance")
			{
                var ordannance = new Ordonnance()
                {
                    Id = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.UtcNow,
                    IsSynced = false,
                    UpdatedAt = null,
                    First_Care_At = 0,
					Attachments = new List<string>() { _documentPath },
					Frequencies = new List<Frequency>(),
					Patient = patient,
					PatientId = patient.Id,
					PatientName = patient.Fullname
				};
			
                await new StorageService<Ordonnance>().AddAsync(ordannance);

               
                if (App.IsConnected())
                {
                    //UserDialogs.Instance.ShowLoading("Chargement...");
                    new StorageService<Ordonnance>().PushOrdonnance(ordannance, true);
                    //UserDialogs.Instance.HideLoading();
                }
                 
                ShowMessage();

            }
			else
			{
				var document = new Document()
				{
					Id = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.UtcNow,
                    IsSynced = false,
                    UpdatedAt = null,
                    Reference = DateTime.UtcNow.Ticks,
                    AddDate = DateTime.UtcNow,
					Patient = patient,
					PatientId = patient.Id,
					AttachmentPath = _documentPath,
				};

                await Task.Delay(1000);

                await CoreMethods.PushPageModel<AddDocumentViewModel>(document,true,false);			
            }

		}

        async void ShowMessage()
        {
            await ToastService.Show("Votre ordonnance a bien été enregistrée !");
            MessagingCenter.Send(this,"Showhomemessage");
        }
       
	}
}
