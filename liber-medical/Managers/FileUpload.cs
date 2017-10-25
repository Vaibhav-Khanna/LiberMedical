using libermedical.Helpers;
using libermedical.Services;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using Xamarin.Forms;

namespace libermedical.Managers
{
    public static class FileUpload
    {
        private static string _auth => string.Concat("token=", Settings.Token);
        public static async void UploadFile(string filepath, string type,string id)
        {
            try
            {
                var client = new HttpClient();
                var requestContent = new MultipartFormDataContent();
                var streamContent = new StreamContent(DependencyService.Get<IPDFStream>().GetStream(filepath));
                requestContent.Add(streamContent, "image", $"{ type}/{id}/{Path.GetFileName(filepath)}");
                var url = new Uri(string.Format(Constants.RestUrl + "/upload?" + _auth, string.Empty));
                var response =  await client.PostAsync(url, requestContent);

            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }


}
