using CommunityToolkit.Mvvm.ComponentModel;
using Mapsui.Extensions;
using System.ComponentModel;
using System.Threading.Tasks;

namespace MFASeeker;

public partial class Geolocator : ObservableObject
{
    public static IGeolocator Default = new GeolocatorImplementation();
    CancellationTokenSource _cancelTokenSource = new CancellationTokenSource();
    public Geolocator(double reading = 0, double rotationAngle = 0)
    {
        Reading = reading ; 
        RotationAngle = rotationAngle;
    }

    [ObservableProperty]
    private double reading;
    [ObservableProperty]
    private double rotationAngle;

    public Action<double>? OnCompassChangedAction { get; set; }

    public async Task StartUpdateCompassAsync()
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
            }
        }
        catch (TaskCanceledException ex) { Console.WriteLine(ex.ToString()); }
    }
    public void StopUpdatingCompass()
    {
        //_cancelTokenSource?.Cancel();
        Compass.Stop();
    }
    private void OnCompassReadingChanged(object sender, CompassChangedEventArgs e)
    {
        Reading = e.Reading.HeadingMagneticNorth;
        //RotationAngle = 360 - e.Reading.HeadingMagneticNorth;
        OnCompassChangedAction?.Invoke(Reading);
    }
}
