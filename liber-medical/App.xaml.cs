using Akavache;
using FreshMvvm;
using libermedical.Models;
using libermedical.Services;
using libermedical.ViewModels;
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
			//MainPage = new NavigationPage(new WheelPicker());
			//MainPage = new NavigationPage(new OrdonnanceCotationPage());

			//			            MainPage = new MainTabPage();


			var tabbedNavigation = new FreshTabbedNavigationContainer();
			tabbedNavigation.Style = Resources["TabbedPage"] as Style;
//
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
			//			MainPage = new NavigationPage(new LoginPage());
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

        
	}
}
