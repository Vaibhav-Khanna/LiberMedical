using libermedical.Helpers;
using Xamarin.Forms;

namespace libermedical.Pages
{
	public partial class PlusPage
    {
        public PlusPage() : base(0, 0)
        {
            InitializeComponent();

            ToolbarItems.Add(new ToolbarItem("Déconnexion","", HandleAction));

        }

        void Handle_Tapped(object sender, System.EventArgs e)
        {
            if (listS.IsVisible)
            {
                listS.IsVisible = false;
                image.Rotation = 90;
            }              
            else
            {
                image.Rotation = -90;
                listS.IsVisible = true;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            bt.TranslationY = -30;

        }

        async void HandleAction()
        {
            var response = await DisplayAlert("Déconnexion", "Êtes-vous sûr de vouloir vous déconnecter ?", "Oui", "Annuler");

            if (response)
            {
                Settings.Token = string.Empty;
                Settings.TokenExpiration = 0;
                Settings.IsLoggedIn = false;
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
        }
    }
}
