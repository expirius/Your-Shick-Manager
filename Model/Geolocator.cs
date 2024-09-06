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
            
        }
        catch (TaskCanceledException ex) { Console.WriteLine(ex.ToString()); }
    }
    public void StopUpdatingLocation()
    {
        _cancelTokenSource?.Cancel();
    }

}
