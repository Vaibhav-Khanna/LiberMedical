using libermedical.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(TabbedPageRenderer))]

namespace libermedical.iOS.Renderers
{
	public class TabbedPageRenderer : TabbedRenderer
	{
		protected override void OnElementChanged(VisualElementChangedEventArgs e)
		{
			base.OnElementChanged(e);

			TabBar.TintColor = UIColor.Green;
			var uiColor = Color.FromHex("#4a4a4a").ToUIColor();
			TabBar.BarTintColor = uiColor;
			TabBar.BackgroundColor = uiColor;
			TabBar.UnselectedItemTintColor = UIColor.White;
		}


		//	Changing of default tab bar behaviour was made with manuals:
		//	https://stackoverflow.com/questions/18734794/how-can-i-change-the-text-and-icon-colors-for-tabbaritems-in-ios-7/19533368#19533368
		//	http://stackoverflow.com/questions/12771212/custom-uitabbaritem-without-title-label
		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			var items = TabBar.Items;
			foreach (var item in items)
			{
				item.ImageInsets = new UIEdgeInsets(6.0f, 0, -6.0f, 0);
			}
		}
	}
}