using libermedical.CustomControls;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace libermedical.Pages
{
	public partial class AddDocumentPage : BasePage
    {
        public AddDocumentPage() : base(-1, 0, false)
        {
            InitializeComponent();
        }
        async void Cancel_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        async void Save_Tapped(object sender, System.EventArgs e)
        {

            await Navigation.PopModalAsync();
        }
        async void Document_Tapped(object sender, System.EventArgs e)
        {
            await CrossMedia.Current.Initialize();


            var pickerOptions = new PickMediaOptions();

            var file = await CrossMedia.Current.PickPhotoAsync(pickerOptions);
            if (file != null)
            {
                var profilePicture = ImageSource.FromStream(() => file.GetStream());
                var typeNavigation = "normal";

            }
        }
    }
}
