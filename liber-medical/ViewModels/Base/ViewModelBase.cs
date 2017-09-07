using System.Threading.Tasks;
using System.Windows.Input;
using FreshMvvm;
using libermedical.Services;
using Xamarin.Forms;

namespace libermedical.ViewModels.Base
{
	public abstract class ViewModelBase : FreshBasePageModel
	{

		public ICommand  NavBackCommand => new Command(async () => 
			await CoreMethods.PopPageModel(true));

		public async Task<string> PushPageModelWithNewNavigation<T>(object data, bool animate = true) where T : FreshBasePageModel
		{
			var page = FreshPageModelResolver.ResolvePageModel<T>(data);
			var navigationName = "CustomNavigation";
			FreshIOC.Container.Resolve<IFreshNavigationService>(this.CurrentNavigationServiceName);
			var navigationContainer = new CustomNavigationContainer(page, navigationName);
			await CoreMethods.PushNewNavigationServiceModal(navigationContainer, page.GetModel(), animate);
			return navigationName;
		}
	}
}
