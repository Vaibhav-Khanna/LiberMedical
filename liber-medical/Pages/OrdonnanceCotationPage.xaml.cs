using libermedical.CustomControls;

namespace libermedical.Pages
{
    public partial class OrdonnanceCotationPage : BasePage
    {

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

            InitializeComponent();
        }

        void Handle_OnChanged(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            if (Switch.On)
            {
                Picker.IsVisible = true;
                Footer.IsVisible = true;
            }
            else
            {
                Picker.IsVisible = false;
                Footer.IsVisible = false;
            }
        }

        async void Cancel_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
