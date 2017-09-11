using Xamarin.Forms;
using Foundation;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace libermedical.iOS
{
	[Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Vapolia.WheelPickerForms.Ios.WheelPickerRenderer.InitializeForms();
            Forms.Init();
            var application = new App();
			var color = ((Color)application.Resources["HeaderFooterBackgroundColor"]).ToUIColor();
            //Vapolia.WheelPickerForms.Ios.WheelPickerRenderer.InitializeForms();
			UINavigationBar.Appearance.BarTintColor =  color; //bar background
			UINavigationBar.Appearance.TintColor = UIColor.White; //Tint color of button items
			UINavigationBar.Appearance.TitleTextAttributes = new UIStringAttributes {ForegroundColor = UIColor.White}; //Tint color of button items
	        UISwitch.Appearance.OnTintColor = color;
	        UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, true);
            
            LoadApplication(application);
			return base.FinishedLaunching(app, options);
        }
    }
}
