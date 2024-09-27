using Mapsui;
using Mapsui.Extensions;
using Mapsui.Projections;
using Mapsui.Tiling;
using Mapsui.Widgets.Zoom;
using Microsoft.Maui.Devices.Sensors;
using System.Threading;
using Map = Mapsui.Map;
using MyLocationLayer = Mapsui.Layers.MyLocationLayer;

namespace MFASeeker.Model
{
    public static class MapManager
    {
        private static readonly MPoint MOSCOWLOCATION = new(37.6156, 55.7522);
        private static Map? Map;
        private static MyLocationLayer? _myLocationLayer;
        private static CancellationTokenSource? cancellationTokenSource;

        public static MPoint? CurrentLocationMPoint;

        // Работа с картой
        public static Map CreateMap()
        {
            Map = new() { CRS = "EPSG:3857" };
            Map.Layers.Add(OpenStreetMap.CreateTileLayer());
            // Добавление виджета масштабирование + и -
            Map.Widgets.Add(CreateZoomInOutWidget(Orientation.Vertical, Mapsui.Widgets.VerticalAlignment.Bottom, Mapsui.Widgets.HorizontalAlignment.Right));

            // Переобразование под сферический тип координат для OSM
            var sphericalMercatorCoordinate = SphericalMercator.FromLonLat(MOSCOWLOCATION.X, MOSCOWLOCATION.Y).ToMPoint();
            Map.Home = n => n.CenterOnAndZoomTo(sphericalMercatorCoordinate, n.Resolutions[19]);
            // Слой с позицией пользователя
            if (_myLocationLayer == null)
            {
                _myLocationLayer = new MyLocationLayer(Map);
                Map.Layers.Add(_myLocationLayer);
            }
            _ = StartListeningLocation();
            return Map;
        }
        public static void ToggleCompassMode()
        {
            // Мониторинг компаса
            Geolocator.OnCompassChangedAction += (newReading) =>
            {
                if (Map == null || _myLocationLayer == null)
                    return;
                if (newReading == -1)
                    _myLocationLayer.UpdateMyViewDirection(-1, 0, false);
                else
                    _myLocationLayer.UpdateMyViewDirection(newReading, Map.Navigator.Viewport.Rotation, true);
            };
            Geolocator.ToggleCompass();
        }
        public static void StartSpectateMode()
        {
            if (_myLocationLayer != null)
            {
                _myLocationLayer.IsCentered = true;
            }
        }
        public static void StopSpectateMode()
        {
            if (_myLocationLayer != null)
                _myLocationLayer.IsCentered = false;
        }

        private static async Task StartListeningLocation()
        {
            if (_myLocationLayer == null) return;
            cancellationTokenSource = new();
            StartSpectateMode();
            var progress = new Progress<Location>(UpdateLocation);
            await Geolocator.Default.StartListening(progress, cancellationTokenSource.Token);
        }
        private static void UpdateLocation(Location location)
        {
            // Конвертируем в сферические координаты и обновляем карту
            CurrentLocationMPoint = SphericalMercator.FromLonLat(location.Longitude, location.Latitude).ToMPoint();
            _myLocationLayer?.UpdateMyLocation(CurrentLocationMPoint, true);
        }
        // Виджеты
        private static ZoomInOutWidget CreateZoomInOutWidget(Orientation orientation,
        Mapsui.Widgets.VerticalAlignment verticalAlignment, Mapsui.Widgets.HorizontalAlignment horizontalAlignment)
        {
            return new ZoomInOutWidget
            {
                Orientation = orientation,
                VerticalAlignment = verticalAlignment,
                HorizontalAlignment = horizontalAlignment,
                MarginX = 25,
                MarginY = 150,
                BackColor = new Mapsui.Styles.Color(0, 0, 0, 240),
                Size = 45f
            };
        }
    }
}
