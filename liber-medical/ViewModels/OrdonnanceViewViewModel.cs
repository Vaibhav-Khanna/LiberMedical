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

		public override void Init(object initData)
		{
			base.Init(initData);
			if (initData != null)
			{
				if (initData is Ordonnance)
				{
					Ordonnance = initData as Ordonnance;
					Title = Ordonnance.PatientName;
					ImageSource = Ordonnance.Attachments != null && Ordonnance.Attachments.Count > 0 ? Ordonnance.Attachments[0] : string.Empty;
				}
                else if (initData is Document)
                {
                    var document = initData as Document;
                    Title = document.Patient.Fullname;
                    ImageSource = document.AttachmentPath;
                }
                else if (initData is string)
                {
                    Title = System.IO.Path.GetFileNameWithoutExtension((string)initData);
                    ImageSource = (string)initData;
                }
                else
				{
					Teledeclaration = initData as Teledeclaration;
					Title = Teledeclaration.Label;
					ImageSource = Teledeclaration.FilePath;
				}

			}
		}
	}
}
