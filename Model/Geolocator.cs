using CommunityToolkit.Mvvm.ComponentModel;
using Mapsui.Extensions;
using System.ComponentModel;
using System.Threading.Tasks;

namespace MFASeeker;

public partial class Geolocator : ObservableObject
{
    public static IGeolocator Default = new GeolocatorImplementation();
    public Geolocator(double reading = 0, double rotationAngle = 0)
    {
        Reading = reading ; 
        RotationAngle = rotationAngle;
    }

    [ObservableProperty]
    private double reading;
    [ObservableProperty]
    private double rotationAngle;
    [ObservableProperty]
    private bool isActive;

    public Action<double>? OnCompassChangedAction { get; set; }

    public void StartUpdateCompass()
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
                }
                IsActive = Compass.Default.IsMonitoring;
            }
        }
        catch (TaskCanceledException ex) { Console.WriteLine(ex.ToString()); }
    }
    private void OnCompassReadingChanged(object? sender, CompassChangedEventArgs e)
    {
        Reading = e.Reading.HeadingMagneticNorth;
        //RotationAngle = 360 - e.Reading.HeadingMagneticNorth;
        OnCompassChangedAction?.Invoke(Reading);
    }
}
