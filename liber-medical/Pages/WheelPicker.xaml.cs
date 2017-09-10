using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace libermedical.Pages
{
    public partial class WheelPicker : ContentPage
    {
        public WheelPicker()
        {
            InitializeComponent();
            BindingContext = new MainPageModel();
        }
    }

    public class MainPageModel
    {
        public IList<IList<string>> ItemsSource = new List<IList<string>>
        {
            new List<string>
            {
                "1",
                "2",
                "3",
                "4",
                "5",
                "6",
                "7",
            },
            new List<string>
            {
                "Monday",
                "Tuesday",
                "Wednesday",
                "Thursday",
                "Friday",
                "Saturday",
                "Sunday",
            },
            new List<string>
            {
                "1",
                "2",
                "3",
                "4",
                "5",
                "6",
                "7",
            },
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
