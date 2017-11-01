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
			SubscribeToMessages();
			WelcomeText = $"Bonjour {JsonConvert.DeserializeObject<User>(Settings.CurrentUser).Firstname}, que souhaitez vous faire?";
			CheckForAdvisor();
		}
		public ICommand AssistCommand => new Command(async () =>
		{
			var action = await CoreMethods.DisplayActionSheet(null, "Annuler", null, "Appel vocal", "E-mail", "SMS");
			switch (action)
			{
				case "Appel vocal":
					Device.OpenUri(new System.Uri($"tel:{Settings.AdvisorContact}"));
					break;

				case "E-mail":
					Device.OpenUri(new System.Uri($"mailto:{Settings.AdvisorEmail}"));
					break;

				case "SMS":
					Device.OpenUri(new System.Uri($"sms:{Settings.AdvisorContact}"));
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

				var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                { Directory = "Docs", Name = DateTime.Now.Ticks.ToString(), CompressionQuality = 30 });
                //if photo ok
                if (file != null)
				{
					var profilePicture = ImageSource.FromStream(() => file.GetStream());
					var typeNavigation = "normal";

					await CoreMethods.PushPageModel<PatientListViewModel>(new string[] { "HomeSelectPatient", typeNavigation, typeDoc }, true);

					//var page = FreshPageModelResolver.ResolvePageModel<PatientListViewModel>();
					//var basicNavContainer = new FreshNavigationContainer(page, "HomeSelectPatient");
					//await CoreMethods.PushNewNavigationServiceModal(basicNavContainer, new FreshBasePageModel[] { page.GetModel() });
				}
			}
			else if (action == "Bibliothèque photo")
			{
				await CrossMedia.Current.Initialize();

				var pickerOptions = new PickMediaOptions() { CompressionQuality = 30 };

				var file = await CrossMedia.Current.PickPhotoAsync(pickerOptions);
				if (file != null)
				{
					var profilePicture = ImageSource.FromStream(() => file.GetStream());
					var typeNavigation = "normal";
					_documentPath = file.Path;
					await CoreMethods.PushPageModel<PatientListViewModel>(new string[] { "HomeSelectPatient", typeNavigation, typeDoc }, true);

					//var page = FreshPageModelResolver.ResolvePageModel<PatientListViewModel>();
					//var basicNavContainer = new FreshNavigationContainer(page, "HomeSelectPatient");
					//await CoreMethods.PushNewNavigationServiceModal(basicNavContainer, new FreshBasePageModel[] { page.GetModel() });

				}
			}
		});

		//This concern  ordonances or documents in fast process
		public ICommand FastCommand => new Command(async () =>
		{
			var action = await CoreMethods.DisplayActionSheet(null, "Annuler", null, "Ordonnance", "Document");

			if ((action == "Ordonnance") || (action == "Document"))
			{
				await CrossMedia.Current.Initialize();

				if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
				{
					await CoreMethods.DisplayAlert("L'appareil photo n'est pas disponible", null, "OK");
					return;
				}

				var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                { Directory = "Docs", Name = DateTime.UtcNow.Ticks.ToString(), CompressionQuality =30  });
				if (file != null)
				{
					//if document we change typeDoc to document
					if (action == "Document") { typeDoc = "document"; }

					var profilePicture = ImageSource.FromStream(() => file.GetStream());
					var typeNavigation = "fast";
					_documentPath = file.Path;
					await CoreMethods.PushPageModel<PatientListViewModel>(new string[] { "HomeSelectPatient", typeNavigation, typeDoc }, true);

					//var page = FreshPageModelResolver.ResolvePageModel<PatientListViewModel>();
					//var basicNavContainer = new FreshNavigationContainer(page, "HomeSelectPatient");
					//await CoreMethods.PushNewNavigationServiceModal(basicNavContainer, new FreshBasePageModel[] { page.GetModel() });
				}
			}
		});

		private void SubscribeToMessages()
		{
			MessagingCenter.Subscribe<PatientListViewModel, Patient>(this, Events.HomePageSetPatientForOrdonnance,
				async (sender, patient) =>
				{
					if (patient != null)
					{
						await AddPrescription(patient);
					}
				});
		}

		private async Task AddPrescription(Patient patient)
		{
			if (typeDoc == "ordonnance")
			{
				var ordannance = new Ordonnance()
				{
					Id = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.UtcNow,
                    First_Care_At = App.ConvertToUnixTimestamp(DateTime.UtcNow),
					Attachments = new List<string>() { _documentPath },
					Frequencies = new List<Frequency>(),
					Patient = patient,
					PatientId = patient.Id,
					PatientName = patient.Fullname
				};
				await new StorageService<Ordonnance>().AddAsync(ordannance);
                if (App.IsConnected())
                {
                    UserDialogs.Instance.ShowLoading("Processing...");
                    await new StorageService<Ordonnance>().PushOrdonnance(ordannance, true);
                    UserDialogs.Instance.HideLoading();
                }
            }
			else
			{
				var document = new Document()
				{
					Id = Guid.NewGuid().ToString(),
                    CreatedAt = DateTime.UtcNow,
                    Reference = DateTime.UtcNow.Ticks,
                    AddDate = DateTime.UtcNow,
					Patient = patient,
					PatientId = patient.Id,
					AttachmentPath = _documentPath,
				};
				await new StorageService<Document>().AddAsync(document);
                if (App.IsConnected())
                {
                    UserDialogs.Instance.ShowLoading("Processing...");
                    await new StorageService<Document>().PushDocument(document, true);
                    UserDialogs.Instance.HideLoading();
                }
            }

		}
	}
}
