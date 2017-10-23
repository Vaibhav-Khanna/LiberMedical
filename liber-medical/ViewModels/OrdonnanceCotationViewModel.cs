﻿using System;
using System.Collections.Generic;
using libermedical.Models;
using libermedical.ViewModels.Base;
using Xamarin.Forms;
using System.Windows.Input;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections;

namespace libermedical.ViewModels
{
    public class OrdonnanceCotationViewModel : ViewModelBase
    {
        public ObservableCollection<object> CotationManual { get; set; }

        public ObservableCollection<object> CotationManual1 = new ObservableCollection<object>() { "1", "2", "3", "4", "5" };
        public ObservableCollection<object> CotationManual2 = new ObservableCollection<object>() { "AMI", "AMS", "AMK", "AMT", "AMJ" };

        public bool CanEdit { get; set; }

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

        private bool _hasManualCotations;
        public bool HasManualCotations
        {
            get { return _hasManualCotations; }
            set
            {
                _hasManualCotations = value;
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

        private object _selected;
        public object Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                var newValue = _selected as IList;
                if (newValue != null && newValue.Count == 3)
                    _selected = $"{newValue[0]} {newValue[1]} {newValue[2]}";                
                RaisePropertyChanged();
            }

        }

        public OrdonnanceCotationViewModel()
        {
            try
            {
                SubscribeMessages();
                CotationManual = new ObservableCollection<object>();
                CotationManual.Add(CotationManual1);
                CotationManual.Add(CotationManual2);
                CotationManual.Add(CotationManual1);
                Selected = new ObservableCollection<object>() { "1", "AMI", "1" };
                ItemSelectedCommand = new Command<Tuple<int, int, IList<int>>>(tuple =>
                {
                    var selectedWheelIndex = tuple.Item1;
                    var selectedItemIndex = tuple.Item2;
                    var selectedValue = ItemsSource[selectedItemIndex];
                });
            }
            catch (Exception ex)
            {

            }
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
                    if (CanEdit)
                    {
                        if ((string)args == "1 AMI 1")
                            AMI1 = !AMI1;
                        if ((string)args == "1 AMI 2")
                            AMI2 = !AMI2;
                        if ((string)args == "1 AMI 4")
                            AMI4 = !AMI4;
                        if ((string)args == "1 AMI 1.5")
                            AMI15 = !AMI15;
                        if ((string)args == "1 AIS 3")
                            AIS3 = !AIS3;
                        if ((string)args == "1 AIS 4")
                            AIS4 = !AIS4;
                        if ((string)args == "2 AIS 3")
                            AIS32 = !AIS32;
                        if ((string)args == "1 AMS 7.5")
                            AMS75 = !AMS75;
                        if ((string)args == "1 AMS 9.5")
                            AMS95 = !AMS95;

                        if (Frequency == null || Frequency.Quotations == null)
                        {
                            Frequency = new Frequency();
                            Frequency.Quotations = new List<string>();
                        }
                        Frequency.Quotations.Add((string)args);
                        ShouldEnableAdd = true;
                    }
                });
            }
        }

        public ICommand AddCommand
        {
            get
            {
                return new Command(async () =>
                {
                    MessagingCenter.Send(this, Events.UpdateCotations, Frequency);
                    await App.Current.MainPage.Navigation.PopModalAsync(true);
                });
            }
        }

        private void SubscribeMessages()
        {
            MessagingCenter.Unsubscribe<OrdonnanceFrequence2ViewModel, bool>(this, Events.EnableCotationsEditMode);
            MessagingCenter.Subscribe<OrdonnanceFrequence2ViewModel, bool>(this, Events.EnableCotationsEditMode,
            (sender, canEdit) =>
            {
                CanEdit = canEdit;
            });

            MessagingCenter.Unsubscribe<OrdonnanceCreateEditViewModel, bool>(this, Events.EnableCotationsEditMode);
            MessagingCenter.Subscribe<OrdonnanceCreateEditViewModel, bool>(this, Events.EnableCotationsEditMode,
                        (sender, canEdit) =>
                        {
                            CanEdit = canEdit;
                        });
        }

        private bool _ami1;
        public bool AMI1
        {
            get { return _ami1; }
            set
            {
                _ami1 = value;
                RaisePropertyChanged();
            }
        }
        private bool _ami2;
        public bool AMI2
        {
            get { return _ami2; }
            set
            {
                _ami2 = value;
                RaisePropertyChanged();
            }
        }
        private bool _ami4;
        public bool AMI4
        {
            get { return _ami4; }
            set
            {
                _ami4 = value;
                RaisePropertyChanged();
            }
        }
        private bool _ami15;
        public bool AMI15
        {
            get { return _ami15; }
            set
            {
                _ami15 = value;
                RaisePropertyChanged();
            }
        }
        private bool _ais3;
        public bool AIS3
        {
            get { return _ais3; }
            set
            {
                _ais3 = value;
                RaisePropertyChanged();
            }
        }
        private bool _ais4;
        public bool AIS4
        {
            get { return _ais4; }
            set
            {
                _ais4 = value;
                RaisePropertyChanged();
            }
        }
        private bool _ais32;
        public bool AIS32
        {
            get { return _ais32; }
            set
            {
                _ais32 = value;
                RaisePropertyChanged();
            }
        }
        private bool _ams75;
        public bool AMS75
        {
            get { return _ams75; }
            set
            {
                _ams75 = value;
                RaisePropertyChanged();
            }
        }
        private bool _ams95;
        public bool AMS95
        {
            get { return _ams95; }
            set
            {
                _ams95 = value;
                RaisePropertyChanged();
            }
        }
    }
}
