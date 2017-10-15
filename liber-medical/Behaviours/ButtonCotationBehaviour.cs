using System;
using Xamarin.Forms;

namespace libermedical.Behaviours
{
	public class ButtonCotationBehaviour : Behavior<Button>
	{
		public static readonly BindableProperty CanEditProperty = BindableProperty.Create(nameof(CanEdit), typeof(bool), typeof(ButtonCotationBehaviour), default(bool));
		public bool CanEdit
		{
			get { return (bool)GetValue(CanEditProperty); }
			set { SetValue(CanEditProperty, value); }
		}

		protected override void OnAttachedTo(Button button)
		{
			button.Clicked += Button_Clicked;
			base.OnAttachedTo(button);
		}

		private void Button_Clicked(object sender, EventArgs e)
		{
			if (CanEdit)
			{
				if (((Button)sender).BackgroundColor == (Color)Application.Current.Resources["HeaderFooterBackgroundColor"])
					((Button)sender).BackgroundColor = (Color)Application.Current.Resources["LightGrey"];
				else
					((Button)sender).BackgroundColor = (Color)Application.Current.Resources["HeaderFooterBackgroundColor"];
			}
		}

		protected override void OnDetachingFrom(Button button)
		{
			button.Clicked -= Button_Clicked;
			base.OnDetachingFrom(button);
		}

	}
}
