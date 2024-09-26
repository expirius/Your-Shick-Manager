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
using MFASeeker.View;
using System.ComponentModel;

namespace MFASeeker.ViewModel;

public partial class SearchViewModel : ObservableObject
{
    private enum TriState
    {
        Unchecked,
        Follow,
        UnFollow
    }

    private static WritableLayer? pointFeatures;
    private static WritableLayer? _tempPointFeatures;
    private static CalloutStyle? _activeCalloutStyle;
    private readonly IPopupService popupService;

    [ObservableProperty]
    private Toilet? newToilet; 

    [ObservableProperty]
    private static bool locationCheckBoxIsChecked;
    [ObservableProperty]
    private MapControl mapControl;
    [ObservableProperty]
    private static bool isEnabledSpectateMode;
    [ObservableProperty]
    private string? currentStateText;
    // Стандартное состояние для чекбокса
    private TriState _currentState;

    public SearchViewModel()
    {
        MapControl = new() { Map = MapManager.CreateMap() };
        // Слой с точками туалетов
        pointFeatures = MapPinManager.CreatePointLayer("AllToiletsLayer", true);
        // Загрузка точек на слой из ... чего либо
        pointFeatures.AddRange(MapPinManager.GetFeaturesLocal());

        MapControl.Map.Layers.Add(pointFeatures);
        NewToilet = new();

        MapControl.SingleTap += OnMapTaped; // Тап по карте
        MapControl.LongTap += OnMapLongTaped;
        MapControl.Map.Info += MapOnInfo; // Тап по пину
        MapControl.Map.Navigator.ViewportChanged += OnViewPortChanged; // при перетаскивании карты
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
                await MapManager.EnableSpectateModeAsync();
                break;
            case TriState.UnFollow:
                LocationCheckBoxIsChecked = false;
                CurrentStateText = "Unfollow";

                MapManager.ToggleCompassMode();
                MapManager.StopLocationUpdates();
                /*
                 * ЛОГИКА для отвязки камеры
                 */
                break;
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
          
            pointFeatures?.Add(MapPinManager.GetFeatureMark(NewToilet));
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
}