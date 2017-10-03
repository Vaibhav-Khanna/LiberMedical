using System;
using System.Collections.Generic;
using libermedical.Models;
using libermedical.ViewModels.Base;
using Xamarin.Forms;
using System.Windows.Input;

namespace libermedical.ViewModels
{
    public class OrdonnanceCotationViewModel : ViewModelBase
    {
        private bool _shouldEnableAdd;
        public bool ShouldEnableAdd
        {
            get { return _shouldEnableAdd; }
            set
            {
                _shouldEnableAdd = value;
                RaisePropertyChanged();
            }
        }
        public Frequency Frequency { get; set; }
        public IList<string> ItemsSource { get; } = new List<string>
        {
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday",
            "Sunday"
        };
        public Command ItemSelectedCommand { get; }


        public OrdonnanceCotationViewModel()
        {
            ItemSelectedCommand = new Command<Tuple<int, int, IList<int>>>(tuple =>
            {
                var selectedWheelIndex = tuple.Item1;
                var selectedItemIndex = tuple.Item2;
                var selectedValue = ItemsSource[selectedItemIndex];
            });
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            if (initData != null)
            {
                Frequency = initData as Frequency;
            }
        }

        public ICommand SelectCotationCommand
        {
            get
            {
                return new Command((args) =>
                {
                    if (Frequency == null || Frequency.Quotations==null)
                    {
                        Frequency = new Frequency();
                        Frequency.Quotations = new List<string>();
                    }
                    Frequency.Quotations.Add((string)args);
                    ShouldEnableAdd = true;
                });
            }
        }

        public ICommand AddCommand
        {
            get
            {
                return new Command(async() =>
                {
                    MessagingCenter.Send(this, Events.UpdateCotations, Frequency);
                    await App.Current.MainPage.Navigation.PopModalAsync(true);
                });
            }
        }
    }
}
