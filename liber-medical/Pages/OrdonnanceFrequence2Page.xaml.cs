using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using libermedical.CustomControls;
using libermedical.Enums;
using libermedical.ViewModels;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace libermedical.Pages
{
    public partial class OrdonnanceFrequence2Page : BasePage
    {
        public List<string> Movements { get; set; }

        public OrdonnanceFrequence2Page() : base(-1, 64, false)
        {
            InitializeComponent();

            Movements = new List<string>
            {
                "Non", "IFD", "IFP", "IFO", "IFN", "IFR", "IFS"
            };

            MovementPicker.ItemsSource = Movements;

            MessagingCenter.Subscribe<OrdonnanceFrequence2ViewModel>(this, Events.SetInitialPickerValue, async (sender) =>
            {
                while (this.BindingContext == null)
                {
                    await Task.Delay(100);
                }

                if (!string.IsNullOrEmpty((BindingContext as OrdonnanceFrequence2ViewModel).Frequency.Movement))
                {
                    MovementPicker.SelectedIndex =
                        Movements.IndexOf((BindingContext as OrdonnanceFrequence2ViewModel).Frequency.Movement);
                }
                else
                {
                    MovementPicker.SelectedIndex = 0;
                }

                NightSwitch.On = (BindingContext as OrdonnanceFrequence2ViewModel).Frequency.Night;

                if ((BindingContext as OrdonnanceFrequence2ViewModel).Frequency.Increase == IncreaseEnum.Non)
                {
                    maj.Text = "Non";
                }
                else if ((BindingContext as OrdonnanceFrequence2ViewModel).Frequency.Increase == IncreaseEnum.MAU)
                {
                    maj.Text = "MAU";
                }
                else
                {
                    maj.Text = "MCI";
                }
            });
        }

        async void Cancel_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        void Majoration_Tapped(object sender, System.EventArgs e)
        {
            //Changement des majorations à la tap sur la zone
            string[] tab = { "Non", "MAU", "MCI" };
            var cot = maj.Text;

            for (var i = 0; i < tab.Length; i++)
            {
                if (cot == tab[i] && i < tab.Length - 1)
                {
                    maj.Text = tab[i + 1];
                }
                if (cot == tab[i] && i == tab.Length - 1)
                {
                    maj.Text = tab[0];
                }
            }
            (this.BindingContext as OrdonnanceFrequence2ViewModel).Frequency.Increase = maj.Text == tab[0]
                ? IncreaseEnum.Non
                : maj.Text == tab[1]
                    ? IncreaseEnum.MAU
                    : IncreaseEnum.MCI;
        }

        void Deplacement_Tapped(object sender, System.EventArgs e)
        {
            MovementPicker.Focus();
        }

        private void NightOnChanged(object sender, ToggledEventArgs e)
        {
            (this.BindingContext as OrdonnanceFrequence2ViewModel).Frequency.Night = e.Value;
        }

        private void Picker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            (this.BindingContext as OrdonnanceFrequence2ViewModel).Frequency.Movement =
                Movements[MovementPicker.SelectedIndex];
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (listCotations.ItemsSource != null)
                CotationsViewCell.Height = listCotations.HeightRequest = (listCotations.ItemsSource as ObservableCollection<string>).Count * 45;
            else
                CotationsViewCell.Height = listCotations.HeightRequest = 0;

        }
    }
}
