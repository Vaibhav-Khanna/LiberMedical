using System.Windows.Input;
using libermedical.Models;
using libermedical.ViewModels.Base;
using Xamarin.Forms;

namespace libermedical.ViewModels
{
    public class OrdonnanceDetailViewModel : ViewModelBase
    {
        private Ordonnance _ordonnance;
        public Ordonnance Ordonnance
        {
            get { return _ordonnance; }
            set
            {
                _ordonnance = value;
                RaisePropertyChanged();
            }
        }

        public OrdonnanceDetailViewModel()
        {
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            if (initData != null)
            {
                Ordonnance = initData as Ordonnance;
            }
        }

        public ICommand CreateOrdonnanceCommand => new Command(async () =>
        {
            await CoreMethods.PushPageModel<OrdonnanceCreateEditViewModel>(Ordonnance, true);
        });

        public ICommand OpenDocumentCommand => new Command(async () =>
        {
            await CoreMethods.PushPageModel<OrdonnanceViewViewModel>(Ordonnance, true);
        });

        public ICommand CloseCommand => new Command(async () =>
        {
            await CoreMethods.PopPageModel();
        });

       
    }
}
