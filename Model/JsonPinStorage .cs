using System.Text.Json;

namespace MFASeeker.Model;

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
        var markerToDelete = toiletsJson.FirstOrDefault(t => t.Id == marker.Id);
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
}
