using CommunityToolkit.Maui.Core;
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
using MFASeeker.Model;
using MFASeeker.Services;
using MFASeeker.View;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace MFASeeker.ViewModel;

public partial class SearchViewModel : ObservableObject
{
    private static PinManagerViewModel? pinManagerVM;

    private static WritableLayer? pointFeatures;
    private static CalloutStyle? _activeCalloutStyle;

    [ObservableProperty]
    private Toilet? newToilet = new();

    [ObservableProperty]
    private MapControl searchMapControl;
    [ObservableProperty]
    private string? currentStateText;

    [ObservableProperty]
    private bool locationCheckBoxIsChecked;

    [ObservableProperty]
    private string? currentLocationLabel;

    public SearchViewModel(PinManagerViewModel pinMngrVM)
    {
        SearchMapControl = new() { Map = MapManager.CreateMap() };

        SearchMapControl.SingleTap += OnMapTaped; // Тап по карте
        SearchMapControl.LongTap += OnMapLongTaped;
        SearchMapControl.Map.Info += MapOnInfo; // Тап по пину
        SearchMapControl.TouchMove += OnTouchMove; // при перетаскивании карты
        MapManager.LocationUpdated += OnLocationUpdate;

        pinManagerVM = pinMngrVM;
        pinManagerVM.ToiletsUpdated += OnToiletsUpdated;

        LocationCheckBoxIsChecked = true;
        ChangeSpectateModeCommand.Execute(null);

        InitializeAsync();
    }
    [RelayCommand]
    private async Task AddImage()
    {
        var localImageService = new LocalImageService();
        var fileResult = await localImageService.TakePhoto();

        if (fileResult != null)
        {
            ImageFile? imageFile = await localImageService.Upload(fileResult);
            if (imageFile != null)
            {
                NewToilet?.Images.Add(imageFile);
                Console.WriteLine("Image added: " + imageFile.ByteBase64);
            }
        }
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
    private async void InitializeAsync()
    {
        //var features = await MapPinManager.GetFeaturesLocalAsync();
        // pointFeatures.AddRange(features);
        pointFeatures = MapPinManager.CreatePointLayer("AllToiletsLayer", true);
        SearchMapControl.Map.Layers.Add(pointFeatures);
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
                BindingContext = this
            };
            object? result = await Application.Current.MainPage.ShowPopupAsync(popup);
            if (result is bool isConfirmed && isConfirmed)
            {
                if (NewToilet == null) return;

                var worldPosition = mapControl.Map.Navigator.Viewport.ScreenToWorld(e.ScreenPosition);
                NewToilet.Location = new()
                {
                    Longitude = SphericalMercator.ToLonLat(worldPosition).X,
                    Latitude = SphericalMercator.ToLonLat(worldPosition).Y
                };

                // Добавляем метку на карту
                //pointFeatures?.Add(MapPinManager.GetFeature(NewToilet));
                //pointFeatures?.DataHasChanged();

                // Отдаем в pinManager
                pinManagerVM?.AddToiletCommand.Execute(NewToilet);
                // сбрасываю данные туалета VM (но лучше сделать метод .Clear();
                NewToilet = new();
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
    public void OnToiletsUpdated(ObservableCollection<Toilet> toilets)
    {
        if (pointFeatures != null)
        {
            pointFeatures.Clear();
            var tmp = MapPinManager.GetFeatures(toilets.AsEnumerable());
            pointFeatures.AddRange(tmp);
            pointFeatures?.DataHasChanged();
        }
    }
    private async void OnLocationUpdate(Location location)
    {
        if (location != null)
        {
             CurrentLocationLabel = await StandartGeoCodingService.GetAddressFromCoordinates(location.Latitude, location.Longitude);
        }
    }

}