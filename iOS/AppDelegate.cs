using Xamarin.Forms;
using Foundation;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using Syncfusion.SfPdfViewer.XForms.iOS;
using Syncfusion.SfPicker.XForms.iOS;
using FFImageLoading.Forms.Touch;
using Social;



namespace libermedical.iOS
{
	[Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Vapolia.WheelPickerForms.Ios.WheelPickerRenderer.InitializeForms();

			Rg.Plugins.Popup.IOS.Popup.Init();

            Forms.Init();
           
           

            var application = new App();
			var color = ((Color)application.Resources["HeaderFooterBackgroundColor"]).ToUIColor();
            //Vapolia.WheelPickerForms.Ios.WheelPickerRenderer.InitializeForms();
		
            UINavigationBar.Appearance.BarTintColor =  color; //bar background
			UINavigationBar.Appearance.TintColor = UIColor.White; //Tint color of button items
			UINavigationBar.Appearance.TitleTextAttributes = new UIStringAttributes {ForegroundColor = UIColor.White}; //Tint color of button items
	        UISwitch.Appearance.OnTintColor = color;
	        UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, true);
           
            var tint = Xamarin.Forms.Color.FromRgb(145,198,2).ToUIColor();                   
            UIAlertView.Appearance.TintColor = tint;
            UIView.AppearanceWhenContainedIn(typeof(UIAlertController)).TintColor = tint;
            UIView.AppearanceWhenContainedIn(typeof(UIActivityViewController)).TintColor = tint;
            UIView.AppearanceWhenContainedIn(typeof(SLComposeViewController)).TintColor = tint;
                       
            new SfPdfDocumentViewRenderer();
            SfPickerRenderer.Init();
            CachedImageRenderer.Init();

            LoadApplication(application);
			return base.FinishedLaunching(app, options);
        }
    }

   


}
