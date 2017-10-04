using System.Windows.Input;
using libermedical.Models;
using libermedical.ViewModels.Base;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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

        public OrdonnanceFrequence2ViewModel()
        {
            
        }

        private void SubscribeMessage()
        {
            MessagingCenter.Subscribe<OrdonnanceCotationViewModel,Frequency>(this, Events.UpdateCotations, ((sender,args) => {

                if (args != null)
                {
                    Frequency = args as Frequency;
                    Cotations = new ObservableCollection<string>(Frequency.Quotations) ?? null;
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
                Cotations = Frequency.Quotations!=null? new ObservableCollection<string>(Frequency.Quotations) : null;
            }

            MessagingCenter.Send(this, Events.SetInitialPickerValue);
        }

        public ICommand SaveTappedCommand => new Command(async () =>
        {
            await CoreMethods.PopPageModel(null, true);
            await CoreMethods.PopPageModel(null, true);
        });

        public ICommand CotationsTappedCommand => new Command(async () =>
        {
            SubscribeMessage();
            await CoreMethods.PushPageModel<OrdonnanceCotationViewModel>(Frequency, true);
        });
    }
}
