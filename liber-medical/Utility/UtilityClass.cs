using System;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace libermedical.Utility
{
    public static class UtilityClass
    {
        public static async Task<MediaFile> TakePhoto()
        {

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions()
            {
                AllowCropping = true
            });
            return file;

        }

        public static bool CameraAvailable()
        {

            bool result = false;

            if (CrossMedia.Current.IsCameraAvailable || CrossMedia.Current.IsTakePhotoSupported)
            {
                result = true;
            }
            return result;

        }

    }
}
