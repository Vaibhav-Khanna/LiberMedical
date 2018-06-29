using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Akavache;
using Com.OneSignal;
using Com.OneSignal.Abstractions;
using FreshMvvm;
using libermedical.Enums;
using libermedical.Helpers;
using libermedical.Managers;
using libermedical.Models;
using libermedical.Pages;
using libermedical.PopUp;
using libermedical.Request;
using libermedical.Services;
using libermedical.ViewModels;
using Plugin.Connectivity;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
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
        private static IInvoicesManager _invoicesManager;
        private static IInvoicesManager _billsManager;

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

            if (!string.IsNullOrEmpty(Settings.Token))
            {
                CreateTabbedPage();
                MainPage = tabbedNavigation;
            }
            else
            {
                MainPage = new NavigationPage(new LoginPage()); // { BarTextColor = Color.White };
            }

            OneSignal.Current.StartInit("045fee61-44e6-45d3-8366-a8cda02647a2").HandleNotificationOpened(HandleNotificationOpened).EndInit();

           
            if (IsConnected())
                SyncData();
            
            Plugin.Connectivity.CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
            {
                if (args.IsConnected)
                    SyncData();
            };

            Plugin.Connectivity.CrossConnectivity.Current.ConnectivityTypeChanged += (sender, e) => 
            {
                if(e.IsConnected)
                    SyncData();
            };             
        }

      
        // Handle Notiications recieved via oneSignal and take appropriate actions
        private static async void HandleNotificationOpened(OSNotificationOpenedResult result)
        {
            OSNotificationPayload payload = result.notification.payload;
            Dictionary<string,object> additionalData = payload.additionalData;
            string message = payload.body;
            string actionID = result.action.actionID;

            var extraMessage = "Notification opened with text: " + message;

            if (additionalData != null)
            {
                string id = ""; //  the id of the object to open
                string kind = ""; // the kind of object the id belongs to and take relevant action to open page based on this

                if (additionalData.ContainsKey("id"))
                {
                    id = (additionalData["id"].ToString());
                }
                if (additionalData.ContainsKey("kind"))
                {
                    kind = (additionalData["kind"].ToString());
                }

                switch (kind)
                {
                    case "Document":
                        {
                            Document obj;

                            if (await CrossConnectivity.Current.IsRemoteReachable("https://www.google.com"))
                                obj = await DocumentsManager.GetAsync(id);
                            else
                                obj = await new StorageService<Document>().GetItemAsync(id);

                            if (obj != null)
                            {
                                await (tabbedNavigation._tabs[0].BindingContext as FreshBasePageModel).CoreMethods.PushPageModel<AddDocumentViewModel>(obj, true, Device.RuntimePlatform == Device.iOS);
                            }

                            break;
                        }
                    case "Teledeclaration":
                        {
                            Teledeclaration obj;

                            if (await CrossConnectivity.Current.IsRemoteReachable("https://www.google.com"))
                                obj = await TeledeclarationsManager.GetAsync(id);
                            else
                                obj = await new StorageService<Teledeclaration>().GetItemAsync(id);

                            if (obj != null)
                            {
                                await (tabbedNavigation._tabs[0].BindingContext as FreshBasePageModel).CoreMethods.PushPageModel<TeledeclarationSecureActionViewModel>(obj, true, true);
                            }

                            break;
                        }
                    case "Ordonnance":
                        {
                            Ordonnance obj;

                            if (await CrossConnectivity.Current.IsRemoteReachable("https://www.google.com"))
                                obj = await OrdonnanceManager.GetAsync(id);
                            else
                                obj = await new StorageService<Ordonnance>().GetItemAsync(id);

                            if (obj != null)
                            {
                                await (tabbedNavigation._tabs[0].BindingContext as FreshBasePageModel).CoreMethods.PushPageModel<OrdonnanceCreateEditViewModel>(obj, true);
                            }

                            break;    
                        }
                    case "InvoiceToSecure":
                        {
                            if (await CrossConnectivity.Current.IsRemoteReachable("https://www.google.com"))
                            {
                                var request = new GetListRequest(20, 1, sortField: "createdAt", sortDirection: SortDirectionEnum.Desc);
                                var invoices = await InvoicesManager.GetListAsync(request);

                                if (invoices != null && invoices.rows != null && invoices.rows.Count > 0)
                                {
                                    var invoice = invoices.rows.First();
                                
                                    if (invoice.FilePath.Contains(".pdf"))
                                        await (tabbedNavigation._tabs[0].BindingContext as FreshBasePageModel).CoreMethods.PushPageModel<SecuriseBillsViewModel>(invoice, true);
                                    else
                                        await (tabbedNavigation._tabs[0].BindingContext as FreshBasePageModel).CoreMethods.PushPageModel<OrdonnanceViewViewModel>(invoice, true);
                                }
                            }
                            break;
                        }
                    default:
                        break;
                }

            }
        }       


        static bool IsOpening = false;

        public static void MoveToLogin()
        {
            if (IsOpening)
                return;

            IsOpening = true;

            Device.BeginInvokeOnMainThread( () => 
            {
                App.Current.MainPage = new NavigationPage(new LoginPage());
            });

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
            return Plugin.Connectivity.CrossConnectivity.Current.IsConnected;
        }

        static bool isSyncing = false;

        public async static Task SyncData()
        {
            if (isSyncing || !IsConnected())
                return;
            
            isSyncing = true;

            var synchelper = new StorageService<BaseDTO>();
            await synchelper.SyncTables();

            isSyncing = false;
        }


        public static long ConvertToUnixTimestamp(DateTime date)
        {
            //DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return  (long) (date.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;;
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


        public static async Task<bool> AskForCameraPermission()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
               
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera))
                    {
                        await tabbedNavigation.DisplayAlert("Accès à la caméra", "L'accès est requis pour les ordonnances et les documents", "Oui");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);
                    //Best practice to always check that the key exists

                    if (results.ContainsKey(Permission.Camera))
                        status = results[Permission.Camera];
                }

                if (status == PermissionStatus.Granted)
                {
                    return true;
                }
                else
                {
                    await tabbedNavigation.DisplayAlert("Accès refusé", "Veuillez activer l'autorisation dans les paramètres pour utiliser cette fonctionnalité", "Oui");
                    return false;
                }
               
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<bool> AskForPhotoPermission()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Photos);

                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Photos))
                    {
                        await tabbedNavigation.DisplayAlert("Accès à la caméra", "L'accès est requis pour les ordonnances et les documents", "Oui");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Photos);
                    //Best practice to always check that the key exists

                    if (results.ContainsKey(Permission.Photos))
                        status = results[Permission.Photos];
                }

                if (status == PermissionStatus.Granted)
                {
                    return true;
                }
                else
                {
                    await tabbedNavigation.DisplayAlert("Accès refusé", "Veuillez activer l'autorisation dans les paramètres pour utiliser cette fonctionnalité", "Oui");
                    return false;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }


        private Task updateProfile;

        public void UpdateProfile(MyAccountEditViewModel myAccountEditViewModel)
        {
             UpdateProfileAsync();
        }

        private void UpdateProfileAsync()
        {
            //TODO: Send new profile from Settings to server.
            //await Task.Delay(400);
            //await ToastService.Show("Un nouveau mot de passe vient de vous etre transmis par email");
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

        public static IInvoicesManager InvoicesManager => _invoicesManager ??
                                                      (_invoicesManager = new InvoicesManager(new RestService<Invoice>("invoicesToSecure")));

        public static IInvoicesManager BillsManager => _billsManager ??
        ( _billsManager = new InvoicesManager(new RestService<Invoice>("invoices")));


    }
}
