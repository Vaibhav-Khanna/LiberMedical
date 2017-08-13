
using Android.App;
using Android.Content.PM;
using Android.OS;

namespace libermedical.Droid
{
	[Activity(Label = "liber-medical.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            Vapolia.WheelPickerForms.Droid.WheelPickerRenderer.InitializeForms();
            global::Xamarin.Forms.Forms.Init(this, bundle);


	        Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

			LoadApplication(new App());
        }
    }
}
