using System;
using System.Collections.Generic;
using Synchro.CustomRenderer;
using Xamarin.Forms;
using FFImageLoading.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Synchro.Pages
{
    public partial class ImageViewPage : ContentPage
    {
        public ImageViewPage()
        {
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
           
            InitializeComponent();
           
            if(Device.RuntimePlatform == Device.iOS)
            {
                var zoom = new ZoomImageIOS() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand, Aspect = Aspect.AspectFit, BackgroundColor = Color.Black };
                zoom.SetBinding(CachedImage.SourceProperty,"Image");
                stack_container.Children.Insert(0,zoom);
            }
            else if(Device.RuntimePlatform == Device.Android)
            {
                var zoom = new ZoomImageAndroid() { HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
                var image = new CachedImage(){ DownsampleToViewSize = false, BitmapOptimizations = true, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand,Aspect = Aspect.AspectFit, BackgroundColor = Color.Black };
                image.SetBinding(CachedImage.SourceProperty, "Image");
                zoom.Content = image;
                stack_container.Children.Insert(0,zoom);
            }

        }
    }
}
