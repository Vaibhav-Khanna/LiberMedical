using System.Threading.Tasks;
using libermedical.Models;
using libermedical.Pages;
using libermedical.Services;
using libermedical.Utility;
using libermedical.ViewModels.Base;
using Plugin.Media;
using Xamarin.Forms;

namespace libermedical.ViewModels
{
	public class OrdonnancesListViewModel : ListViewModelBase<Ordonnance>
	{
		public OrdonnancesListViewModel(IStorageService<Ordonnance> storageService) : base(storageService)
		{
		}

		protected override async Task TapCommandFunc(Cell cell)
		{
			var ctx = cell.BindingContext;
			await CoreMethods.PushPageModelWithNewNavigation<DetailsPatientListViewModel>(ctx);
		}

		public System.Windows.Input.ICommand OpenCameraCommand => new Command(async () =>
		{
			var action = await CoreMethods.DisplayActionSheet(null, "Annuler", null, "Ordonnance rapide", "Ordonnance classique");
			var typeDoc = "ordonnance";
			string typeNavigation = "";

			if ((action != null) && (action != "Annuler"))
			{
				//init du plugin photo 
				await CrossMedia.Current.Initialize();

				if (UtilityClass.CameraAvailable())
				{

					var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions()
					{
						AllowCropping = true
					});

					if (file != null)
					{

						var profilePicture = ImageSource.FromStream(() => file.GetStream());

						if (action == "Ordonnance rapide") { typeNavigation = "fast"; }
						if (action == "Ordonnance classique") { typeNavigation = "normal"; }

                        //await CoreMethods.PushPageModel<PatientListPage>(new NavigationPage(new PatientListPage(typeNavigation, typeDoc)),true);
                       // await CoreMethods.PushPageModelWithNewNavigation<PatientListPage>(null, true);

					}
				}
				else
				{
					await CoreMethods.DisplayAlert("No Camera", ":( No camera available.", "OK");
				}
			}
			else
			{
				return;
			}
		});
	}
}
