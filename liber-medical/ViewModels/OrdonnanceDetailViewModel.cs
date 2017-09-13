using libermedical.Models;
using libermedical.ViewModels.Base;

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
    }
}
