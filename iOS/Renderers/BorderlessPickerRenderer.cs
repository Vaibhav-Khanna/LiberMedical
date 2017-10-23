﻿using libermedical.CustomControls;
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
		public static void Init() { }
		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			Control.Layer.BorderWidth = 0;
			Control.BorderStyle = UITextBorderStyle.None;
		}
	}
}