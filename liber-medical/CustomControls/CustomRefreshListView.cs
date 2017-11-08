using System;

using Xamarin.Forms;

namespace libermedical.CustomControls
{
    public class CustomRefreshListView : ListView
    {
        
        public static readonly BindableProperty RefreshTextProperty = BindableProperty.Create(nameof(RefreshText), typeof(string), typeof(CustomRefreshListView), string.Empty );

        public string RefreshText
        {
            get { return (string)GetValue(RefreshTextProperty); }
            set { SetValue(RefreshTextProperty, value); }
        }
    }
}

