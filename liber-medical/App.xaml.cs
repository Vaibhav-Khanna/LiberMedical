using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Akavache;
using FreshMvvm;
using libermedical.Helpers;
using libermedical.Models;
using libermedical.Services;
using libermedical.ViewModels;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace libermedical
{
	public partial class App : Application
	{        
		public App()
		{
			InitializeComponent();
            BlobCache.ApplicationName = "LiberMedical";
            FreshIOC.Container.Register<IStorageService<Patient>, StorageService<Patient>>();
			FreshIOC.Container.Register<IStorageService<Ordonnance>, StorageService<Ordonnance>>();
			FreshIOC.Container.Register(UserDialogs.Instance);

			MessagingCenter.Subscribe<MyAccountEditViewModel>(this, "ProfileUpdate", UpdateProfile);
			if (string.IsNullOrEmpty(Settings.CurrentUser))
			{
				var s = new Profile {CreatedAt = DateTimeOffset.Now, LastName = "Turanga", FirstName = "Leela", EmailAddress = "leela@planetexpress.com", PhoneNumber = "+1 (217) 314-15-92"};
				Settings.CurrentUser = JsonConvert.SerializeObject(s);
			}

			var tabbedNavigation = new LibermedicalTabbedNavigation { Style = Resources["TabbedPage"] as Style };
			tabbedNavigation.AddTab<HomeViewModel>("", "home_green.png");
			tabbedNavigation.AddTab<PatientListViewModel>("", "patients.png");
			tabbedNavigation.AddTab<OrdonnancesListViewModel>("", "ordonnances.png");
			tabbedNavigation.AddTab<TeledeclarationsListViewModel>("", "teledeclaration.png");
			tabbedNavigation.AddTab<PlusViewModel>("", "plus_tabbed.png");

			foreach (var tabbedNavigationTabbedPage in tabbedNavigation.TabbedPages)
			{
				tabbedNavigationTabbedPage.Style = Resources["NavigationPage"] as Style;
			}
			MainPage = tabbedNavigation;
//			MainPage = new NavigationPage(new LoginPage()){ BarTextColor = Color.White};
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}

		private Task updateProfile;

		public void UpdateProfile(MyAccountEditViewModel myAccountEditViewModel)
		{
			updateProfile = UpdateProfileAsync();
		}

		private Task UpdateProfileAsync()
		{
			//TODO: Send new profile from Settings to server.
			return null;
		}
	}
}
