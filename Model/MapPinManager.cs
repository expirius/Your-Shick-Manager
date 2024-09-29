using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Projections;
using Mapsui.Providers;
using Mapsui.Styles;
using Color = Mapsui.Styles.Color;
using Mapsui.Utilities;
using Exception = System.Exception;
using System.Text;
using System.Text.Json;
using MFASeeker.Services;

namespace MFASeeker.Model
{
    public static class MapPinManager
    {
        public static WritableLayer CreatePointLayer(string name, bool isInfoLayer)
        {
            return new WritableLayer( )
            {
                Name = name,
                IsMapInfoLayer = isInfoLayer,
            };
        }
        private static PointFeature CreateFeature(Toilet toilet)
        {
                // описание точки
                var feature = new PointFeature(SphericalMercator.FromLonLat(toilet.Location.Longitude, toilet.Location.Latitude).ToMPoint());
                feature[nameof(toilet.Name)] = toilet.Name;
                feature[nameof(toilet.Id)] = toilet.Id;
                feature[nameof(toilet.Location.Longitude)] = toilet.Location.Longitude;
                feature[nameof(toilet.Location.Latitude)] = toilet.Location.Latitude;
                feature[nameof(toilet.Description)] = toilet.Description;
                feature[nameof(toilet.Rating)] = toilet.Rating;
                // styles
                feature.Styles.Add(CreateSvgStyle(ToiletIconProvider.GetIconPath(toilet), 0.08));
                feature.Styles.Add(CreateCalloutStyleByToilet(toilet));
                
                return feature;
        }
        public static PointFeature GetFeatureMark(Toilet toilet)
        {
            // сохранение в память
            JsonPinStorage pinStorage = new();
            pinStorage.SaveMarker(toilet);
            // Возврат фичи
            return CreateFeature(toilet);
        }
        // Локальные пины для теста
        public static IEnumerable<PointFeature> GetFeaturesLocal()
        {
            JsonPinStorage pinStorage = new();
            var toilets = pinStorage.GetMarkers();
            // добавление самой точки и её иконки + callout style
            if (toilets != null)
                return toilets.Select(t =>
                {
                var feature = new PointFeature(SphericalMercator.FromLonLat(t.Location.Longitude, t.Location.Latitude).ToMPoint());
                feature[nameof(t.Name)] = t.Name;
                feature[nameof(t.Id)] = t.Id;
                feature[nameof(t.Location.Longitude)] = t.Location.Longitude;
                feature[nameof(t.Location.Latitude)] = t.Location.Latitude;
                feature[nameof(t.Description)] = t.Description;
                feature[nameof(t.Rating)] = t.Rating;
                // styles
                feature.Styles.Add(CreateSvgStyle(ToiletIconProvider.GetIconPath(t), 0.08));
                feature.Styles.Add(CreateCalloutStyleByToilet(t));

                return feature;
            });
            else { return []; }
            
        }

        // Стиль карточки пина
        private static CalloutStyle CreateCalloutStyleByToilet(Toilet t)
        {
            // Формируем строку с нужной информацией
            StringBuilder keyValuePairs = new();
            keyValuePairs.AppendLine($"Точка: {t.Name}\n");
            keyValuePairs.AppendLine($"Описание: {t.Description}\n");
            keyValuePairs.AppendLine($"Рейтинг: {t.Rating}\n");
            /*
            * получение адреса в callout'e. 
            * Обязательно в будущем каждому туалету давать адрес при сохранении точки.
            * Так меньше нагрузки
            */
            var address = Task.Run(() => StandartGeoCodingService.GetAddressFromCoordinates(t.Location.Latitude, t.Location.Longitude))
                .GetAwaiter().GetResult();
            keyValuePairs.AppendLine($"Адрес: {address}");
            return new CalloutStyle
            {
                    Title = keyValuePairs.ToString(),
                    TitleFont = { FontFamily = null, Size = 12, Italic = false, Bold = true },
                    TitleFontColor = Color.Gray,
                    MaxWidth = 120,
                    RectRadius = 10,
                    ShadowWidth = 4,
                    Enabled = false,
            };
        }
        // Создание svg для пина
        private static SymbolStyle CreateSvgStyle(string imagePath, double scale)
        {
            try
            {
                var pinId = typeof(MapPinManager).LoadSvgId(imagePath);
                return new SymbolStyle
                {
                    BitmapId = pinId,
                    SymbolScale = scale
                };
            }
            catch (Exception) { throw;}
        }
    }
}
