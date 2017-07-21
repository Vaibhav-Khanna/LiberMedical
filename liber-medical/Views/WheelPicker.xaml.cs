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

    // 1 wheel example

    /*
    public class MainPageModel
    {
        public IList<string> ItemsSource { get; } = new List<string>
    {
        "Lundi",
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

*/

    // 3 wheel

    public class MainPageModel
    {
        public IList<string> Wheel1 { get; } = new List<string>
          {
              "1",
              "2",
              "3",
              "4",
              "5",

          };
        public IList<string> Wheel2 { get; } = new List<string>
          {
              "Lundi",
              "MArdi",
              "Wednesday",
              "Thursday",
              "Friday",
              "Saturday",
              "Sunday",
          };
        public IList<string> Wheel3 { get; } = new List<string>
          {
              "1",
              "2",
              "3",
              "4",
              "5",

          };

        public IList<IList<string>> ItemsSource = new List<IList<string>>();




        public Command ItemSelectedCommand { get; }

        public MainPageModel()
        {
            ItemsSource.Add(Wheel1);
            ItemsSource.Add(Wheel2);
            ItemsSource.Add(Wheel3);

            ItemSelectedCommand = new Command<Tuple<int, int, IList<int>>>(tuple =>
            {
                var selectedWheelIndex = tuple.Item1;
                var selectedItemIndex = tuple.Item2;
                var selectedValue = ItemsSource[selectedItemIndex];
            });
        }
    }
}
