using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Foundation;
using libermedical.iOS.Services;
using libermedical.Services;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(ShareFile))]
namespace libermedical.iOS.Services
{
    public class ShareFile : IShare
    {
        public async Task ShareLocalFile(string localFilePath, string title = "", object view = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(localFilePath))
                {
                    Console.WriteLine("Plugin.ShareFile: ShareLocalFile Warning: localFilePath null or empty");
                    return;
                }

                var items = new NSObject[] { NSObject.FromObject(title), NSUrl.FromFilename(localFilePath) };
                var activityController = new UIActivityViewController(items, null);
                var vc = GetVisibleViewController();

                NSString[] excludedActivityTypes = null;

                if (excludedActivityTypes != null && excludedActivityTypes.Length > 0)
                    activityController.ExcludedActivityTypes = excludedActivityTypes;

                if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
                {
                    if (activityController.PopoverPresentationController != null)
                    {
                        activityController.PopoverPresentationController.SourceView = vc.View;
                    }
                }

                await vc.PresentViewControllerAsync(activityController, true);
            }
            catch (Exception ex)
            {
                if (ex != null && !string.IsNullOrWhiteSpace(ex.Message))
                    Console.WriteLine("Exception in Plugin.ShareFile: ShareLocalFile Exception: {0}", ex);
            }
        }

        /// <summary>
        /// Share a file from a remote resource on compatible services
        /// </summary>
        /// <param name="fileUri">uri to external file</param>
        /// <param name="fileName">name of the file</param>
        /// <param name="title">Title of popup on share (not included in message)</param>
        /// <returns>awaitable bool</returns>
        public async Task ShareRemoteFile(string fileUri, string fileName, string title = "", object view = null)
        {
            try
            {
                var webClient = new WebClient();

                var uri = new System.Uri(fileUri);
                var bytes = await webClient.DownloadDataTaskAsync(uri);

                var filePath = WriteFile(fileName, bytes);
               
                await ShareLocalFile(filePath, title, view);
            }
            catch (Exception ex)
            {
                if (ex != null && !string.IsNullOrWhiteSpace(ex.Message))
                    Console.WriteLine("Exception in Plugin.ShareFile: ShareRemoteFile Exception: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Writes the file to local storage.
        /// </summary>
        /// <returns>The file.</returns>
        /// <param name="fileName">File name.</param>
        /// <param name="bytes">Bytes.</param>
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

        UIViewController GetVisibleViewController()
        {
            UIViewController viewController = null;
            var window = UIApplication.SharedApplication.KeyWindow;


            if (window != null && window.WindowLevel == UIWindowLevel.Normal)
                viewController = window.RootViewController;

            if (viewController == null)
            {
                window = UIApplication.SharedApplication.Windows.OrderByDescending(w => w.WindowLevel).FirstOrDefault(w => w.RootViewController != null && w.WindowLevel == UIWindowLevel.Normal);
                if (window == null)
                    throw new InvalidOperationException("Could not find current view controller");
                else
                    viewController = window.RootViewController;
            }

            while (viewController.PresentedViewController != null)
                viewController = viewController.PresentedViewController;


            return viewController;
        }

        public async void ShareFileBytes(byte[] fileData, string fileName, string title = "", object view = null)
        {
            try
            {
                var filePath = WriteFile(fileName, fileData);
                await ShareLocalFile(filePath, title, view);
            }
            catch (Exception ex)
            {
                if (ex != null && !string.IsNullOrWhiteSpace(ex.Message))
                    Console.WriteLine("Exception in Plugin.ShareFile: ShareRemoteFile Exception: {0}", ex.Message);
            }
        }
    }
}
