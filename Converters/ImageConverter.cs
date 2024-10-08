using MFASeeker.Model;
using MFASeeker.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFASeeker.Converters
{
    public class ImageConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            try
            {
                if (value is ImageFile imageFile)
                {
                    LocalImageService imageService = new();
                    ImageSource? temp = ImageSource.FromStream(() => imageService.ByteArrayToStream(
                                                                     imageService.StringToByteBase64(imageFile.ByteBase64)));
                    return temp;
                }
                if (value is List<ImageFile> images && images.Count > 0)
                {
                    LocalImageService imageService = new();
                    ImageSource? temp = ImageSource.FromStream(() => imageService.ByteArrayToStream(
                                                                     imageService.StringToByteBase64(images[0].ByteBase64)));
                    return temp;
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ImageSource.FromFile(@"toilet_undefined.png");
            }
            return ImageSource.FromFile(@"toilet_undefined.png");
        }
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
