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
            if (value is ImageFile imageFile)
            {
                LocalImageService imageService = new();
                ImageSource? temp = ImageSource.FromStream(() => imageService.ByteArrayToStream(
                                                                 imageService.StringToByteBase64(imageFile.ByteBase64)));
                return temp;
            }
            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
