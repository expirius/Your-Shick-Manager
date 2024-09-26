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

namespace MFASeeker.Model
{
    public static class MapPinManager
    {
        public static WritableLayer CreatePointLayer()
        {
            return new WritableLayer()
            {
                Name = "AllToilets",
                IsMapInfoLayer = true,
            };
        }
        public static PointFeature CreateMarkFeature(Toilet toilet)
        {
            Toilet newToilet = new()
            {
                Name = toilet.Name ?? "",
                Location = toilet.Location,
                Rating = toilet.Rating,
                Description = toilet.Description ?? "",
                CreatedDate = DateTime.Now,
                UserName = DeviceInfo.Current.Name,
            };
            return GetFeature(newToilet);
        }
        private static PointFeature GetFeature(Toilet toilet)
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
            feature.Styles.Add(CreateCalloutStyleByToilet(toilet));
            feature.Styles.Add(CreateSvgStyle(ToiletIconProvider.GetIconPath(toilet), 0.08));
            return feature;
        }
        // Локальные пины для теста
        public static IEnumerable<PointFeature> GetFeaturesLocal()
        {
            var toilets = GetLocalToiletsTest();
            // добавление самой точки и её иконки + callout style
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
                feature.Styles.Add(CreateCalloutStyleByToilet(t));
                feature.Styles.Add(CreateSvgStyle(ToiletIconProvider.GetIconPath(t), 0.08));
                return feature;
            });
            
        }
        private static IEnumerable<Toilet> GetLocalToiletsTest()
        {
            return
            [
                new() { Id = 0, Rating = 0, Name = "Кофейня", Description = "ОПИСАНИЕ БОЛЬШОЕ ОЧЕНЬ ОПИСАНИЕ БОЛЬШОЕ ОЧЕНЬ ОПИСАНИЕ БОЛЬШОЕ ОЧЕНЬ", Location = new Location(53.256586, 34.373289)},
                new() { Id = 1, Rating = 5, Name = "Гостиница, ресторан", Description = "фыы asdasf фывфы", Location = new Location(53.254117, 34.377019)},
                new() { Id = 2, Rating = 3.44, Name = "Озон", Description = "Попросись у кассиршы, иногда пускает0#%#@ ффф.", Location = new Location(53.253010, 34.375285)},
                new() { Id = 3, Rating = 2.2, Name = "Пивнушка", Description = "", Location = new Location(53.254977, 34.373497)},
                new() { Id = 4, Rating = 1.9690, Name = "A school", Location = new Location(53.254519, 34.375026)},
                new() { Id = 5, Rating = 4, Name = "Сарай", Location = new Location(53.256070, 34.374074)},
            ];
        }
        // Стиль карточки пина
        private static CalloutStyle CreateCalloutStyleByToilet(Toilet t)
        {
            // Формируем строку с нужной информацией
            var keyValuePairs = new StringBuilder();
            keyValuePairs.AppendLine($"Точка: {t.Name}");
            keyValuePairs.AppendLine($"Описание: {t.Description}");
            keyValuePairs.AppendLine($"Рейтинг: {t.Rating}");

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
        // Временный метод для заполнения и тестирования
    }
}
