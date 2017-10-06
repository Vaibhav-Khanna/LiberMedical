using System;
using Xamarin.Forms;

namespace libermedical.Behaviours
{
    public class ButtonCotationBehaviour : Behavior<Button>
    {
        protected override void OnAttachedTo(Button button)
        {
            button.Clicked += Button_Clicked;
            base.OnAttachedTo(button);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (((Button)sender).BackgroundColor == (Color)Application.Current.Resources["HeaderFooterBackgroundColor"])
                ((Button)sender).BackgroundColor = (Color)Application.Current.Resources["LightGrey"];
            else
                ((Button)sender).BackgroundColor = (Color)Application.Current.Resources["HeaderFooterBackgroundColor"];
        }

        protected override void OnDetachingFrom(Button button)
        {
            button.Clicked -= Button_Clicked;
            base.OnDetachingFrom(button);
        }

    }
}
