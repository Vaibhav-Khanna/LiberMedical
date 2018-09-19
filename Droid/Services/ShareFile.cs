using System;
using System.Net;
using System.Threading.Tasks;
using Android.Content;
using libermedical.Droid.Services;
using libermedical.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(ShareFile))]
namespace libermedical.Droid.Services
{
    public class ShareFile : IShare
    {
        private readonly Context _context;
        public ShareFile()
        {
            _context = Android.App.Application.Context;
        }

        public void ShareFileBytes(byte[] fileData, string fileName, string title = "", object view = null)
        {
            var filePath = WriteFile(fileName, fileData);

            Show(title, "", filePath);
        }

        public async Task ShareLocalFile(string localFilePath, string title = "", object view = null)
        {
            await Task.Delay(100);
            Show(title, "", localFilePath);
        }

        public async Task ShareRemoteFile(string fileUri, string fileName, string title = "", object view = null)
        {
            try
            {
                var webClient = new WebClient();

                var uri = new System.Uri(fileUri);
                var bytes = await webClient.DownloadDataTaskAsync(uri);

                var filePath = WriteFile(fileName, bytes);

                Show(title, "", filePath);
            }
            catch (Exception ex)
            {
                if (ex != null && !string.IsNullOrWhiteSpace(ex.Message))
                    Console.WriteLine("Exception in Plugin.ShareFile: ShareRemoteFile Exception: {0}", ex.Message);
            }
        }

        private string WriteFile(string fileName, byte[] bytes)
        {
            string localPath = "";

            try
            {
                string localFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
                localPath = System.IO.Path.Combine(localFolder, fileName);
                System.IO.File.WriteAllBytes(localPath, bytes); // write to local storage
            }
            catch (Exception ex)
            {
                if (ex != null && !string.IsNullOrWhiteSpace(ex.Message))
                    Console.WriteLine("Exception in Plugin.ShareFile: ShareRemoteFile Exception: {0}", ex);
            }

            return localPath;
        }

        public void Show(string title, string message, string filePath)
        {
            var extension = filePath.Substring(title.LastIndexOf(".", StringComparison.Ordinal) + 1).ToLower();
            var contentType = string.Empty;

            // You can manually map more ContentTypes here if you want.
            switch (extension)
            {
                case "pdf":
                    contentType = "application/pdf";
                    break;
                case "png":
                    contentType = "image/png";
                    break;
                default:
                    contentType = "application/octetstream";
                    break;
            }

            var intent = new Intent(Intent.ActionSend);
            intent.SetType(contentType);
            intent.PutExtra(Intent.ExtraStream, Android.Net.Uri.Parse("file://" + filePath));
            intent.PutExtra(Intent.ExtraText, string.Empty);
            intent.PutExtra(Intent.ExtraSubject, message ?? string.Empty);

            var chooserIntent = Intent.CreateChooser(intent, title ?? string.Empty);
            chooserIntent.SetFlags(ActivityFlags.ClearTop);
            chooserIntent.SetFlags(ActivityFlags.NewTask);
            _context.StartActivity(chooserIntent);
        }
    }
}
