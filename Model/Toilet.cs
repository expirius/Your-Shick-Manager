using CommunityToolkit.Mvvm.ComponentModel;

namespace MFASeeker.Model
{
    public partial class Toilet : ObservableObject
    {
        [ObservableProperty]
        private int id;
        [ObservableProperty]
        private string? name;
        [ObservableProperty]
        private Location? location;
        [ObservableProperty]
        private string? description;
        [ObservableProperty]
        private double rating;

        public string? UserName { get; set; }
        public string? UserId { get; set; }
        public DateTime CreatedDate { get; set; }

        public string GetMarkerInfo()
        {
            return $"Name: {Name}, Location: {Location}, Description: {Description}, Rating: {Rating}, Created By: {UserId}, On: {CreatedDate}";
        }
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }
    }
}
