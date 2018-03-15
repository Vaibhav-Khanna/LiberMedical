using System;
using System.Collections.Generic;
using System.Linq;
using libermedical.CustomControls;
using libermedical.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ToolbarContentPage), typeof(ToolBarPageRenderer))]
namespace libermedical.iOS.Renderers
{
   
    public class ToolBarPageRenderer : PageRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
        }


        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (NavigationController == null)
            return;

            try
            {
                var itemsInfo = (this.Element as ContentPage).ToolbarItems;

                if (itemsInfo?.Count < 1)
                    return;

                var navigationItem = this.NavigationController.TopViewController.NavigationItem;

                var leftNativeButtons = (navigationItem.LeftBarButtonItems ?? new UIBarButtonItem[] { }).ToList();

                var rightNativeButtons = (navigationItem.RightBarButtonItems ?? new UIBarButtonItem[] { }).ToList();


                var rightnewlist = new List<UIBarButtonItem>();

                rightNativeButtons.ForEach((obj) =>
                   {

                       var info = GetButtonInfo(itemsInfo.ToList(), obj.Title);

                       if (info != null)
                       {
                           if (info.Priority == 0)
                           {
                               rightnewlist.Add(obj);
                           }
                           else
                           {
                               leftNativeButtons.Add(obj);
                           }

                       }


                   });

                navigationItem.RightBarButtonItems = rightnewlist.ToArray();
                navigationItem.LeftBarButtonItems = leftNativeButtons.ToArray();
            
            }
            catch(Exception)
            {
                
            }

        }


            private ToolbarItem GetButtonInfo(List<ToolbarItem> items, string name)
           {
                if ( items == null)
                    return null;
                    
                 return items.Where( (arg1) => arg1.Text ==(name)).FirstOrDefault();
           }

    }    

}

