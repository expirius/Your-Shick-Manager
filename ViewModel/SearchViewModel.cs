using CommunityToolkit.Mvvm.ComponentModel;
using Mapsui.UI.Maui;
using MFASeeker.Model;

namespace MFASeeker.ViewModel;

public partial class SearchViewModel : ObservableObject
{
    private MapManager mapManager;
    private PinManager pinManager;
    public SearchViewModel()
    {
        MapControl = new();
        mapManager = new();
        pinManager = new PinManager();

        if (mapManager.Map != null)
        {
            MapControl.Map = mapManager.Map;
            //MapControl.Map.Info += MapOnInfo;
            MapControl.Map.Layers.Add(pinManager.CreatePointLayer());
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

}
