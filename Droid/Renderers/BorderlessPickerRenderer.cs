using libermedical.CustomControls;
using libermedical.Droid.Renderers;
using System.ComponentModel;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BorderlessPicker), typeof(BorderlessPickerRenderer))]
namespace libermedical.Droid.Renderers
{
    public class BorderlessPickerRenderer : PickerRenderer
    {

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

          

            if (Element != null)
            {
                var ele = Element as BorderlessPicker;

                if (ele.Text_Aligment == Xamarin.Forms.TextAlignment.Center)
                {
                    // Left over Default Behaviour
                }
                else if (ele.Text_Aligment == Xamarin.Forms.TextAlignment.End)
                {
                    Control.TextAlignment = Android.Views.TextAlignment.ViewEnd;
                }
                else if (ele.Text_Aligment == Xamarin.Forms.TextAlignment.Start)
                {
                    Control.TextAlignment = Android.Views.TextAlignment.ViewStart;
                }
            }

        }
    }
}
