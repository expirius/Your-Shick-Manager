using Mapsui;
using Mapsui.Extensions;
using Mapsui.Projections;
using Mapsui.Tiling;
using Mapsui.UI.Objects;
using Mapsui.Widgets;
using Mapsui.Widgets.ButtonWidget;
using Mapsui.Widgets.Zoom;
using Svg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Mapsui.Layers;
using Map = Mapsui.Map;
using MyLocationLayer = Mapsui.Layers.MyLocationLayer;
using Mapsui.UI.Maui;
using Microsoft.Maui.ApplicationModel;
using static Microsoft.Maui.ApplicationModel.Permissions;
using System.Threading;
using MFASeeker.ViewModel;
using System.ComponentModel;
using BruTile.Tms;

namespace MFASeeker.Model
{
    public class MapManager
    {
        public MapManager() 
        {
            CreateMap();
        }
        private readonly MPoint MOSCOWLOCATION = new(37.6156, 55.7522);
        public Map? Map;
        private MyLocationLayer? _myLocationLayer;
        private Geolocator compass = new();
        private CancellationToken cancellationToken;
        // Работа с картой
        private void CreateMap()
        {
            Map = new() { CRS = "EPSG:3857" };

            Map.Layers.Add(OpenStreetMap.CreateTileLayer());
            // Добавление виджета масштабирование + и -
            Map.Widgets.Add(CreateZoomInOutWidget(Orientation.Vertical, Mapsui.Widgets.VerticalAlignment.Top, Mapsui.Widgets.HorizontalAlignment.Right));

            // Переобразование под сферический тип координат для OSM
            var sphericalMercatorCoordinate = SphericalMercator.FromLonLat(MOSCOWLOCATION.X, MOSCOWLOCATION.Y).ToMPoint();
            Map.Home = n => n.CenterOnAndZoomTo(sphericalMercatorCoordinate, n.Resolutions[19]);
        }
        public async Task EnableCompassModeAsync()
        {
            // Мониторинг компаса
            compass.OnCompassChangedAction += (newReading) =>
            {
                if (Map == null)
                    return;
                if (_myLocationLayer == null)
                    return;
                _myLocationLayer.UpdateMyViewDirection(newReading, Map.Navigator.Viewport.Rotation, true);
            };
            await compass.StartUpdateCompassAsync();
        }
        public async Task EnableSpectateModeAsync(CancellationToken token)
        {
            cancellationToken = token;
            if (Map == null)
                return;
            if (_myLocationLayer != null)
                return;
            _myLocationLayer?.Dispose();
            _myLocationLayer = new MyLocationLayer(Map);
            Map.Layers.Add(OpenStreetMap.CreateTileLayer());
            Map.Layers.Add(_myLocationLayer);

            // Мониторинг текущего местоположения
            var progress = new Progress<Location>(location =>
            {
                var currentLocation = ConvertToMPoint(location);
                // конвертируется МПоинт в сферические координаты
                currentLocation = SphericalMercator.FromLonLat(currentLocation.X, currentLocation.Y).ToMPoint();
                if(!cancellationToken.IsCancellationRequested)
                    _myLocationLayer.UpdateMyLocation(currentLocation, true);
            });
            // При изменении локции прогресс прогоняется заново
            await Geolocator.Default.StartListening(progress, cancellationToken);
        }
        /*
        public void StopSpectateMode()
        {
            if (Map == null)
                return;
            if (Map.Layers.Count == 0)
                return;
            CancellationTokenSource cts = new();
            cts.Cancel();
            cancellationToken = cts.Token;

        }
        */
        // Виджеты
        private static ZoomInOutWidget CreateZoomInOutWidget(Orientation orientation,
        Mapsui.Widgets.VerticalAlignment verticalAlignment, Mapsui.Widgets.HorizontalAlignment horizontalAlignment)
        {
            return new ZoomInOutWidget
            {
                Orientation = orientation,
                VerticalAlignment = verticalAlignment,
                HorizontalAlignment = horizontalAlignment,
                MarginX = 20,
                MarginY = 20,
            };
        }
        // Переобразование точки в MPoint для MAPSUI
        private MPoint? ConvertToMPoint(Location? locationTask)
        {
            Location? location = locationTask;
            if (location == null)
                return null;

            return new MPoint { X = location.Longitude, Y = location.Latitude };
        }
    }
}
