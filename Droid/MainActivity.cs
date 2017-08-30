
using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;

namespace libermedical.Droid
{
	[Activity(Label = "Liber Medical", Icon = "@drawable/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            Vapolia.WheelPickerForms.Droid.WheelPickerRenderer.InitializeForms();
            Xamarin.Forms.Forms.Init(this, bundle);


	        UserDialogs.Init(this);
			LoadApplication(new App());
        }
    }
}
