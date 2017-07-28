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
        //  public ObservableCollection<Cotation> cotation { get; set; }

        public OrdonnanceCotationPage() : base(-1, 64, false)
        {
            /*  cotation = new ObservableCollection<Cotation>
                  {
                  new Cotation {
                      FirstCode=1,
                      SecondCode= "AMI",
                      ThirdCode= 1
                  },
                  new Cotation {
                      FirstCode=1,
                      SecondCode= "AMI",
                      ThirdCode= 2
                  },
                  new Cotation {
                      FirstCode=1,
                      SecondCode= "AMI",
                      ThirdCode= 4
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
              }; */
            BindingContext = this;
            InitializeComponent();

        }

        void Handle_OnChanged(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            if (Switch.On == true) { Picker.IsVisible = true; Footer.IsVisible = true; }
            else { Picker.IsVisible = false; Footer.IsVisible = false; }
        }

        async void Cancel_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }



    }
}
