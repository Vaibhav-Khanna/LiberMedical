using System.Windows.Input;
using libermedical.Models;
using libermedical.ViewModels.Base;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace libermedical.ViewModels
{
    public class OrdonnanceFrequence2ViewModel : ViewModelBase
    {
        private ObservableCollection<string> _cotations;
        public ObservableCollection<string> Cotations
        {
            get { return _cotations; }
            set
            {
                _cotations = value;
                RaisePropertyChanged();
                if (Cotations.Count != 0)
                    Height = Cotations.Count * 45;
            }
        }

        private Frequency _frequency;
        public Frequency Frequency
        {
            get { return _frequency; }
            set
            {
                _frequency = value;                
                RaisePropertyChanged();    
            }
        }
        private int _height=45;
        public int Height
        {
            get { return _height; }
            set
            {
                _height = value;
                RaisePropertyChanged();
            }
        }

        public OrdonnanceFrequence2ViewModel()
        {
            
        }

        private void SubscribeMessage()
        {
            MessagingCenter.Subscribe<OrdonnanceCotationViewModel,Frequency>(this, Events.UpdateCotations, ((sender,args) => {

                if (args != null)
                {
                    var frequency = args as Frequency;
                    Cotations = frequency.Quotations != null ? new ObservableCollection<string>(frequency.Quotations.Distinct()) : new ObservableCollection<string>();
                }

                MessagingCenter.Unsubscribe<OrdonnanceCotationViewModel, Frequency>(this, Events.UpdateCotations);
            }));
        }
        public override void Init(object initData)
        {
            base.Init(initData);
            if (initData != null)
            {
                Frequency = initData as Frequency;
                Cotations = Frequency.Quotations!=null? new ObservableCollection<string>(Frequency.Quotations.Distinct()) : new ObservableCollection<string>();
            }

            MessagingCenter.Send(this, Events.SetInitialPickerValue);
        }

        public ICommand SaveTappedCommand => new Command(async () =>
        {
            Frequency.Quotations = Cotations.ToList();
            await CoreMethods.PopPageModel(null, true);
            await CoreMethods.PopPageModel(null, true);
            MessagingCenter.Send(this,Events.UpdateFrequencies,Frequency);
        });

        public ICommand CotationsTappedCommand => new Command(async () =>
        {
            SubscribeMessage();
            await CoreMethods.PushPageModel<OrdonnanceCotationViewModel>(Frequency, true);
        });
        public ICommand DeleteCotation => new Command((args) =>
        {
            Cotations.Remove(args as string);
        });
    }
}
