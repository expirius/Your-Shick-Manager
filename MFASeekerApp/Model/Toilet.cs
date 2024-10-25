using System.Collections.ObjectModel;

namespace Entities
{
    public class Toilet : BaseEntity
    {
        public Location? Location { get; set; }
        public User? User { get; set; }

        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public double Rating { get; set; } = 0;
        public ObservableCollection<ImageFile> Images { get; set; } = []; // ДЛЯ LOCAL. Возможно потребуется вывести в VM
        public bool IsPrivate { get; set; } = true;

        public Toilet()
        {
            this.Guid = CreateGuid();
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
        private Guid CreateGuid() => new(System.Guid.NewGuid().ToString().GetHashCode().ToString("x"));

    }
}
