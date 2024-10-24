using System.Text.Json;

namespace MFASeeker.Model;

// Всё это дело лучше увести в static 
// Переименовать в LocalPinStorage
public class JsonPinStorage : IPinStorage
{
    private readonly string filePath;

    public JsonPinStorage()
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

    public async Task<List<Toilet>> GetMarkersAsync()
    {
        try
        {
            var json = await File.ReadAllTextAsync(filePath);
            var toilets = JsonSerializer.Deserialize<List<Toilet>>(json) ?? [];
            return toilets;
        }
        catch (JsonException jsonEx)
        {
            Console.WriteLine($"Неверный формат json: {jsonEx.Message}");
            string backupFilePath = $"{filePath}.old";
            File.Copy(filePath, backupFilePath, true);
            await File.WriteAllTextAsync(filePath, string.Empty);
            return [];
        }
    }

    public async Task SaveMarkerAsync(Toilet marker)
    {
        var toiletsJson = await GetMarkersAsync();
        toiletsJson.Add(marker);

        var json = JsonSerializer.Serialize(toiletsJson);
        await File.WriteAllTextAsync(filePath, json);
    }

    public async Task DeleteMarkerAsync(Toilet marker)
    {
        var toiletsJson = await GetMarkersAsync();
        var markerToDelete = toiletsJson.FirstOrDefault(t => t.Guid == marker.Guid);
        if (markerToDelete != null)
        {
            toiletsJson.Remove(markerToDelete);
            var json = JsonSerializer.Serialize(toiletsJson);
            await File.WriteAllTextAsync(filePath, json);
        }
    }
    public async Task DeleteMarkerAsync(string guid)
    {
        var toiletsJson = await GetMarkersAsync();
        var markerToDelete = toiletsJson.FirstOrDefault(t => t.Guid == guid);
        if (markerToDelete != null)
        {
            toiletsJson.Remove(markerToDelete);
            var json = JsonSerializer.Serialize(toiletsJson);
            await File.WriteAllTextAsync(filePath, json);
        }
    }
    public async Task UpdateMarker(Toilet toilet)
    {
        var toiletsJson = await GetMarkersAsync();
        var markerToUpdate = toiletsJson.FirstOrDefault(t => t.Guid == toilet.Guid);
        if (markerToUpdate != null)
        {
            // Обновляем поля существующего маркера
            markerToUpdate.Name = toilet.Name;
            // markerToUpdate.Location = toilet.Location; в будущем и локацию
            markerToUpdate.Rating = toilet.Rating;
            markerToUpdate.Images = toilet.Images;
            markerToUpdate.Description = toilet.Description;

            var json = JsonSerializer.Serialize(toiletsJson);
            await File.WriteAllTextAsync(filePath, json);
        }
    }
}
