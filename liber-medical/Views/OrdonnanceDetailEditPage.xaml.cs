using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using libermedical.CustomControls;
using libermedical.Models;
using Xamarin.Forms;

namespace libermedical.Views
{
    public partial class OrdonnanceDetailEditPage : BasePage
    {
        public ObservableCollection<Cotation> cotation { get; set; }

        public OrdonnanceDetailEditPage() : base(-1, -1, false)
        {
            BindingContext = this;
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
                }



            };

            InitializeComponent();




        }
        async void Cancel_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new HomePage());
        }
        async void Save_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new HomePage());
        }

    }
}
