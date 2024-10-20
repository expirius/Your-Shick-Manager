using MFASeeker.Model;
using MFASeeker.Services;
using MFASeeker.ViewModel;
using System.Collections.ObjectModel;
using System.Globalization;

namespace MFASeeker.Converters
{
    public class ImageConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        { /*
            if (value is string byteBase64)
            {
                //return System.Convert.FromBase64String(byteBase64);
            }
            if (value is ImageFileViewModel imageFile)
            {
                //return imageFile?.ImageSource;
                //ImageSource? temp = ImageSource.FromStream(() => imageService.ByteArrayToStream(
                //                                                 imageService.StringToByteBase64(imageFile.ByteBase64)));
                //return temp;
            }
            if (value is ObservableCollection<ImageFile> images && images.Count > 0)
            {
                //return images[0]?.Image;
                //ImageSource? temp = ImageSource.FromStream(() => imageService.ByteArrayToStream(
                //                                                 imageService.StringToByteBase64(images[0].ByteBase64)));
                //return temp;
            } */
            return ImageSource.FromFile("Images/toilet_undefined.png");
        }
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
