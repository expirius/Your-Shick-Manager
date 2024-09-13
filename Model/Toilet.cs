namespace MFASeeker.Model
{
    public class Toilet
    {
        public int Id;
        public string? Name;
        public Location? Location;
        public string? Description;
        public short Raiting;

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }
    }
}
