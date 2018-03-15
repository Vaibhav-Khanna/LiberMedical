using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;

namespace libermedical.Droid
{
    [Activity(
        Label = "Liber'Médical",
        Theme = "@style/splashscreen",
        Icon = "@drawable/icon",
        NoHistory = true,
        MainLauncher = true,
        ScreenOrientation = ScreenOrientation.Portrait,
        ConfigurationChanges = ConfigChanges.Locale | ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class StartActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
           
            StartActivity(typeof(MainActivity));

        }
    }
}
