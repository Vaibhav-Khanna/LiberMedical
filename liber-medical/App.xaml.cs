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
			FreshIOC.Container.Register<IAzureService<Patient>, AzureService<Patient>>();
			//MainPage = new NavigationPage(new WheelPicker());
			//MainPage = new NavigationPage(new OrdonnanceCotationPage());

			//			            MainPage = new MainTabPage();


			var tabbedNavigation = new FreshTabbedNavigationContainer();
			tabbedNavigation.Style = Resources["TabbedPage"] as Style;

			tabbedNavigation.AddTab<HomeViewModel>("", "home_green.png", null);
			tabbedNavigation.AddTab<PatientListViewModel>("", "patients.png", null);
			tabbedNavigation.AddTab<DetailsPatientListViewModel>("", "ordonnances.png", null);
			tabbedNavigation.AddTab<PatientListViewModel>("", "teledeclaration.png", null);
			tabbedNavigation.AddTab<PatientListViewModel>("", "plus_tabbed.png", null);

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
