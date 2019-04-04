using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace libermedical.PopUp
{
    public partial class ToastPage : PopupPage
    {
        public ToastPage(string Text)
        {
            InitializeComponent();
            ToastLabel.Text = Text;
        }

        protected override void OnAppearingAnimationEnd()
        {
            Device.StartTimer(new TimeSpan(0, 0, 2), () =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (PopupNavigation.Instance.PopupStack.Count != 0)
                        await PopupNavigation.Instance.PopAllAsync();
                });

                return false;
            });

            base.OnAppearingAnimationEnd();
        }
    }
}
