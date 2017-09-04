using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace libermedical.Utility
{
    public static class UtilityClass
    {
        public static async Task<MediaFile> TakePhoto()
        {
            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                AllowCropping = true
            });
            return file;
        }

        public static bool CameraAvailable()
        {
            return CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported;
        }
    }
}
