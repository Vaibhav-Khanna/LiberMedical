using libermedical.CustomControls;
using libermedical.iOS.Renderers;
using System.ComponentModel;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BorderlessPicker), typeof(BorderlessPickerRenderer))]
namespace libermedical.iOS.Renderers
{
    public class BorderlessPickerRenderer : PickerRenderer
	{
				
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			Control.Layer.BorderWidth = 0;
			Control.BorderStyle = UITextBorderStyle.None;

            if (Element != null)
            {
                var ele = Element as BorderlessPicker;

                if (ele.Text_Aligment == TextAlignment.Center)
                {
                    Control.TextAlignment = UITextAlignment.Justified;
                }
                else if (ele.Text_Aligment == TextAlignment.End)
                {
                    Control.TextAlignment = UITextAlignment.Right;
                }
                else if (ele.Text_Aligment == TextAlignment.Start)
                {
                    Control.TextAlignment = UITextAlignment.Left;
                }
            }

		}
	}
}
