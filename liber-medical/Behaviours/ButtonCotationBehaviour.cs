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
            if (((Button)sender).BackgroundColor == Color.FromHex("#91c602"))
                ((Button)sender).BackgroundColor = Color.FromHex("#d1d1d1");
            else
                ((Button)sender).BackgroundColor = Color.FromHex("#91c602");
        }

        protected override void OnDetachingFrom(Button button)
        {
            button.Clicked -= Button_Clicked;
            base.OnDetachingFrom(button);
        }

    }
}
