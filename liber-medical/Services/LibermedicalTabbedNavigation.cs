using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreshMvvm;
using Xamarin.Forms;

namespace libermedical.Services
{
	public class LibermedicalTabbedNavigation :TabbedPage, IFreshNavigationService
	{
		private readonly List<Page> _tabs = new List<Page>();

		public LibermedicalTabbedNavigation() : this("SomeDefaultContainerName")
		{
		}

		public LibermedicalTabbedNavigation(string navigationServiceName)
		{
			NavigationServiceName = navigationServiceName;
			RegisterNavigation();
		}

		public IEnumerable<Page> TabbedPages => _tabs;

		public string NavigationServiceName { get; }

		protected void RegisterNavigation()
		{
			FreshIOC.Container.Register<IFreshNavigationService>(this, NavigationServiceName);
		}

		public virtual Page AddTab<T>(string title, string icon, object data = null) where T : FreshBasePageModel
		{
			var page = FreshPageModelResolver.ResolvePageModel<T>(data);
			page.GetModel().CurrentNavigationServiceName = NavigationServiceName;
			_tabs.Add(page);
			var navigationContainer = CreateContainerPageSafe(page);
			navigationContainer.Title = title;
			if (!string.IsNullOrWhiteSpace(icon))
				navigationContainer.Icon = icon;
			Children.Add(navigationContainer);
			return navigationContainer;
		}

		internal Page CreateContainerPageSafe(Page page)
		{
			if (!(page is NavigationPage) && !(page is MasterDetailPage) && !(page is TabbedPage))
				return CreateContainerPage(page);

			if(page is NavigationPage) ((NavigationPage)page).BarTextColor = Color.White;
			return page;
		}

		protected virtual Page CreateContainerPage(Page page)
		{
			return new NavigationPage(page) {BarTextColor = Color.White};
		}

		public Task PushPage(Page page, FreshBasePageModel model, bool modal = false, bool animate = true)
		{
			if (modal)
				return CurrentPage.Navigation.PushModalAsync(CreateContainerPageSafe(page));
			return CurrentPage.Navigation.PushAsync(page);
		}

		public Task PopPage(bool modal = false, bool animate = true)
		{
			if (modal)
				return CurrentPage.Navigation.PopModalAsync(animate);
			return CurrentPage.Navigation.PopAsync(animate);
		}

		public Task PopToRoot(bool animate = true)
		{
			return CurrentPage.Navigation.PopToRootAsync(animate);
		}

		public void NotifyChildrenPageWasPopped()
		{
			foreach (var page in Children)
				if (page is NavigationPage)
					((NavigationPage) page).NotifyAllChildrenPopped();
		}

		public Task<FreshBasePageModel> SwitchSelectedRootPageModel<T>() where T : FreshBasePageModel
		{
			var page = _tabs.FindIndex(o => o.GetModel().GetType().FullName == typeof(T).FullName);

			if (page > -1)
			{
				CurrentPage = Children[page];
				var topOfStack = CurrentPage.Navigation.NavigationStack.LastOrDefault();
				if (topOfStack != null)
					return Task.FromResult(topOfStack.GetModel());
			}
			return null;
		}
	}
}