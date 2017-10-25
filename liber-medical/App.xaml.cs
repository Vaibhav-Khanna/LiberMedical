﻿using System;
using System.Threading.Tasks;
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
        private static IUserManager _userManager;
        private static ILoginManager _loginManager;
        private static IOrdonnanceManager _ordonnanceManager;
        private static IPatientsManager _patientsManager;
        private static IDocumentsManager _documentsManager;
        private static ITeledeclarationsManager _teledeclarationsManager;
        public static LibermedicalTabbedNavigation tabbedNavigation;

        public App()
        {
            InitializeComponent();
            BlobCache.ApplicationName = "LiberMedical";
            FreshIOC.Container.Register<IStorageService<Patient>, StorageService<Patient>>();
            FreshIOC.Container.Register<IStorageService<Ordonnance>, StorageService<Ordonnance>>();
            FreshIOC.Container.Register<IStorageService<Teledeclaration>, StorageService<Teledeclaration>>();
            FreshIOC.Container.Register(UserDialogs.Instance);

            MessagingCenter.Subscribe<MyAccountEditViewModel>(this, "ProfileUpdate", UpdateProfile);

            MessagingCenter.Subscribe<LoginPage>(this, Events.CreateTabbedPage, sender =>
            {
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
            tabbedNavigation.AddTab<HomeViewModel>("", "home_selected.png");
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

        public static long ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date.ToUniversalTime() - origin).TotalSeconds);
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

        //public static IUserManager UserManager(string id, string route)
        //{
        //	if (!string.IsNullOrEmpty(route))
        //		return _userManager = new UserManager(new RestService<User>($"users/{id}/{route}"));
        //	return _userManager = new UserManager(new RestService<User>($"users/{id}"));
        //}

        public static ILoginManager LoginManager => _loginManager ??
                                                    (_loginManager = new LoginManager(new RestService<LoginRequest>("")));

        public static IOrdonnanceManager OrdonnanceManager => _ordonnanceManager ??
                                                              (_ordonnanceManager = new OrdonnanceManager(new RestService<Ordonnance>("ordonnances")));

        public static IPatientsManager PatientsManager => _patientsManager ??
                                                              (_patientsManager = new PatientsManager(new RestService<Patient>("patients")));

        public static ITeledeclarationsManager TeledeclarationsManager => _teledeclarationsManager ??
                                                                            (_teledeclarationsManager = new TeledeclarationsManager(new RestService<Teledeclaration>("teledeclarations")));
        public static IUserManager UserManager => _userManager ??
                                                      (_userManager = new UserManager(new RestService<User>("users")));

        public static IDocumentsManager DocumentsManager => _documentsManager ??
                                                      (_documentsManager = new DocumentsManager(new RestService<Document>("documents")));



    }
}
