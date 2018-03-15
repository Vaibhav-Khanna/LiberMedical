using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using libermedical.CustomControls;
using libermedical.iOS.Renderers;
using System.Linq;
using UIKit;
using Foundation;

[assembly:ExportRenderer (typeof(CustomRefreshListView),typeof(CustomRefreshListviewRenderer))]
namespace libermedical.iOS.Renderers
{
    public class CustomRefreshListviewRenderer : ListViewRenderer
    {

        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);

            var ele = Element as CustomRefreshListView;

            if(ele!=null)
            {
                var refreshControl = ((UITableViewController)ViewController).RefreshControl;

                if (refreshControl == null)
                    return;

                var titleLabel = (UILabel)refreshControl.Subviews[0].Subviews.Last();

                if (titleLabel != null)
                {
                    titleLabel.Lines = 0;
                    titleLabel.LineBreakMode = UILineBreakMode.WordWrap;
                    var title = ele.RefreshText;
                    refreshControl.AttributedTitle = new NSAttributedString(title);
                }
            }

        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var ele = Element as CustomRefreshListView;

            if (ele != null)
            {
                if (e.PropertyName == CustomRefreshListView.RefreshTextProperty.PropertyName)
                {
                    var refreshControl = ((UITableViewController)ViewController).RefreshControl;

                    if (refreshControl == null)
                        return;
                    
                    var titleLabel = (UILabel) refreshControl.Subviews[0].Subviews.Last();

                    if (titleLabel != null)
                    {
                        titleLabel.Lines = 0;
                        titleLabel.LineBreakMode = UILineBreakMode.WordWrap;
                        var title = ele.RefreshText;
                        refreshControl.AttributedTitle = new NSAttributedString(title);
                    }
                }
            }
        }

    }
}

