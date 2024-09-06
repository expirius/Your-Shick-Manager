/*
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFASeeker.Model
{
    public class CompassManager_old
    {
        public double CompassHeading;

        public async Task ToggleCompassAsync()
        {
            await Task.Run(() =>
            {
                if (Compass.Default.IsSupported)
                    if (!Compass.Default.IsMonitoring)
                    {
                        Compass.Default.ReadingChanged += Compass_ReadingChanged;
                        Compass.Start(SensorSpeed.UI);
                    }
            });
        }
        private void Compass_ReadingChanged(object? sender, CompassChangedEventArgs e)
        {
            CompassHeading = e.Reading.HeadingMagneticNorth;
        }
    }
}
    */
