using CommunityToolkit.Mvvm.ComponentModel;
using Mapsui;
using Mapsui.Layers;
using Mapsui.Styles;
using Mapsui.UI;
using Mapsui.UI.Maui;
using MFASeeker.Model;
using Microsoft.Maui.ApplicationModel;

namespace MFASeeker.ViewModel;

public partial class SearchViewModel : ObservableObject
{
    private MapManager mapManager;
    private PinManager pinManager;
    private static MemoryLayer? pointLayer;
    private static CalloutStyle? _activeCalloutStyle;
    public SearchViewModel()
    {
        MapControl = new();
        mapManager = new();
        pinManager = new PinManager();

        if (mapManager.Map != null)
        {
            MapControl.Map = mapManager.Map;

            pointLayer = pinManager.CreatePointLayer();
            MapControl.Map.Layers.Add(pointLayer);

            MapControl.SingleTap += OnMapTaped;
            MapControl.Map.Info += MapOnInfo; // Привязка на событие клика
        }
    }
    public enum TriState
    {
        Unchecked,
        Follow,
        UnFollow
    }

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
                mapManager.EnableCompassMode();
                await mapManager.EnableSpectateModeAsync(cts.Token);
                break;
            case TriState.UnFollow:
                LocationCheckBoxIsChecked = false;
                CurrentStateText = "Unfollow";
                mapManager.EnableCompassMode();
                /*
                 * ЛОГИКА для отвязки камеры
                 */
                break;
        }
    }

    private static void OnMapTaped(object? sender, Mapsui.UI.TappedEventArgs e)
    {
        if (e.NumOfTaps > 0)
        {
            var styles = pointLayer?.Features?.SelectMany(feature => feature.Styles);
            if (styles == null) return;
            foreach (var style in styles) style.Enabled = false;
            pointLayer?.DataHasChanged();
        }
    }

    private static void MapOnInfo(object? sender, MapInfoEventArgs e)
    {
        var calloutStyle = e.MapInfo?.Feature?.Styles.Where(s => s is CalloutStyle).Cast<CalloutStyle>().FirstOrDefault();

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