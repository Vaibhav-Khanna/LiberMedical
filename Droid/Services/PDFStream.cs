using libermedical.Droid.Services;
using libermedical.Services;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(PDFStream))]
namespace libermedical.Droid.Services
{
    public class PDFStream : IPDFStream
    {
        public Stream GetStream(string filename)
        {
            return new FileStream(GetFilePath(filename), FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public void Save(string filename, byte[] data)
        {
            var filepath = GetFilePath(filename);
            if (!System.IO.File.Exists(filepath))
            {
                System.IO.File.Create(filepath);
            }
            Java.IO.FileOutputStream fos = new Java.IO.FileOutputStream(filepath);
            fos.Write(data);
            fos.Close();
        }

        private string GetFilePath(string filename)
        {
            return System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), filename);
        }
    }
}