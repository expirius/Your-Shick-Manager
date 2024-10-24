namespace Entities
{
    public class Toilet
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        public string? Name { get; set; } = string.Empty;
        public Location? Location { get; set; }
        public string? Description { get; set; } = string.Empty;
        public double Rating { get; set; } = 0;
        public string? UserName { get; set; }
        public string? UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<ImageFile> Images { get; set; } = [];

        public Toilet()
        {
            this.Guid = CreateGuid();
            CreatedDate = DateTime.Now;
        }

        public string GetInfo()
        {
            return $"Name: {Name}, Location: {Location}, Description: {Description}, Rating: {Rating}, Created By: {UserId}, On: {CreatedDate}";
        }
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }
        private string CreateGuid()
        {
            return System.Guid.NewGuid().ToString().GetHashCode().ToString("x");
        }

    }
}
