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

namespace MFASeeker.Model
{
    public class PinManager
    {
        private static readonly string? _apiService;

        public MemoryLayer CreatePointLayer()
            {
                return new MemoryLayer
                {
                    Name = "AllToilets",
                    IsMapInfoLayer = true,
                    Features = new MemoryProvider(GetFeaturesLocal()).Features,
                    //Style = CreateSvgStyle(@"Resources.Icons.toilet_marker_dark.svg", 0.06)
                };
            }

        public PinManager()
        {
            //_apiService = apiService;
        }

        private IEnumerable<IFeature> GetFeaturesLocal()
        {
            //Сделать в будущем в зависимости от типа (Id) соответствующую иконку
            //var pinId = typeof(PinManager).LoadSvgId("toileticon1.svg"); // пример
            var toilets = GetLocalToiletsTest();
            return toilets.Select(t =>
            {
                var feature = new PointFeature(SphericalMercator.FromLonLat(t.Location.Longitude, t.Location.Latitude).ToMPoint());
                feature[nameof(t.Name)] = t.Name;
                feature[nameof(t.Id)] = t.Id;
                feature[nameof(t.Location.Longitude)] = t.Location.Longitude;
                feature[nameof(t.Location.Latitude)] = t.Location.Latitude;
                feature[nameof(t.Description)] = t.Description;
                feature[nameof(t.Raiting)] = t.Raiting;
                // styles
                switch (t.Raiting)
                {
                    case 5:
                        feature.Styles.Add(CreateSvgStyle(@"Resources.Icons.rank5_toilet.svg", 0.1));
                        break;
                    case >=4:
                        feature.Styles.Add(CreateSvgStyle(@"Resources.Icons.rank4_toilet.svg", 0.1));
                        break;
                    case >=3:
                        feature.Styles.Add(CreateSvgStyle(@"Resources.Icons.rank3_toilet.svg", 0.1));
                        break;
                    case >=2:
                        feature.Styles.Add(CreateSvgStyle(@"Resources.Icons.rank2_toilet.svg", 0.1));
                        break;
                    case >=1:
                        feature.Styles.Add(CreateSvgStyle(@"Resources.Icons.rank1_toilet.svg", 0.1));
                        break;
                    default:
                        feature.Styles.Add(CreateSvgStyle(@"Resources.Icons.defaultrank_toilet.svg", 0.1));
                        break;
                }
                //feature.Styles.Add(CreateCalloutStyle(feature.ToStringOfKeyValuePairs()));
                feature.Styles.Add(CreateCalloutStyleByToilet(t));
                return feature;
            });
        }
        // Временный метод для заполнения и тестирования
        private IEnumerable<Toilet> GetLocalToiletsTest()
        {
            return
            [
                new() { Id = 0, Raiting = 0, Name = "Кофейня", Description = "ОПИСАНИЕ БОЛЬШОЕ ОЧЕНЬ ОПИСАНИЕ БОЛЬШОЕ ОЧЕНЬ ОПИСАНИЕ БОЛЬШОЕ ОЧЕНЬ", Location = new Location(53.256586, 34.373289)},
                new() { Id = 1, Raiting = 5, Name = "Гостиница, ресторан", Description = "фыы asdasf фывфы", Location = new Location(53.254117, 34.377019)},
                new() { Id = 2, Raiting = 3.44, Name = "Озон", Description = "Попросись у кассиршы, иногда пускает0#%#@ ффф.", Location = new Location(53.253010, 34.375285)},
                new() { Id = 3, Raiting = 2.2, Name = "Пивнушка", Description = "", Location = new Location(53.254977, 34.373497)},
                new() { Id = 4, Raiting = 1.9690, Name = "A school", Location = new Location(53.254519, 34.375026)},
                new() { Id = 5, Raiting = 4, Name = "Сарай", Location = new Location(53.256070, 34.374074)},
            ];
        }
        //
        private static CalloutStyle CreateCalloutStyleByToilet(Toilet t)
        {
            // Формируем строку с нужной информацией
            var keyValuePairs = new StringBuilder();
            keyValuePairs.AppendLine($"Точка: {t.Name}");
            keyValuePairs.AppendLine($"Описание: {t.Description}");
            keyValuePairs.AppendLine($"Рейтинг: {t.Raiting}");
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
        // Стиль карточки пина
        private static CalloutStyle CreateCalloutStyle(string content)
        {
            return new CalloutStyle
            {
                Title = content,
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
                var pinId = typeof(PinManager).LoadSvgId(imagePath);
                return new SymbolStyle
                {
                    BitmapId = pinId,
                    SymbolScale = scale
                };
            }
            catch (Exception ex) {throw;}
        }
    }
}
