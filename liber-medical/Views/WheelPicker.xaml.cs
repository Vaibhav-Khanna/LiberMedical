using System;
using System.Collections.Generic;
using libermedical.CustomControls;
using Xamarin.Forms;

namespace libermedical.Views
{
    public partial class WheelPicker : BasePage
    {
        public WheelPicker()
        {
            InitializeComponent();
            BindingContext = new MainPageModel();
        }
    }

    public class MainPageModel
    {
        public IList<string> ItemsSource { get; } = new List<string>
        {
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday",
            "Sunday",
        };

        public Command ItemSelectedCommand { get; }

        public MainPageModel()
        {
            ItemSelectedCommand = new Command<Tuple<int, int, IList<int>>>(tuple =>
            {
                var selectedWheelIndex = tuple.Item1;
                var selectedItemIndex = tuple.Item2;
                var selectedValue = ItemsSource[selectedItemIndex];
            });
        }
    }
}
