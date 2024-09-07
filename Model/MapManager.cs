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
        public async Task EnableSpectateModeAsync(CancellationToken cancellationToken)
        {
            if (Map == null)
                return;

            _myLocationLayer?.Dispose();
            _myLocationLayer = new MyLocationLayer(Map);

            Map.Layers.Add(OpenStreetMap.CreateTileLayer());
            Map.Layers.Add(_myLocationLayer);

            // Включение компаса
            //CompassVM compassVM = new CompassVM();
            //await compassVM.ToggleCompass(cancellationToken);

            // Мониторинг текущего местоположения
            var progress = new Progress<Location>(location =>
            {
                var currentLocation = ConvertToMPoint(location);
                // конвертируется МПоинт в сферические координаты
                currentLocation = SphericalMercator.FromLonLat(currentLocation.X, currentLocation.Y).ToMPoint(); ;
                _myLocationLayer.UpdateMyLocation(currentLocation, true);

            });
            // При изменении локции прогресс прогоняется заново
            await Geolocator.Default.StartListening(progress, cancellationToken); 

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
