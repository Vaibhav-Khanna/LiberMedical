using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using libermedical.CustomControls;
using Xamarin.Forms;

namespace libermedical.Views
{
    public partial class LoginPage : BasePage
    {
        private string email;
        private string pass;
        private Color green = Color.FromHex("#91c602");
        private Color grey = Color.FromHex("#d1d1d1");
        private bool emailValid = false;



        public LoginPage() : base(-1, -1, false)
        {
            InitializeComponent();

        }


        async void Handle_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new MissingPasswordPage());
        }

        void Login_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            //Regex email pattern
            var emailPattern = "^(?(\")(\".+?(?<!\\\\)\"@)|(([0-9a-z]((\\.(?!\\.))|[-!#\\$%&'\\*\\+/=\\?\\^`\\{\\}\\|~\\w])*)(?<=[0-9a-z])@))(?(\\[)(\\[(\\d{1,3}\\.){3}\\d{1,3}\\])|(([0-9a-z][-\\w]*[0-9a-z]*\\.)+[a-z0-9][\\-a-z0-9]{0,22}[a-z0-9]))$";
            //email entered by user
            email = ((Entry)sender).Text;

            //if email format is ok 
            if ((email != null) && (Regex.IsMatch(email, emailPattern)))
            {
                //Flag to true
                emailValid = true;

                if ((pass != null) && (pass.Length >= 8))
                {
                    //button activation if pass >= 8
                    button.IsEnabled = true;
                    button.BackgroundColor = green;
                }

            }
            //if email format is Nok 
            else
            {
                //flag to false
                emailValid = false;
                //desactivation of button
                button.BackgroundColor = grey;
                button.IsEnabled = false;
            }


        }

        void Login_Completed(object sender, System.EventArgs e)
        {
            //if completed focus to the password
            if (emailValid)
            { password.Focus(); }

        }

        void Password_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            //password entered by user
            pass = ((Entry)sender).Text;
            //if one element not ok , desactivation of button
            if ((pass.Length < 8) || (!emailValid))
            {
                button.BackgroundColor = grey;
                button.IsEnabled = false;
            }
            //if all element are ok , activation of button
            if ((emailValid) && (pass.Length >= 8))
            {
                button.IsEnabled = true;
                button.BackgroundColor = green;

            }
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            //password and login verification from API

            // if ok

        }
    }
}
