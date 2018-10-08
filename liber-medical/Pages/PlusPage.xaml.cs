using libermedical.Helpers;
using Xamarin.Forms;
using Akavache;
using System.Reactive.Linq;
using Acr.UserDialogs;

namespace libermedical.Pages
{
	public partial class PlusPage
    {
        public PlusPage() : base(0, 0)
        {
            InitializeComponent();

            ToolbarItems.Add(new ToolbarItem("Déconnexion","", HandleAction));

        }
              
        protected override void OnAppearing()
        {
            base.OnAppearing();

            bt.TranslationY = -30;

        }

        async void HandleAction()
        {
            var response = await DisplayAlert("Déconnexion", "Êtes-vous sûr de vouloir vous déconnecter ? Tous les fichiers non synchronisés seraient perdus.", "Oui", "Annuler");

            if (response)
            {
                Settings.Token = string.Empty;
                Settings.TokenExpiration = 0;
                Settings.IsLoggedIn = false;

                if (await Plugin.Connectivity.CrossConnectivity.Current.IsRemoteReachable("https://www.google.com"))
                {
                    UserDialogs.Instance.ShowLoading("Synchronisation des modifications...");
                    await App.SyncData();
                    UserDialogs.Instance.HideLoading();
                }

                await BlobCache.UserAccount.InvalidateAll();

                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
        }

    }
}
