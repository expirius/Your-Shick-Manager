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
using System.Runtime.CompilerServices;

namespace MFASeeker.Model
{
    public static class PinManager
    {
        private static MemoryLayer _userToiletsLayer;
        //private static readonly string? _apiService;

        public static List<IFeature> CreatePointLayer()
        {
            return GetFeaturesLocal().ToList();
                //new MemoryLayer
                //Name = "AllToilets",
                //IsMapInfoLayer = true,
                //new MemoryProvider(GetFeaturesLocal()).Features;
        }


        public static IFeature AddNewMarkOnLayer(Location location)
        {
            // Добавляем новый Feature в MemoryProvider
            var newToilet = CreateMark("test1", location);

            return GetFeature(newToilet);
        }
        /// <summary>
        /// Добавление новой метки
        /// </summary>
        /// <param name="nameMark">Имя метки</param>
        /// <param name="location">Локация метки</param>
        /// <param name="rating">Установленный рейтинг</param>
        /// <param name="description">Описание</param>
        private static UserToiletMarker CreateMark(string nameMark, Location location, 
                                        double rating = 0, string description ="")
        {
            return new(){
                Name = nameMark,
                Location = location,
                Rating = rating,
                Description = description,
                CreatedDate = DateTime.Now,
                UserName = DeviceInfo.Current.Name,
            };
        }

        //КАК ПРАВИЛЬНО ТУТ ПЕРЕДЕЛАТЬ
        private static IFeature GetFeature(UserToiletMarker toilet)
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
            string svgIconPath = toilet.Rating switch
            {
                5 => @"Resources.Icons.rank5_toilet.svg",
                >= 4 => @"Resources.Icons.rank4_toilet.svg",
                >= 3 => @"Resources.Icons.rank3_toilet.svg",
                >= 2 => @"Resources.Icons.rank2_toilet.svg",
                >= 1 => @"Resources.Icons.rank1_toilet.svg",
                _ => @"Resources.Icons.defaultrank_toilet.svg"
            };

            feature.Styles.Add(CreateSvgStyle(svgIconPath, 0.1));
            feature.Styles.Add(CreateCalloutStyleByToilet(toilet));
            return feature;
        }
        private static IEnumerable<IFeature> GetFeaturesLocal()
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
                feature[nameof(t.Rating)] = t.Rating;
                // styles
                switch (t.Rating)
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
        // Карточка по туалету
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

        // Временный метод для заполнения и тестирования
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
    }
}
