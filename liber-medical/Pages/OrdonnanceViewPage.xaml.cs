using System;
using System.Collections.Generic;
using FFImageLoading.Forms;
using libermedical.CustomControls;
using libermedical.Renderers;
using Xamarin.Forms;

namespace libermedical.Pages
{
    public partial class OrdonnanceViewPage : BasePage
    {
        public OrdonnanceViewPage() : base(-1, 0, false)
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
            {
                var zoom = new ZoomImageIOS() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Aspect = Aspect.AspectFit, BackgroundColor = Color.Black };
                zoom.SetBinding(CachedImage.SourceProperty, "ImageSource");
                stack_container.Children.Insert(0, zoom);
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                var zoom = new ZoomImageAndroid() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
                var image = new CachedImage() { DownsampleToViewSize = false, BitmapOptimizations = true, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Aspect = Aspect.AspectFit, BackgroundColor = Color.Black };
                image.SetBinding(CachedImage.SourceProperty, "ImageSource");
                zoom.Content = image;
                stack_container.Children.Insert(0, zoom);
            }

        }
        async void Back_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
