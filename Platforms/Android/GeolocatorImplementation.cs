using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using MFASeeker;
using MFASeeker.Model;
using Location = Android.Locations.Location;
using Permissions = Microsoft.Maui.ApplicationModel.Permissions;

namespace MFASeeker;

public class GeolocatorImplementation : IGeolocator
{
    GeolocationContinuousListener? locator;

    public async Task StartListening(IProgress<Microsoft.Maui.Devices.Sensors.Location> positionChangedProgress, CancellationToken cancellationToken)
    {
        // ПРОВЕРКА ПРАВ!
        var permission = await Permissions.CheckStatusAsync<Permissions.LocationAlways>();
        if (permission != PermissionStatus.Granted)
        {
            permission = await Permissions.RequestAsync<Permissions.LocationAlways>();
            if (permission != PermissionStatus.Granted)
            {
                //await Toast.Make("No permission").Show(CancellationToken.None);
                return;
            }
        }

        locator = new GeolocationContinuousListener();
        var taskCompletionSource = new TaskCompletionSource();
        cancellationToken.Register(() =>
        {
            locator.Dispose();
            locator = null;
            taskCompletionSource.TrySetResult();
        });
        locator.OnLocationChangedAction = location =>
            positionChangedProgress.Report(
                new Microsoft.Maui.Devices.Sensors.Location(location.Latitude, location.Longitude));
        await taskCompletionSource.Task;
    }
    public void StopListening()
    {
        if (locator != null)
        {
            locator.Dispose();
            locator = null;
        }
    }
}

internal class GeolocationContinuousListener : Java.Lang.Object, ILocationListener
{
    public Action<Location>? OnLocationChangedAction { get; set; }

    LocationManager? locationManager;
    public GeolocationContinuousListener()
    {
        locationManager = (LocationManager?)Android.App.Application.Context.GetSystemService(Android.Content.Context.LocationService);

        var isInternetAvailable = Connectivity.NetworkAccess == NetworkAccess.Internet;
        // Requests location updates each second and notify if location changes more then 100 meters
        if (isInternetAvailable)
        {
            // Используем сетевой провайдер при наличии интернета
            locationManager?.RequestLocationUpdates(LocationManager.NetworkProvider, 100, 1, this);
        }
        else
        {
            // Используем GPS провайдер при отсутствии интернета
            locationManager?.RequestLocationUpdates(LocationManager.GpsProvider, 100, 1, this);
        }
    }

    public void OnLocationChanged(Location location)
    {
        OnLocationChangedAction?.Invoke(location);
    }

    public void OnProviderDisabled(string provider)
    {
    }

    public void OnProviderEnabled(string provider)
    {
    }

    public void OnStatusChanged(string? provider, [GeneratedEnum] Availability status, Bundle? extras)
    {
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        locationManager?.RemoveUpdates(this);
        locationManager?.Dispose();
    }
}