using MFASeeker.Model;
using MFASeeker.Services;
using System.Globalization;

namespace MFASeeker.Converters
{
    public class ImageConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            LocalImageService imageService = new();
            if (value is ImageFile imageFile)
            {
                //ImageSource? temp = ImageSource.FromStream(() => imageService.ByteArrayToStream(
                //                                                 imageService.StringToByteBase64(imageFile.ByteBase64)));
                //return temp;
            }
            if (value is List<ImageFile> images && images.Count > 0)
            {
                return images[0].Image;
                //ImageSource? temp = ImageSource.FromStream(() => imageService.ByteArrayToStream(
                //                                                 imageService.StringToByteBase64(images[0].ByteBase64)));
                //return temp;
            }
            return ImageSource.FromFile("Images/toilet_undefined.png");
        }
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
