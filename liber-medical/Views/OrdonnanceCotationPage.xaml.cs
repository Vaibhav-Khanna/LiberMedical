using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using libermedical.CustomControls;
using libermedical.Models;
using Xamarin.Forms;

namespace libermedical.Views
{
    public partial class OrdonnanceCotationPage : BasePage
    {
        public ObservableCollection<Cotation> cotation { get; set; }

        public OrdonnanceCotationPage() : base(-1, -1, false)
        {
            cotation = new ObservableCollection<Cotation>
                {
                new Cotation {
                    FirstCode=1,
                    SecondCode= "AMI",
                    ThirdCode= 2
                },
                new Cotation {
                    FirstCode=2,
                    SecondCode= "AMI",
                    ThirdCode= 1
                },
                new Cotation {
                    FirstCode=3,
                    SecondCode= "AMK",
                    ThirdCode= 5
                },
                new Cotation {
                    FirstCode=3,
                    SecondCode= "AMK",
                    ThirdCode= 5
                },
                new Cotation {
                    FirstCode=3,
                    SecondCode= "AMK",
                    ThirdCode= 5
                },
                new Cotation {
                    FirstCode=3,
                    SecondCode= "AMK",
                    ThirdCode= 5
                },
                new Cotation {
                    FirstCode=3,
                    SecondCode= "AMK",
                    ThirdCode= 5
                },
                new Cotation {
                    FirstCode=3,
                    SecondCode= "AMK",
                    ThirdCode= 5
                },
                new Cotation {
                    FirstCode=3,
                    SecondCode= "AMK",
                    ThirdCode= 5
                }
            };
            BindingContext = this;
            InitializeComponent();
            //  BindingContext = new MainPageModel();
        }
        async void Cancel_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }


        /* public class MainPageModel
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
             "Monday",
             "Tuesday",
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
             ItemsSource.Add(Wheel1);
             ItemsSource.Add(Wheel2);
             ItemsSource.Add(Wheel3);


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
         }*/
    }
}
