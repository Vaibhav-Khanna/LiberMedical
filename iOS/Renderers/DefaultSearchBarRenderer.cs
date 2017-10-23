using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using libermedical.iOS.Renderers;

[assembly: ExportRenderer(typeof(SearchBar),typeof(DefaultSearchBarRenderer))]
namespace libermedical.iOS.Renderers
{
    public class DefaultSearchBarRenderer : SearchBarRenderer
    {

        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);


            if(Control!=null)
            {
                Control.ShowsCancelButton = false;
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            Control.ShowsCancelButton = false;
        }

    }
}
