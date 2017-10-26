using System;
using Xamarin.Forms;

namespace libermedical.CustomControls
{
	public class BorderlessPicker : Picker
	{
		

        public static readonly BindableProperty Text_AlignmentProperty = BindableProperty.Create(nameof(Text_Aligment), typeof(TextAlignment), typeof(BorderlessPicker), TextAlignment.Center );
       
        public TextAlignment Text_Aligment
        {
            get { return (TextAlignment)GetValue(Text_AlignmentProperty); }
            set { SetValue(Text_AlignmentProperty, value); }
        }

	}
}
