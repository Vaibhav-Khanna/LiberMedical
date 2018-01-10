using System;
using libermedical.ViewModels.Base;
using Plugin.Share;
using Xamarin.Forms;

namespace libermedical.ViewModels
{
    public class PlusOtherPageModel : ViewModelBase
    {
        public PlusOtherPageModel()
        {
        }

        private Plugin.Share.Abstractions.BrowserOptions options = new Plugin.Share.Abstractions.BrowserOptions() { ChromeShowTitle = true, SafariBarTintColor = new Plugin.Share.Abstractions.ShareColor(145, 198, 2), ChromeToolbarColor = new Plugin.Share.Abstractions.ShareColor(145, 198, 2), SafariControlTintColor = new Plugin.Share.Abstractions.ShareColor(255, 255, 255), UseSafariWebViewController = true };


        public Command CGVCommand => new Command(async (obj) =>
        {
            await CrossShare.Current.OpenBrowser("https://libermedical.fr/_tos.html", options);
        });

        public Command CGUCommand => new Command(async (obj) =>
        {
            await CrossShare.Current.OpenBrowser("https://libermedical.fr/cgu.html", options);
        });

        public Command FAQCommand => new Command(async (obj) =>
        {
            await CrossShare.Current.OpenBrowser("https://libermedical.fr/faq-mobile.php", options);
        });

        public Command BackCommand => new Command(async () =>
        {
            await CoreMethods.PopPageModel(true,false);
        });

    }
}
