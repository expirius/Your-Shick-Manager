

namespace MFASeeker;

public class Geolocator 
{
    public static IGeolocator Default = new GeolocatorImplementation();
}
/*
public async Task StartUpdateLocationAsync()
{
    _cancelTokenSource = new CancellationTokenSource();

    try
    {
            Location = await Geolocation.GetLocationAsync(
                   new GeolocationRequest
                   {
                       DesiredAccuracy = GeolocationAccuracy.Medium,
                       Timeout = TimeSpan.FromSeconds(10),
                       RequestFullAccuracy = true,
                   });
            if (Location != null) 
                OnLocationUpdated(new GeolocationLocationChangedEventArgs(Location));
    }
    catch (TaskCanceledException ex)
    {
        Console.WriteLine(ex.ToString());
    }
}
public void StopUpdatingLocation()
    {
        _cancelTokenSource?.Cancel();
    }

    // del eve
    protected virtual void OnLocationUpdated(GeolocationLocationChangedEventArgs e)
    {
        LocationUpdated?.Invoke(this, e);
    }
*/
