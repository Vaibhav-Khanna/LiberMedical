using libermedical.CustomControls;
using libermedical.Enums;
using libermedical.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace libermedical.Pages
{
    public partial class OrdonnanceFrequence2Page : BasePage
	{
		public List<string> Movements { get; set; }

		public OrdonnanceFrequence2Page() : base(-1, 64, false)
		{
			InitializeComponent();
            SubscribeMessages();

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
                Device.BeginInvokeOnMainThread(() =>
                {
                    UpdateCotationsViewCellHeight(new ObservableCollection<string>((BindingContext as OrdonnanceFrequence2ViewModel).Cotations));
                });
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
           // UpdateListAsync();
			
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
		private async void UpdateListAsync()
		{
			while (this.BindingContext == null)
			{
				await Task.Delay(100);
			}


			var items = listCotations.ItemsSource != null ? (listCotations.ItemsSource as ObservableCollection<string>) : new ObservableCollection<string>();
			if (items.Count == 0)
			{
				CotationsViewCell.Height = listCotations.HeightRequest = 0;
			}
			else
			{
				CotationsViewCell.Height = listCotations.HeightRequest = (listCotations.ItemsSource as ObservableCollection<string>).Count * 40 + 10;
			}
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();

        }

        private void SubscribeMessages()
        {
            MessagingCenter.Subscribe<OrdonnanceFrequence2ViewModel, ObservableCollection<string>>(this, Events.UpdateCotationsViewCellHeight, (sender, args) =>
            {
                UpdateCotationsViewCellHeight(args);
            });

        }

        private void UpdateCotationsViewCellHeight(ObservableCollection<string> cotations)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                listCotations.ItemsSource = cotations;
                CotationsViewCell.Height = cotations.Count * 43;

            });
        }
    }
}
