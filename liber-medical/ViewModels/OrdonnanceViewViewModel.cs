using libermedical.Helpers;
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

        private double downloadprogress = 0;
        public double Downloadprogress{
            get { return downloadprogress; }
            set
            {
                downloadprogress = value;
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
        public Teledeclaration Teledeclaration { get; set; }

        public Command DownloadStarted => new Command(() =>
       {

       });

        public Command Downloading => new Command((obj) =>
       {
            var args = obj as FFImageLoading.Forms.CachedImageEvents.DownloadProgressEventArgs;
            if(args!=null)
            {
                Downloadprogress = (double) ( (double)args.DownloadProgress.Current / (double)args.DownloadProgress.Total);
                if (downloadprogress >= 0.98)
                   Downloadprogress = 0;
            }

       });

        public OrdonnanceViewViewModel()
        {

        }
      
        private string GetDocumentPath(string path)
        {
            if (!string.IsNullOrEmpty(path))
            if (path.StartsWith("Ordonnance/") || path.StartsWith("PatientDocuments/") || path.StartsWith("contract/") || path.StartsWith("invoice/") || path.ToLower().StartsWith("teledeclarations/"))
                {
                    return $"{Constants.RestUrl}file?path={System.Net.WebUtility.UrlEncode(path)}&token={Settings.Token}";
                }
                else
                    return path;
            else
                return string.Empty;
        }

        public override void Init(object initData)
        {
            base.Init(initData);
            if (initData != null)
            {
                if (initData is Ordonnance)
                {
                    Ordonnance = initData as Ordonnance;
                    Title = Ordonnance.PatientName;
                    if (Ordonnance.Attachments != null && Ordonnance.Attachments.Count > 0)
                        ImageSource = GetDocumentPath(Ordonnance.Attachments[0]);
                }
                else if (initData is Document)
                {
                    var document = initData as Document;
                    Title = document.Patient?.Fullname;
                    ImageSource = GetDocumentPath(document.AttachmentPath);
                }
                else if (initData is string)
                {
                    Title = System.IO.Path.GetFileNameWithoutExtension((string)initData);
                    ImageSource = GetDocumentPath((string)initData);
                }
                else if (initData is Invoice)
                {
                    var invoice = initData as Invoice;
                    Title = invoice.Label;
                    ImageSource = GetDocumentPath(invoice.FilePath);
                }
                else
                {
                    Teledeclaration = initData as Teledeclaration;
                    Title = Teledeclaration.Label;
                    ImageSource = GetDocumentPath(Teledeclaration.FilePath);
                }

            }
        }
    }
}
