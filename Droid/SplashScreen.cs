using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace libermedical.Droid
{
    [Activity(
        Label = "Liber Medical",
        Theme = "@style/Theme.Splash",
        Icon = "@drawable/icon",
        NoHistory = true,
        MainLauncher = true,
        ScreenOrientation = ScreenOrientation.Portrait,
        ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class StartActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Intent.AddFlags(ActivityFlags.NoAnimation);
            StartActivity(typeof(MainActivity));
            OverridePendingTransition(0, 0);
        }
    }
}
