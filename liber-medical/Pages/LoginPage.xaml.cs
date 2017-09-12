using System.Text.RegularExpressions;
using libermedical.CustomControls;
using libermedical.Helpers;
using libermedical.Models;
using libermedical.Request;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace libermedical.Pages
{
    public partial class LoginPage : BasePage
    {
        private string _email;
        private string _pass;
        private Color green = Color.FromHex("#91c602");
        private Color grey = Color.FromHex("#d1d1d1");
        private bool _emailValid = false;


        public LoginPage() : base(-1, -1, false)
        {
            InitializeComponent();
        }


        async void Handle_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new MissingPasswordPage(_email));
        }

        void Login_TextChanged(object sender, TextChangedEventArgs e)
        {
            //email entered by user
            _email = ((Entry) sender).Text;

            if (IsEmail(_email))
            {
                //Flag to true
                _emailValid = true;

                if (!string.IsNullOrEmpty(_pass) && (_pass.Length >= 6))
                {
                    //button activation if pass >= 6
                    button.IsEnabled = true;
                    button.BackgroundColor = green;
                }
            }
            //if email format is Nok 
            else
            {
                //flag to false
                _emailValid = false;
                //desactivation of button
                button.BackgroundColor = grey;
                button.IsEnabled = false;
            }
        }

        void Login_Completed(object sender, System.EventArgs e)
        {
            //if completed focus to the password
            if (_emailValid)
            {
                password.Focus();
            }
        }

        void Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            //password entered by user
            _pass = ((Entry) sender).Text;
            //if one element not ok , desactivation of button
            if ((_pass.Length < 6) || (!_emailValid))
            {
                button.BackgroundColor = grey;
                button.IsEnabled = false;
            }
            //if all element are ok , activation of button
            if (_emailValid && _pass.Length >= 6)
            {
                button.IsEnabled = true;
                button.BackgroundColor = green;
            }
        }

        private async void Handle_Clicked(object sender, System.EventArgs e)
        {
            //password and login verification from API
            if (IsBusy) return;

            if (string.IsNullOrEmpty(_email) || string.IsNullOrEmpty(_pass))
            {
                await DisplayAlert("Merci d'entrer un email et/ou un mot de passe", null, "OK");
                return;
            }

            IsBusy = true;

            ShowLoading();

            var login = new LoginRequest
            {
                email = _email,
                password = _pass
            };

            var token = await App.LoginManager.Login(login);
            if (token != null)
            {
                Settings.IsLoggedIn = true;
                Settings.Token = token.token;
                Settings.TokenExpiration = token.tokenExpiration;

                var user = new User
                {
                    Email = _email
                };
                Settings.CurrentUser = JsonConvert.SerializeObject(user);

                await Navigation.PushModalAsync(App.tabbedNavigation);
            }
            else
            {
                HideAllPopup();
                await DisplayAlert("Mauvais email et/ou mot de passe. Merci de réessayer", null, null, "OK");
                IsBusy = false;
            }

            HideAllPopup();

            IsBusy = false;
        }

        public static bool IsEmail(string email)
        {
            //Regex email pattern
            var emailPattern =
                "^(?(\")(\".+?(?<!\\\\)\"@)|(([0-9a-z]((\\.(?!\\.))|[-!#\\$%&'\\*\\+/=\\?\\^`\\{\\}\\|~\\w])*)(?<=[0-9a-z])@))(?(\\[)(\\[(\\d{1,3}\\.){3}\\d{1,3}\\])|(([0-9a-z][-\\w]*[0-9a-z]*\\.)+[a-z0-9][\\-a-z0-9]{0,22}[a-z0-9]))$";

            return Regex.IsMatch(email, emailPattern);
        }
    }
}
