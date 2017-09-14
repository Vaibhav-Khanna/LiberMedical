using libermedical.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace libermedical.ViewModels
{
    class SecuriseBillsViewModel : ViewModelBase
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
