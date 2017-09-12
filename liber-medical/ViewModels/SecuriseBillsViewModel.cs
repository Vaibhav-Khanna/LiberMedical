using libermedical.ViewModels.Base;
using System.IO;
using System.Windows.Input;
using Xamarin.Forms;

namespace libermedical.ViewModels
{
    public class SecuriseBillsViewModel : ViewModelBase
    {
        private Stream m_pdfDocumentStream;
        public Stream PdfDocumentStream
        {
            get
            {
                return m_pdfDocumentStream;
            }
            set
            {
                m_pdfDocumentStream = value;
                RaisePropertyChanged();
            }
        }
        public SecuriseBillsViewModel()
        {

        }

        public ICommand BackCommand => new Command(async () => await Application.Current.MainPage.Navigation.PopModalAsync());
    }
}

