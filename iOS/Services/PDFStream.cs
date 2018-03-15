using libermedical.iOS.Services;
using libermedical.Services;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(PDFStream))]
namespace libermedical.iOS.Services
{
    public class PDFStream : IPDFStream
    {
        public Stream GetStream(string path)
        {
            return new FileStream(GetFilePath(path), FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public void Save(string filename, byte[] data)
        {
            System.IO.File.WriteAllBytes(GetFilePath(filename), data);
        }

        private string GetFilePath(string filename)
        {
            return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), filename);
        }
    }
}
