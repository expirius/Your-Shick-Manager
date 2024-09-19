using CommunityToolkit.Mvvm.ComponentModel;
using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Projections;
using Mapsui.Styles;
using Mapsui.UI.Maui;
using MFASeeker.Model;

namespace MFASeeker.ViewModel;

public partial class SearchViewModel : ObservableObject
{
    private enum TriState
    {
        Unchecked,
        Follow,
        UnFollow
    }
    private static GenericCollectionLayer<List<IFeature>>? pointFeatures;
    private static CalloutStyle? _activeCalloutStyle;

    [ObservableProperty]
    private bool locationCheckBoxIsChecked;
    [ObservableProperty]
    private MapControl mapControl;
    [ObservableProperty]
    private bool isEnabledSpectateMode;

    // Свойство для отображения текущего состояния текста
    [ObservableProperty]
    private string? currentStateText;
    // Стандартное состояние для чекбокса
    private TriState _currentState;
    public SearchViewModel()
    {
        MapControl = new() { Map = MapManager.CreateMap() };

        pointFeatures = new()
        {
            Features = PinManager.CreatePointLayer(),
            IsMapInfoLayer = true,
            Name = "AllToiletsLayer"
        };

        MapControl.Map.Layers.Add(pointFeatures);

        MapControl.SingleTap += OnMapTaped; // Тап по карте
        MapControl.LongTap += OnMapLongTaped;
        MapControl.Map.Info += MapOnInfo; // Тап по пину
    }
    // Метод для переключения состояний
    public void ChangeState()
    {
        _currentState = _currentState switch
        {
            TriState.Unchecked => TriState.Follow,
            TriState.Follow => TriState.UnFollow,
            TriState.UnFollow => TriState.Follow,
            //TriState.Compass => TriState.Unchecked,
            _ => TriState.Unchecked
        };
        UpdateCheckBox();
    }
    // Метод для обновления состояния CheckBox
    private async void UpdateCheckBox()
    {
        CancellationTokenSource cts = new();
        switch (_currentState)
        {
            /*case TriState.Unchecked:
            //    LocationCheckBoxIsChecked = false;
            //    CurrentStateText = "Unchecked";
            //    mapManager.StopSpectateMode();
            //    break;
            */
            case TriState.Follow:
                LocationCheckBoxIsChecked = true;
                CurrentStateText = "Follow";
                MapManager.ToggleCompassMode();
                await MapManager.EnableSpectateModeAsync(cts.Token);
                break;
            case TriState.UnFollow:
                LocationCheckBoxIsChecked = false;
                CurrentStateText = "Unfollow";
                MapManager.ToggleCompassMode();
                /*
                 * ЛОГИКА для отвязки камеры
                 */
                break;
        }
    }

    private static void OnMapLongTaped(object? sender, Mapsui.UI.TappedEventArgs e)
    {
        if (e.ScreenPosition != null && pointFeatures != null)
        {
            if (sender is not MapControl mapControl)
                return;
            // Конвертация координат из экрана в географические
            var worldPosition = mapControl.Map.Navigator.Viewport.ScreenToWorld(e.ScreenPosition);

            Location location = new()
            {
                Longitude = SphericalMercator.ToLonLat(worldPosition).X,
                Latitude = SphericalMercator.ToLonLat(worldPosition).Y
            };
            var newFeature = PinManager.AddNewMarkOnLayer(location);
            pointFeatures.Features.Add(newFeature);

            pointFeatures.DataHasChanged();
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
}