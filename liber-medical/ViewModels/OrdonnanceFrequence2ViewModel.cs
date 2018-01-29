using System.Windows.Input;
using libermedical.Models;
using libermedical.ViewModels.Base;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using libermedical.PopUp;
using System.Threading.Tasks;

namespace libermedical.ViewModels
{
    public class OrdonnanceFrequence2ViewModel : ViewModelBase
    {
        private bool _canEdit;
        public bool CanEdit
        {
            get { return _canEdit; }
            set
            {
                _canEdit = value;
                RaisePropertyChanged();
            }
        }
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
        private int _height = 45;
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
            SubscribeEditModeMessage();
        }

        private void SubscribeCotationsMessage()
        {
            MessagingCenter.Subscribe<OrdonnanceCotationViewModel, Frequency>(this, Events.UpdateCotations, ((sender, args) =>
            {

                if (args != null)
                {
                    var frequency = args as Frequency;
                    Cotations = frequency.Quotations != null ? new ObservableCollection<string>(frequency.Quotations) : new ObservableCollection<string>();
                    //Frequency.Quotations = Cotations.ToList();

                    Show.Execute(null);

                    MessagingCenter.Send(this, Events.UpdateCotationsViewCellHeight, Cotations);
                }

                 MessagingCenter.Unsubscribe<OrdonnanceCotationViewModel, Frequency>(this, Events.UpdateCotations);
            }));
        }


        public ICommand Show => new Command(async (args) =>
        {
            await Task.Delay(1000);
            Device.BeginInvokeOnMainThread( async() => 
            {
                await ToastService.Show("Informations enregistrées avec succès");
            }); 
        }); 


        private void SubscribeEditModeMessage()
        {
            MessagingCenter.Unsubscribe<OrdonnanceCreateEditViewModel, bool>(this, Events.EnableCotationsEditMode);
            MessagingCenter.Subscribe<OrdonnanceCreateEditViewModel, bool>(this, Events.EnableCotationsEditMode,
                        (sender, canEdit) =>
                        {
                            CanEdit = canEdit;
                        });
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            if (initData != null)
            {
                Frequency = initData as Frequency;
                Cotations = Frequency.Quotations != null ? new ObservableCollection<string>(Frequency.Quotations) : new ObservableCollection<string>();
                MessagingCenter.Send(this, Events.UpdateCotationsViewCellHeight, Cotations);
            }

            MessagingCenter.Send(this, Events.SetInitialPickerValue);
        }

        public ICommand SaveTappedCommand => new Command(async () =>
        {
            if(Cotations==null || !Cotations.Any())
            {
                await CoreMethods.DisplayAlert("Alerte","Veuillez d’abord ajouter une cotation","Ok");
                return;
            }

            Frequency.Quotations = Cotations.ToList();
            await CoreMethods.PopPageModel(null, true);
            //await CoreMethods.PopPageModel(null, true);
            MessagingCenter.Send(this, Events.UpdateFrequencies, Frequency);
        });

        public ICommand CotationsTappedCommand => new Command(async () =>
        {
            if (CanEdit)
            {
                SubscribeCotationsMessage();

                await CoreMethods.PushPageModel<OrdonnanceCotationViewModel>(new Frequency(){ Quotations = Cotations.ToList() }, true);
                MessagingCenter.Send(this, Events.EnableCotationsEditMode, true);
            }

        });

        public ICommand DeleteCotation => new Command((args) =>
        {
            if (CanEdit)
            {
                Cotations.Remove(args as string);
                ToastService.Show("Informations enregistrées avec succès");
                //Frequency.Quotations = Cotations.ToList();
                MessagingCenter.Send(this, Events.UpdateCotationsViewCellHeight, Cotations);
            }
        });
    }
}
