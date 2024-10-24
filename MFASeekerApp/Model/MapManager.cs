using Mapsui;
using Mapsui.Extensions;
using Mapsui.Projections;
using Mapsui.Tiling;
using Mapsui.Widgets.Zoom;
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
            _myLocationLayer = new MyLocationLayer(Map);
            Map.Layers.Add(_myLocationLayer);

            _ = StartListeningLocation();
            return Map;
        }
        /* ЭТО НЕПРАВИЛЬНО РАБОТАЮЩИЙ КОМПАС
            * При изменении rotation у viewport 
            * UpdateMyViewDirection не учитывает Map.Navigator.Viewport.Rotation
            * Скорее всего карта не обновляется, т.к. ее бэкнул из метода
            * И там отдался объект а не ссылка на свойство
            * Доделаю через время, наверно. Пока не дебажил
            */
        public static void ToggleCompassMode()
        {
            Geolocator.OnCompassChangedAction += (newReading) =>
            {
                if (Map == null || _myLocationLayer == null)
                    return;
                //if (newReading == -1)
                //    _myLocationLayer.UpdateMyViewDirection(-1, 0);
                //else
                //    _myLocationLayer.UpdateMyViewDirection(newReading, Map.Navigator.Viewport.Rotation);
            };
            Geolocator.ToggleCompass();
        }

        public static void CenterToUserLocation()
        {
            if (CurrentLocationMPoint != null)
                Map?.Navigator.CenterOn(CurrentLocationMPoint);
        }
        public static void EnableCentredUser()
        {
            if (_myLocationLayer != null)
            {
                _myLocationLayer.IsCentered = true;
            }
        }
        public static void DisableCentredUser()
        {
            if (_myLocationLayer != null)
                _myLocationLayer.IsCentered = false;
        }

        private static async Task StartListeningLocation()
        {
            if (_myLocationLayer == null) return;
            cancellationTokenSource = new();
            var progress = new Progress<Location>(UpdateLocation);
            await Geolocator.Default.StartListening(progress, cancellationTokenSource.Token);
        }
        private static void UpdateLocation(Location location)
        {
            // Конвертируем в сферические координаты и обновляем карту
            CurrentLocationMPoint = SphericalMercator.FromLonLat(location.Longitude, location.Latitude).ToMPoint();
            _myLocationLayer?.UpdateMyLocation(CurrentLocationMPoint, true);

            LocationUpdated?.Invoke(location);
        }
        // events
        public static event Action<Location> LocationUpdated;
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
