using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Projections;
using Mapsui.Providers;
using Mapsui.Styles;
using Color = Mapsui.Styles.Color;

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
                Style = SymbolStyles.CreatePinStyle(symbolScale: 0.7),
            };
        }

        public PinManager()
        {
            //_apiService = apiService;
        }

        private IEnumerable<IFeature> GetFeaturesLocal()
        {
            var toilets = GetLocalToiletsTest();
            return toilets.Select(t =>
            {
                var feature = new PointFeature(SphericalMercator.FromLonLat(t.Location.Longitude, t.Location.Latitude).ToMPoint());
                feature[nameof(t.Name)] = t.Name;
                feature[nameof(t.Id)] = t.Id;
                feature[nameof(t.Location.Longitude)] = t.Location.Longitude;
                feature[nameof(t.Location.Latitude)] = t.Location.Latitude;
                feature[nameof(t.Description)] = t.Description;
                feature.Styles.Add(CreateCalloutStyle(feature.ToStringOfKeyValuePairs()));
                return feature;
            });
        }
        private IEnumerable<Toilet> GetLocalToiletsTest()
        {
            return new List<Toilet>
            {
                new Toilet { Id = 0, Name = "Кофейня", Description = "ТЕКТСКРЫШЛАВЫШ фывфы", Location = new Location(53.256586, 34.373289)},
                new Toilet { Id = 1, Name = "Гостиница, ресторан", Description = "фыы asdasf фывфы", Location = new Location(53.254117, 34.377019)},
                new Toilet { Id = 2, Name = "Озон", Description = "Попросись у кассиршы, иногда #%#@ пускает0.", Location = new Location(53.253010, 34.375285)},

            };
        }
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
                SymbolOffset = new Offset(0, SymbolStyle.DefaultHeight * 1f)
            };
        }
    }
}
