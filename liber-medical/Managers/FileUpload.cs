using libermedical.Helpers;
using libermedical.Services;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace libermedical.Managers
{
    public static class FileUpload
    {
        private static string _auth => string.Concat("token=", Settings.Token);
        public static async Task UploadFile(string filepath, string type, string id)
        {
            try
            {
                var client = new HttpClient();
                var content = new MultipartFormDataContent();
                var url = new Uri(string.Format(Constants.RestUrl + "upload?" + _auth, string.Empty));
                content.Add(new StreamContent(DependencyService.Get<IPDFStream>().GetStream(filepath)), type, $"{ type}/{id}/{Path.GetFileName(filepath)}");
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/Octet");
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }


}
