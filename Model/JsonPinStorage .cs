using System.Text.Json;

namespace MFASeeker.Model
{
    public class JsonPinStorage : IPinStorage
    {
        private readonly string filePath;
        public JsonPinStorage()
        {
            filePath = Path.Combine(FileSystem.Current.CacheDirectory, "markers.json"); // Использование локального хранилища
            if (!File.Exists(filePath))
            {
                // Создаем пустой файл с начальными данными
                var toiletsList = new List<Toilet>(); // Пустой список
                var json = JsonSerializer.Serialize(toiletsList);
                File.WriteAllText(filePath, json);
            }
        }
        
        public void DeleteMarker(Toilet marker)
        {
            var toiletsJson = GetMarkers();
            var markerToDelete = toiletsJson.FirstOrDefault(marker);
            if (markerToDelete != null)
            {
                toiletsJson.Remove(markerToDelete);
                foreach (var toilet in toiletsJson) 
                {
                    SaveMarker(toilet);
                }
            }
        }

        public List<Toilet> GetMarkers()
        {
            try
            {
                var json = File.ReadAllText(filePath);
                var toilets = JsonSerializer.Deserialize<List<Toilet>>(json) ?? [];
                return toilets;
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"Неверный формат json: {jsonEx.Message}");
                // Создание резервной копии
                string backupFilePath = $"{filePath}.old";
                File.Copy(filePath, backupFilePath, true); // Копирование файла в .old

                // Очистка оригинального файла
                File.WriteAllText(filePath, string.Empty); // Очистка файла
                return [];
            }
        }

        public void SaveMarker(Toilet marker)
        {

            var toiletsJson = GetMarkers();
            toiletsJson.Add(marker);

            var json = JsonSerializer.Serialize(toiletsJson);
            File.WriteAllText(filePath, json);

        }
        // Для теста загрузка в json
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
