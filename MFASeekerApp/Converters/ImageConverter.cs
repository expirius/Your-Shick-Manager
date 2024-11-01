using MFASeekerApp.Services;
using System.Collections.ObjectModel;
using System.Globalization;
using MFASeekerApp.Model;


namespace MFASeekerApp.Converters
{
    public class ImageConverter : IValueConverter
    {
        private LocalImageService imageService = new();
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        { 
            if (value is string byteBase64)
            {
                return System.Convert.FromBase64String(byteBase64);
            } 
            if (value is ImageFile imageFile)
            {
                //imageFile?.LoadImageCommand.Execute(null);
                //return imageFile?.ImageSource;
                ImageSource? temp = ImageSource.FromStream(() => imageService.ByteArrayToStream(
                                                                 imageService.StringToByteBase64(imageFile.ByteBase64)));
                return temp;
            }
            
            if (value is ObservableCollection<ImageFile> images && images.Count > 0)
            {
                //return images[0]?.Image;
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
