using Entities;
using MFASeekerApp.Model.Interfaces;
using System.Text.Json;

namespace MFASeekerApp.Services
{
    class LocalToiletService : IToiletService
    {
        private readonly string filePath;

        public LocalToiletService()
        {
            filePath = Path.Combine(FileSystem.Current.AppDataDirectory, "markers.json");
            string oldFilePath = Path.Combine(FileSystem.Current.CacheDirectory, "markers.json");
            if (File.Exists(oldFilePath) && !File.Exists(filePath))
            {
                File.Copy(oldFilePath, filePath, true);
            }
            else if (!File.Exists(oldFilePath) && !File.Exists(filePath))
            {
                var toiletsList = new List<Toilet>();
                var json = JsonSerializer.Serialize(toiletsList);
                File.WriteAllText(filePath, json);
            }
        }

        public async Task<List<Toilet>> GetAllToilets()
        {
            try
            {
                var json = await File.ReadAllTextAsync(filePath);
                var toilets = JsonSerializer.Deserialize<List<Toilet>>(json) ?? [];
                return toilets;
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"Ќеверный формат json: {jsonEx.Message}");
                string backupFilePath = $"{filePath}.old";
                File.Copy(filePath, backupFilePath, true);
                await File.WriteAllTextAsync(filePath, string.Empty);
                return [];
            }
        }
        public async Task AddToilet(Toilet marker)
        {
            var toiletsJson = await GetAllToilets();
            toiletsJson.Add(marker);

            var json = JsonSerializer.Serialize(toiletsJson);
            await File.WriteAllTextAsync(filePath, json);
        }
        public async Task DeleteToilet(Guid guid)
        {
            var toiletsJson = await GetAllToilets();
            var markerToDelete = toiletsJson.FirstOrDefault(t => t.Guid == guid);
            if (markerToDelete != null)
            {
                toiletsJson.Remove(markerToDelete);
                var json = JsonSerializer.Serialize(toiletsJson);
                await File.WriteAllTextAsync(filePath, json);
            }
        }
        public async Task UpdateToilet(Toilet toilet)
        {
            var toiletsJson = await GetAllToilets();
            var markerToUpdate = toiletsJson.FirstOrDefault(t => t.Guid == toilet.Guid);
            if (markerToUpdate != null)
            {
                // ќбновл€ем пол€ существующего маркера
                markerToUpdate.Name = toilet.Name;
                // markerToUpdate.Location = toilet.Location; в будущем и локацию
                markerToUpdate.Rating = toilet.Rating;
                markerToUpdate.Images = toilet.Images;
                markerToUpdate.Description = toilet.Description;

                var json = JsonSerializer.Serialize(toiletsJson);
                await File.WriteAllTextAsync(filePath, json);
            }
        }
        public Task<Toilet> GetToiletByGuid(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
