using Entities;
using System.Collections.ObjectModel;

namespace MFASeekerApp.Model
{
    public class Toilet : BaseEntity
    {
        public string Location { get; set; } = string.Empty;
        public User? User { get; set; }

        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public double Rating { get; set; } = 0;
        public List<ImageFile> Images { get; set; } = []; // ДЛЯ LOCAL. Возможно потребуется вывести в VM
        public bool IsPrivate { get; set; } = true;

        public Toilet()
        {
            Guid = CreateGuid();
            CreatedDate = DateTime.Now;
        }
        public string GetInfo()
        {
            return $"Name: {Name}, Location: {Location}, Description: {Description}, Rating: {Rating}, Created By: {User?.UserName}, On: {CreatedDate}";
        }
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }
        private string CreateGuid() => System.Guid.NewGuid().ToString().GetHashCode().ToString("x");
        public static Location ParseLocationFromString(string location)
        {
            try
            {
                // Убираем пробелы и разделяем строку по запятой
                var coordinates = location.Replace(" ", "").Split(',');
                // Проверяем, что координаты разделены правильно (на 2 части)
                if (double.TryParse($"{coordinates[0]},{coordinates[1]}", out double latitude1) &&
                    double.TryParse($"{coordinates[2]},{coordinates[3]}", out double longitude1))
                    return new Location(latitude1, longitude1);
                else
                {
                    Console.WriteLine($"В координатах ошибка разделителей");
                    return new();
                }
            } catch (Exception ex)
            {
                Console.WriteLine($"Не получилось преобразовать локацию из строки в Location, {ex.Message}");
                return new();
            }
        }
    }
}
