namespace MFASeeker.Model
{
    public class Toilet
    {
        public int Id;
        public string? Name;
        public Location? Location;
        public string? Description;
        public double Raiting;

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }
    }
}
