using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mapsui;
using Mapsui.Layers;
using Mapsui.UI;
using Mapsui.UI.Maui;
using MFASeeker.Model;
using MFASeeker.View;
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

    [ObservableProperty]
    private MapControl mapControl;

    [ObservableProperty]
    private bool isEnabledSpectateMode;


    // Обновление метки пользователя
    [RelayCommand(IncludeCancelCommand = true, AllowConcurrentExecutions = false)]
    private async Task EnableSpectateMode(CancellationToken cancellationToken)
    {
        if (mapManager != null)
            await mapManager.EnableSpectateModeAsync(cancellationToken);
    }
}
