using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mapsui;
using Mapsui.Layers;
using Mapsui.UI;
using Mapsui.UI.Maui;
using MFASeeker.Model;
using MFASeeker.View;
using Microsoft.VisualBasic;
using RTools_NTS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MFASeeker.ViewModel;

public partial class SearchViewModel : ObservableObject
{
    private readonly MapManager mapManager;
    public SearchViewModel()
    {
        MapControl = new();
        mapManager = new();

        MapControl.Map = mapManager.Map;
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
    private TriState _currentState = TriState.Unchecked;

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
    private async Task UpdateCheckBox()
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

            // Переписать для типов слежки (follow / unfollow)
            case TriState.Follow:
                LocationCheckBoxIsChecked = true;
                CurrentStateText = "Follow";
                await mapManager.EnableCompassModeAsync();
                await mapManager.EnableSpectateModeAsync(cts.Token);
                break;
            case TriState.UnFollow:
                LocationCheckBoxIsChecked = false;
                CurrentStateText = "Unfollow";
                await mapManager.EnableCompassModeAsync();
                /*
                 * ЛОГИКА для отвязки камеры
                 */
                break;
        }
    }

}
