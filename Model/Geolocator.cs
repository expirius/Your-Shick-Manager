namespace MFASeeker;

public class Geolocator
{
    public static IGeolocator Default = new GeolocatorImplementation();
    CancellationTokenSource _cancelTokenSource = new CancellationTokenSource();
    Location Location = new();

    public async Task StartUpdateLocationAsync()
    {
        try
        {
            Location? lc = await Geolocation.GetLocationAsync(
                   new GeolocationRequest
                   {
                       DesiredAccuracy = GeolocationAccuracy.Medium,
                       Timeout = TimeSpan.FromSeconds(10),
                       RequestFullAccuracy = true,
                   });
            if (lc != null)
                OnLocationUpdated(new GeolocationLocationChangedEventArgs(lc));
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
        Location = e.Location;
    }
}
