using MFASeeker.Services;
using System.Globalization;

namespace MFASeeker.Converters
{
    public class GeoCodingConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return Task.Run(async () =>
            {
                if (value is (Location location))
                {
                    return await StandartGeoCodingService.GetAddressFromCoordinates(location.Latitude, location.Longitude);
                }
                return "N/A";
            }).GetAwaiter().GetResult(); // 
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
