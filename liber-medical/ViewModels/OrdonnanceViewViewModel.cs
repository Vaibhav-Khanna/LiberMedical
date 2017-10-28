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
        public OrdonnanceViewViewModel()
        {

        }
        private string GetDocumentPath(string path)
        {
            if (!string.IsNullOrEmpty(path))
                if (path.StartsWith("Ordonnance/") || path.StartsWith("PatientDocuments/"))
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
