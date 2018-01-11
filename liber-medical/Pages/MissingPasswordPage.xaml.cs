using System;
using Acr.UserDialogs;
using libermedical.CustomControls;
using libermedical.PopUp;
using libermedical.Request;
using Xamarin.Forms;

namespace libermedical.Pages
{
    public partial class MissingPasswordPage : BasePage
    {
        private bool _validEmail = false;
        private string _email;
        private Color green = Color.FromHex("#91c602");
        private Color grey = Color.FromHex("#d1d1d1");

        public MissingPasswordPage(string email) : base(0, 0)
        {
            InitializeComponent();

            MyEntry.Text = email;
        }

        private void EmailEntry_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue) && LoginPage.IsEmail(e.NewTextValue))
            {
                _validEmail = true;
                _email = e.NewTextValue;
                MyButton.BackgroundColor = green;
                MyButton.IsEnabled = true;
            }
            else
            {
                _validEmail = false;
                MyButton.BackgroundColor = grey;
                MyButton.IsEnabled = false;
            }
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            if (_validEmail)
            {
                MyButton.IsEnabled = false;

                UserDialogs.Instance.ShowLoading("");

                await App.LoginManager.RequestNewPassword(new ForgotPasswordRequest
                {
                    email = _email
                });

                UserDialogs.Instance.HideLoading();

                await ToastService.Show("Un nouveau mot de passe vient de vous etre transmis par email");

                MyButton.IsEnabled = true;

                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Merci d'entrer un email", null, "OK");
            }
        }
    }
}
