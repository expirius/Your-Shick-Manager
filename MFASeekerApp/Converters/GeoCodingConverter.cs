using MFASeekerApp.Model;
using MFASeekerApp.Services;
using System.Globalization;

namespace MFASeekerApp.Converters
{
    public class GeoCodingConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return Task.Run(async() =>
            {
                if (value is string location)
                {
                    Location loc = Toilet.ParseLocationFromString(location);

                    string adress = await StandartGeoCodingService.GetAddressFromCoordinates(loc.Latitude, loc.Longitude);
                    return adress;
                }
                if (value is Location locationT)
                {
                    return await StandartGeoCodingService.GetAddressFromCoordinates(locationT.Latitude, locationT.Longitude);
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
