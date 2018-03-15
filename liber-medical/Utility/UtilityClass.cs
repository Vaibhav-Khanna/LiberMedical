using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace libermedical.Utility
{
    public static class UtilityClass
    {
        public static async Task<MediaFile> TakePhoto()
        {
            if (await App.AskForCameraPermission())
            {
                await CrossMedia.Current.Initialize();
                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    AllowCropping = true, SaveToAlbum = false
                });
                return file;
            }
            else
                return null;
            
        }

        public static bool CameraAvailable()
        {
            return CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported;
        }
    }
}
