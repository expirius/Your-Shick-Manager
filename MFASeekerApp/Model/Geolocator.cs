using CommunityToolkit.Mvvm.ComponentModel;
using Mapsui.Extensions;
using System.ComponentModel;
using System.Threading.Tasks;

namespace MFASeeker;

public static class Geolocator
{
    public static IGeolocator Default = new GeolocatorImplementation();

    private static double reading;

    public static Action<double>? OnCompassChangedAction { get; set; }


    public static void ToggleCompass()
    {
        try
        {
            if (Compass.Default.IsSupported)
            {
                if (!Compass.Default.IsMonitoring)
                {
                    Compass.Default.ReadingChanged += OnCompassReadingChanged;
                    Compass.Default.Start(SensorSpeed.Fastest);
                }
                else
                {
                    Compass.Default.Stop();
                    Compass.Default.ReadingChanged -= OnCompassReadingChanged;

                    OnCompassReadingChanged(null, null);
                }
            }
        }
        catch (TaskCanceledException ex) { Console.WriteLine(ex.ToString()); }
    }
    private static void OnCompassReadingChanged(object? sender, CompassChangedEventArgs? e)
    {
        if (e == null)
            reading = -1;
        else
            reading = e.Reading.HeadingMagneticNorth;
        OnCompassChangedAction?.Invoke(reading);
    }
}
