using FreshMvvm;
using libermedical.Models;
using libermedical.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace libermedical.ViewModels
{
    public class DetailsPatientListViewModel : FreshBasePageModel
    {
        private bool _showStackDocument;
        private bool _showStackOrdonnance;
        private bool _showBoxViewOrdonnances;
        private bool _showBoxViewDocuments;

        public bool ShowStackDocument { get { return _showStackDocument; } set { _showStackDocument = value; RaisePropertyChanged(); } }
        public bool ShowStackOrdonnance { get { return _showStackOrdonnance; } set { _showStackOrdonnance = value; RaisePropertyChanged(); } }
        public bool ShowBoxViewOrdonnances { get { return _showBoxViewOrdonnances; } set { _showBoxViewOrdonnances = value; RaisePropertyChanged(); } }
        public bool ShowBoxViewDocuments { get { return _showBoxViewDocuments; } set { _showBoxViewDocuments = value; RaisePropertyChanged(); } }

        public Patient Patient { get; set; }

        private ObservableCollection<Ordonnance> _ordonnances;
        public ObservableCollection<Ordonnance> Ordonnances { get { return _ordonnances; } set { _ordonnances = value; RaisePropertyChanged(); } }

        private ObservableCollection<Document> _documents;
        public ObservableCollection<Document> Documents { get { return _documents; } set { _documents = value; RaisePropertyChanged(); } }

        private string _bottomTitle;
        public string BottomTitle { get { return _bottomTitle; } set { _bottomTitle = value; RaisePropertyChanged(); } }
        public override void Init(object initData)
        {
            base.Init(initData);
            this.Patient = (Patient)initData;
            BindData();
        }

        private async void BindData()
        {
            ShowStackOrdonnance = ShowBoxViewOrdonnances = true;
            BottomTitle = "+ Ajoutez une ordonnance";
            Ordonnances = new ObservableCollection<Ordonnance>((await new StorageService<Ordonnance>().GetList()).Where(x =>  x.PatientId == Patient.Id));
            Documents = new ObservableCollection<Document>((await new StorageService<Document>().GetList()).Where(x => x.PatientId == Patient.Id));
        }
        public Command EditPatient
        {
            get
            {
                return new Command(async () =>
                {
                    await CoreMethods.PushPageModel<AddEditPatientViewModel>(Patient);
                });
            }
        }

        public ICommand OrdonnancesCommand
        {
            get
            {
                return new Command(() =>
                {
                    ShowStackDocument = false;
                    ShowStackOrdonnance = true;
                    ShowBoxViewOrdonnances = true;
                    ShowBoxViewDocuments = false;
                    BottomTitle = "+ Ajoutez une ordonnance";
                });
            }
        }

        public ICommand DocumentsCommand
        {
            get
            {
                return new Command(() =>
                {
                    ShowStackDocument = true;
                    ShowStackOrdonnance = false;
                    ShowBoxViewDocuments = true;
                    ShowBoxViewOrdonnances = false;
                    BottomTitle = "+ Ajoutez un document";
                });
            }
        }

        public Command AddOrdonnanceCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (BottomTitle == "+ Ajoutez une ordonnance")
                    {
                         await CoreMethods.PushPageModel<OrdonnanceDetailEditViewModel>();
                    }
                    else
                    {
                        await CoreMethods.PushPageModel<AddDocumentViewModel>();
                    }
                });
            }
        }

    }
}
