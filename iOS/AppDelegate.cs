
using Foundation;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace libermedical.iOS
{
	[Register("AppDelegate")]
    public class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
	        Xamarin.Forms.Forms.Init();

			var application = new App();
			var color = ((Xamarin.Forms.Color)application.Resources["HeaderFooterBackgroundColor"]).ToUIColor();
            Vapolia.WheelPickerForms.Ios.WheelPickerRenderer.InitializeForms();
			UINavigationBar.Appearance.BarTintColor =  color; //bar background
			UINavigationBar.Appearance.TintColor = UIColor.White; //Tint color of button items
			UINavigationBar.Appearance.TitleTextAttributes = new UIStringAttributes {ForegroundColor = UIColor.White}; //Tint color of button items
	        UISwitch.Appearance.OnTintColor = color;
			

			LoadApplication(application);
			return base.FinishedLaunching(app, options);
        }
    }
}
