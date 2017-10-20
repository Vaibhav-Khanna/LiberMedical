using libermedical.CustomControls;
using libermedical.iOS.Renderers;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NonScrollableList), typeof(NonScrollableListRenderer))]
namespace libermedical.iOS.Renderers
{
    public class NonScrollableListRenderer : ListViewRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            Control.ScrollEnabled = false;
        }
    }
}
