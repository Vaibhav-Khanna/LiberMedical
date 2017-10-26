using libermedical.Helpers;
using libermedical.Services;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace libermedical.Managers
{
    
    public static class FileUpload
    {

        public static async Task UploadFile(string filepath, string type, string id)
        {
            try
            {
                
                var bytedata = ReadFully(DependencyService.Get<IPDFStream>().GetStream(filepath));
                 
                HttpClient client = new HttpClient();
                MultipartFormDataContent content = new MultipartFormDataContent();
                ByteArrayContent baContent = new ByteArrayContent(bytedata);

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Settings.Token);
                client.DefaultRequestHeaders.Add("path",$"{type}/{id}/{Path.GetFileName(filepath)}");
                content.Add(baContent, "file", $"{Path.GetFileName(filepath)}");

                var response = await client.PostAsync(Constants.RestUrl + "upload", content);

                var result = await response.Content.ReadAsStringAsync();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }


        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];

            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }


    }


}
