using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Projections;
using Mapsui.Styles;
using Mapsui.UI;
using Mapsui.UI.Maui;
using MFASeekerApp.Model;
using MFASeekerApp.Services;
using MFASeekerApp.View;
using System.Collections.ObjectModel;
using System.Threading;
namespace MFASeekerApp.ViewModel;

public partial class SearchViewModel : ObservableObject
{
    private static PinManagerViewModel? pinManagerVM;

    private static WritableLayer? pointFeatures;
    private static CalloutStyle? _activeCalloutStyle;

    [ObservableProperty]
    private ToiletViewModel? newToiletVM = new(new());

    [ObservableProperty]
    private MapControl searchMapControl;
    [ObservableProperty]
    private string? currentStateText;

    [ObservableProperty]
    private bool locationCheckBoxIsChecked;

    [ObservableProperty]
    private string? currentLocationLabel;

    private readonly BaseViewModel _userSession;
    public SearchViewModel(PinManagerViewModel pinMngrVM, BaseViewModel userSession)
    {
        _userSession = userSession;
        pinManagerVM = pinMngrVM;

        SearchMapControl = new() { Map = MapManager.CreateMap() };
        pointFeatures = MapPinManager.CreatePointLayer("AllToiletsLayer", true); // создаем layer с туалетами
        SearchMapControl.Map.Layers.Add(pointFeatures); // добавляем layer с точками туалетов

        SearchMapControl.SingleTap += OnMapTaped; // Тап по карте
        SearchMapControl.LongTap += OnMapLongTaped;
        SearchMapControl.Map.Info += MapOnInfo; // Тап по пину
        SearchMapControl.TouchMove += OnTouchMove; // при перетаскивании карты
        MapManager.LocationUpdated += OnLocationUpdate;
        pinManagerVM.ToiletsUpdated += OnToiletsUpdated;

        // это тоже кусок говнокода (как и всё тут). Control есть, а логика из VM? Прикол.
        LocationCheckBoxIsChecked = true;
        ChangeSpectateMode();

        //_ = pinManagerVM.RefreshToiletsCommand.ExecuteAsync(null);  // по сути это связанность кода,
        //избавляемся, решаем через уведомления
    }
    // Метод для обновления состояния CheckBox
    [RelayCommand]
    private void ChangeSpectateMode()
    {
        if (!LocationCheckBoxIsChecked)
        {
            CurrentStateText = "Unfollow";
            MapManager.ToggleCompassMode();
            MapManager.DisableCentredUser();
        }
        else
        {
            CurrentStateText = "Follow";
            MapManager.ToggleCompassMode();
            MapManager.EnableCentredUser();
            MapManager.CenterToUserLocation();
        }
    }
    private async void OnMapLongTaped(object? sender, Mapsui.UI.TappedEventArgs e)
    {
        if (sender is not MapControl mapControl) return;
        {
            // Блокирую управление картой и центрирование на метку пользователя
            MapManager.DisableCentredUser();
            mapControl.IsEnabled = false;

            //Попап с полями новой точки
            var popup = new NewPinPopup
            {
                BindingContext = NewToiletVM,
            };
            object? result = await Application.Current.MainPage.ShowPopupAsync(popup);
            if (result is bool isConfirmed && isConfirmed)
            {
                if (NewToiletVM == null || NewToiletVM.Toilet == null) return;
                var worldPosition = mapControl.Map.Navigator.Viewport.ScreenToWorld(e.ScreenPosition);
                NewToiletVM.Toilet.Location = // lat lon
                    $"{SphericalMercator.ToLonLat(worldPosition).Y}," +
                    $"{SphericalMercator.ToLonLat(worldPosition).X}";

                //    ____    _    __  _  ___   _  ___  
                //   | __ )  / \   \ \| |/ / | | |/ _ \ 
                //   |  _ \ / _ \   \ \ / /| |_| | | | |
                //   | |_) / ___ \  / / \ \|  _  | |_| |
                //   |____/_/   \_\/_/| |\_\_| |_|\___/           
                // А ПО ХОРОШЕМУ СДЕЛАТЬ ЧЕРЕЗ EVENTS в pinManager,
                // дабы избежать связанности кода ( сейчас мне лень )

                // Отдаем в pinManager
                pinManagerVM?.AddToiletCommand.Execute(NewToiletVM);
                // сбрасываю данные туалета VM (но лучше сделать метод .Clear();
                NewToiletVM = new(new());
            }
            // Возобновляем центрирование и управление картой
            mapControl.IsEnabled = true;
            if (LocationCheckBoxIsChecked)
                MapManager.EnableCentredUser();
        }
    }
    private void OnMapTaped(object? sender, Mapsui.UI.TappedEventArgs e)
    {
        if (e.NumOfTaps > 0 && _activeCalloutStyle != null && pointFeatures != null)
        {
            //if (sender is not Mapsui.UI.Maui.MapControl)
            //    return;
            _activeCalloutStyle.Enabled = false;
            pointFeatures.DataHasChanged();
        }
    }
    private void MapOnInfo(object? sender, MapInfoEventArgs e)
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
    private void OnTouchMove(object? sender, TouchedEventArgs e)
    {
        // при передвижении вьюпорта логика (сброс отслеживания)
        LocationCheckBoxIsChecked = false;
    }

    // Обновление меток
    // МЕТКИ ДОЛЖНЫ БУДУТ ОБНОВЛЯТЬСЯ УЖЕ НЕ ИЗ ПАМЯТИ
    // А ИЗ PINMANAGER'A. Например: RefreshToiletsCommand()
    public void OnToiletsUpdated(ObservableCollection<ToiletViewModel> toiletsVM)
    {
        Task.Run(() =>
        {
            var toiletsTemp = toiletsVM.Select(toilet => toilet.Toilet); // преобразую в enum обычного toilet
            pointFeatures?.Clear(); // очищаю точки на карте
            var tmp = MapPinManager.GetFeatures(toiletsTemp); // получаю отрисовки точек по всей карте
            pointFeatures?.AddRange(tmp); // добавляю их на карту
            pointFeatures?.DataHasChanged(); // уведомляю об изменении
        });
    }
    private async void OnLocationUpdate(Location location)
    {
        if (location != null)
        {
             CurrentLocationLabel = await StandartGeoCodingService.GetAddressFromCoordinates(location.Latitude, location.Longitude);
        }
    }

}