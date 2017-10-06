using libermedical.Models;
using libermedical.ViewModels.Base;
using Xamarin.Forms;

namespace libermedical.ViewModels
{
    public class OrdonnanceViewViewModel : ViewModelBase
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged();
            }
        }

        private ImageSource _imageSource;
        public ImageSource ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
                RaisePropertyChanged();
            }
        }
        public Ordonnance Ordonnance { get; set; }
        public OrdonnanceViewViewModel()
        {

        }

        public override void Init(object initData)
        {
            base.Init(initData);
            if (initData != null)
            {
                Ordonnance = initData as Ordonnance;
                Title = Ordonnance.PatientName;
                ImageSource = Ordonnance.Attachments[0];
            }
        }
    }
}
