using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Projections;
using Mapsui.Styles;
using Mapsui.UI.Maui;
using MFASeeker.Model;
using MFASeeker.Services;
using MFASeeker.View;
using System.ComponentModel;
using System.Diagnostics;

namespace MFASeeker.ViewModel;

public partial class SearchViewModel : ObservableObject
{
    private static WritableLayer? pointFeatures;
    private static CalloutStyle? _activeCalloutStyle;
    private readonly IPopupService popupService;

    [ObservableProperty]
    private Toilet? newToilet = new();

    [ObservableProperty]
    private MapControl searchMapControl;
    [ObservableProperty]
    private string? currentStateText;

    [ObservableProperty]
    private bool locationCheckBoxIsChecked;

    [ObservableProperty]
    private string currentLocationLabel;

    public IAsyncRelayCommand UpdateCheckBoxCommand { get; }

    public SearchViewModel()
    {
        SearchMapControl = new() { Map = MapManager.CreateMap() };

        SearchMapControl.SingleTap += OnMapTaped; // Тап по карте
        SearchMapControl.LongTap += OnMapLongTaped;
        SearchMapControl.Map.Info += MapOnInfo; // Тап по пину
        SearchMapControl.Map.Navigator.ViewportChanged += OnViewPortChanged; // при перетаскивании карты
        MapManager.LocationUpdated += OnLocationUpdate;

        LocationCheckBoxIsChecked = true;
        ChangeSpectateModeCommand.Execute(null);

        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        var features = await MapPinManager.GetFeaturesLocalAsync();

        pointFeatures = MapPinManager.CreatePointLayer("AllToiletsLayer", true);
        pointFeatures.AddRange(features);
        SearchMapControl.Map.Layers.Add(pointFeatures);
    }

    // Метод для обновления состояния CheckBox
    [RelayCommand]
    public void ChangeSpectateMode()
    {
        if (!LocationCheckBoxIsChecked)
        {
            CurrentStateText = "Unfollow";
            MapManager.ToggleCompassMode();
            MapManager.StopSpectateMode();
        }
        else
        {
            CurrentStateText = "Follow";
            MapManager.ToggleCompassMode();
            MapManager.StartSpectateMode();
        }
    }

    private async void OnMapLongTaped(object? sender, Mapsui.UI.TappedEventArgs e)
    {
        //Попап с полями новой точки
        var popup = new NewPinPopup();
        object? result = await Application.Current.MainPage.ShowPopupAsync(popup);
        if (result is bool isConfirmed && isConfirmed)
        {
            if (sender is not MapControl mapControl) return;
            if (NewToilet == null) return;

            var worldPosition = mapControl.Map.Navigator.Viewport.ScreenToWorld(e.ScreenPosition);
            NewToilet.Location = new()
            {
                Longitude = SphericalMercator.ToLonLat(worldPosition).X,
                Latitude = SphericalMercator.ToLonLat(worldPosition).Y
            };

            pointFeatures?.Add(await MapPinManager.GetFeatureMark(NewToilet));
            pointFeatures?.DataHasChanged();

            // сбрасываю данные туалета VM (но лучше сделать метод .Clear();
            NewToilet = new();
        }
    }
    private static void OnMapTaped(object? sender, Mapsui.UI.TappedEventArgs e)
    {
        if (e.NumOfTaps > 0 && _activeCalloutStyle != null && pointFeatures != null)
        {
            //if (sender is not Mapsui.UI.Maui.MapControl)
            //    return;
            _activeCalloutStyle.Enabled = false;
            pointFeatures.DataHasChanged();
        }
    }
    private static void MapOnInfo(object? sender, MapInfoEventArgs e)
    {
        var calloutStyle = e.MapInfo?.Feature?.Styles.Where(s => s is CalloutStyle)
            .Cast<CalloutStyle>().FirstOrDefault();

        if (calloutStyle != null)
        {
            if (_activeCalloutStyle != null && _activeCalloutStyle != calloutStyle)
            {
                // Деактивируем предыдущую активную метку
                _activeCalloutStyle.Enabled = false;
            }
            // Активируем новую метку
            calloutStyle.Enabled = !calloutStyle.Enabled;
            _activeCalloutStyle = calloutStyle.Enabled ? calloutStyle : null;
            
            e.MapInfo?.Layer?.DataHasChanged(); // Обновляем слой для перерисовки графики
        }
    }
    private static void OnViewPortChanged(object? sender, PropertyChangedEventArgs e)
    {
     // при передвижении вьюпорта логика (сброк отслеживания)
    }
    private async void OnLocationUpdate(Location location)
    {
        if (location != null)
        {
             CurrentLocationLabel = await StandartGeoCodingService.GetAddressFromCoordinates(location.Latitude, location.Longitude);
        }
    }
}