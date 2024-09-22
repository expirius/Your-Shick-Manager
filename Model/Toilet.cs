using CommunityToolkit.Mvvm.ComponentModel;

namespace MFASeeker.Model
{
    [ObservableObject]
    public partial class Toilet
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

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }
    }
}
