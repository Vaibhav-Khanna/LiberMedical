using System.Windows.Input;
using libermedical.Models;
using libermedical.ViewModels.Base;
using Xamarin.Forms;

namespace libermedical.ViewModels
{
    public class OrdonnanceDetailViewModel : ViewModelBase
    {
        public Ordonnance Ordonnance { get; set; }

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
    }
}
