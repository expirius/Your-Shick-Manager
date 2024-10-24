using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Projections;
using Mapsui.Styles;
using Mapsui.Utilities;
using MFASeeker.Services;
using Entities;
using System.Text;
using Color = Mapsui.Styles.Color;
using Exception = System.Exception;

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
        public static PointFeature GetFeature(Toilet toilet)
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
        public static IEnumerable<PointFeature> GetFeatures(IEnumerable<Toilet> toilets)
        {
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
        // Локальные пины для теста
        //public async static Task<IEnumerable<PointFeature>> GetFeaturesLocalAsync()
        //{
        //    JsonPinStorage pinStorage = new();
        //    var toilets = await pinStorage.GetMarkersAsync();
        //    // добавление самой точки и её иконки + callout style
        //    if (toilets != null)
        //        return toilets.Select(t =>
        //        {
        //        var feature = new PointFeature(SphericalMercator.FromLonLat(t.Location.Longitude, t.Location.Latitude).ToMPoint());
        //        feature[nameof(t.Name)] = t.Name;
        //        feature[nameof(t.Id)] = t.Id;
        //        feature[nameof(t.Location.Longitude)] = t.Location.Longitude;
        //        feature[nameof(t.Location.Latitude)] = t.Location.Latitude;
        //        feature[nameof(t.Description)] = t.Description;
        //        feature[nameof(t.Rating)] = t.Rating;
        //        // styles
        //        feature.Styles.Add(CreateSvgStyle(ToiletIconProvider.GetIconPath(t), 0.08));
        //        feature.Styles.Add(CreateCalloutStyleByToilet(t));

        //        return feature;
        //    });
        //    else { return []; }  
        //}

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
