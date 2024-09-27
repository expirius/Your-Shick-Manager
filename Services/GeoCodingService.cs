using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MFASeeker.Services
{
    public static class StandartGeoCodingService
    {
        public static async Task<string> GetAddressFromCoordinates(double latitude, double longitude)
        {
            var placemarks = await Geocoding.GetPlacemarksAsync(latitude, longitude);
            var placemark = placemarks?.FirstOrDefault();
            if (placemark != null)
            {
                return
                    $"{placemark.Locality}, \n" + // Город
                    $"{placemark.Thoroughfare} " + // Адраес
                    $"{placemark.SubThoroughfare}"; // Дом
            }
            return "";
        }
    }
}
