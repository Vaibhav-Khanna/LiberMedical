using System.Threading.Tasks;
using FreshMvvm;
using Xamarin.Forms;

namespace libermedical.Services
{
	public class CustomNavigationContainer: FreshNavigationContainer
	{
		public CustomNavigationContainer(Page page, string navigationPageName) : base(page, navigationPageName)
		{
		}

		protected override Page CreateContainerPage(Page page)
		{
			return new NavigationPage(page) {BarTextColor = Color.White};
		}

		public override Task PushPage(Page page, FreshBasePageModel model, bool modal = false, bool animate = true)
		{
			if (modal)
				return Navigation.PushModalAsync(CreateContainerPage(page), animate);
			return Navigation.PushAsync(page, animate);
		}
	}
}
