﻿using System;
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
            try
            {
                var placemarks = await Geocoding.GetPlacemarksAsync(latitude, longitude);

                if (placemarks != null && placemarks.Any())
                {
                    var placemark = placemarks.FirstOrDefault();
                    if (placemark != null)
                    {
                        return
                            $"{placemark.Locality}, " +  // Город
                            $"{placemark.Thoroughfare} " +  // Адрес
                            $"{placemark.SubThoroughfare}"; // Дом
                    }
                }
            }
            catch (ObjectDisposedException ex)
            {
                // Обработка ошибки если объект очищен
                Console.WriteLine($"ObjectDisposedException: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }

            return "N/A";
        }
    }
}