using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MFASeeker.ViewModel
{
    public partial class LocationVM : ObservableObject
    {
        [ObservableProperty]
        private bool isTrackingEnabled;

        [ObservableProperty]
        private double currentHeading;

        /*
        public LocationTrackingViewModel()
        {
            ToggleTrackingCommand = new Command(ToggleTracking);
            IsTrackingEnabled = false;
            Compass.ReadingChanged += OnCompassReadingChanged;
        }
        */
        private async void ToggleTracking()
        {
            IsTrackingEnabled = !IsTrackingEnabled;
            if (IsTrackingEnabled)
            {
                //await StartTrackingAsync();
            }
            else
            {
                StopTracking();
            }
        }

        /*
        private async Task StartTrackingAsync()
        {
            if (await CheckAndRequestPermissionsAsync())
            {
                // Начать отслеживание местоположения
                await Geolocation.StartListeningForegroundAsync(TimeSpan.FromSeconds(1), 10, true);

                // Начать отслеживание направления
                if (!Compass.IsMonitoring)
                {
                    Compass.Start(SensorSpeed.UI);
                }
            }
            else
            {
                IsTrackingEnabled = false;
            }
        }
        */

        private void StopTracking()
        {
            // Остановить отслеживание местоположения
            //Geolocation.StopListeningAsync();

            // Остановить отслеживание направления
            if (Compass.IsMonitoring)
            {
                Compass.Stop();
            }
        }

        private void OnCompassReadingChanged(object sender, CompassChangedEventArgs e)
        {
            CurrentHeading = e.Reading.HeadingMagneticNorth;
        }

        private async Task<bool> CheckAndRequestPermissionsAsync()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }
            return status == PermissionStatus.Granted;
        }
    }
} 
