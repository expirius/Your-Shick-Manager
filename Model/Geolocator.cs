using CommunityToolkit.Mvvm.ComponentModel;
using Mapsui.Extensions;
using System.ComponentModel;
using System.Threading.Tasks;

namespace MFASeeker;

public partial class Geolocator : ObservableObject
{
    public static IGeolocator Default = new GeolocatorImplementation();
    CancellationTokenSource _cancelTokenSource = new CancellationTokenSource();

    public Geolocator(double reading = 100, double rotationAngle = 0)
    {
        Reading = reading ; 
        RotationAngle = rotationAngle;
    }

    [ObservableProperty]
    private double reading;
    [ObservableProperty]
    private double rotationAngle;

    public static Action<Geolocator>? OnCompassChangedAction { get; set; }

    private void OnCompassReadingChanged(object sender, CompassChangedEventArgs e)
    {
        Reading = e.Reading.HeadingMagneticNorth;
        RotationAngle = 360 - e.Reading.HeadingMagneticNorth;
    }

    public async Task StartUpdateCompassAsync()
    {
        try
        {
            if (Compass.Default.IsSupported)
            {
                if (!Compass.Default.IsMonitoring)
                {
                    Compass.Default.ReadingChanged += OnCompassReadingChanged;
                    Compass.Default.Start(SensorSpeed.UI);
                }
                else
                {
                    Compass.Default.Stop();
                    Compass.Default.ReadingChanged -= OnCompassReadingChanged;
                }
            }
        }
        catch (TaskCanceledException ex) { Console.WriteLine(ex.ToString()); }
    }
    public void StopUpdatingLocation()
    {
        _cancelTokenSource?.Cancel();
    }

}
