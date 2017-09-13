﻿using System.Threading.Tasks;
using Acr.UserDialogs;
using Akavache;
using FreshMvvm;
using libermedical.Helpers;
using libermedical.Managers;
using libermedical.Models;
using libermedical.Pages;
using libermedical.Request;
using libermedical.Services;
using libermedical.ViewModels;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace libermedical
{
    public partial class App : Application
    {
        private static ILoginManager _loginManager;
        private static IOrdonnanceManager _ordonnanceManager;

        public static LibermedicalTabbedNavigation tabbedNavigation;

        public App()
        {
            InitializeComponent();
            BlobCache.ApplicationName = "LiberMedical";
            FreshIOC.Container.Register<IStorageService<Patient>, StorageService<Patient>>();
            FreshIOC.Container.Register<IStorageService<Ordonnance>, StorageService<Ordonnance>>();
            FreshIOC.Container.Register(UserDialogs.Instance);

            MessagingCenter.Subscribe<MyAccountEditViewModel>(this, "ProfileUpdate", UpdateProfile);

            MessagingCenter.Subscribe<LoginPage>(this, Events.CreateTabbedPage, sender => {
                CreateTabbedPage();
            });
            
            if (Settings.IsLoggedIn)
            {
                CreateTabbedPage();
                MainPage = tabbedNavigation;
            }
            else
            {
                MainPage = new NavigationPage(new LoginPage()); // { BarTextColor = Color.White };
            }
        }

        private void CreateTabbedPage()
        {
            tabbedNavigation = new LibermedicalTabbedNavigation { Style = Resources["TabbedPage"] as Style };
            tabbedNavigation.AddTab<HomeViewModel>("", "home_green.png");
            tabbedNavigation.AddTab<PatientListViewModel>("", "patients.png");
            tabbedNavigation.AddTab<OrdonnancesListViewModel>("", "ordonnances.png");
            tabbedNavigation.AddTab<TeledeclarationsListViewModel>("", "teledeclaration.png");
            tabbedNavigation.AddTab<PlusViewModel>("", "plus_tabbed.png");

            foreach (var tabbedNavigationTabbedPage in tabbedNavigation.TabbedPages)
            {
                tabbedNavigationTabbedPage.Style = Resources["NavigationPage"] as Style;
            }
        }

        public static bool IsConnected()
        {
            using (var connectivity = CrossConnectivity.Current)
            {
                return connectivity.IsConnected;
            }
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

        public static ILoginManager LoginManager => _loginManager ??
                                                    (_loginManager = new LoginManager(new RestService<LoginRequest>("")));

        public static IOrdonnanceManager OrdonnanceManager => _ordonnanceManager ??
                                                              (_ordonnanceManager = new OrdonnanceManager(new RestService<Ordonnance>("ordonnances")));
    }
}
