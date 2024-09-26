namespace MFASeeker.Model
{
    public interface IPinStorage
    {
        List<Toilet> GetMarkers();
        void SaveMarker(Toilet marker);
        void DeleteMarker(Toilet marker);
    }
}
