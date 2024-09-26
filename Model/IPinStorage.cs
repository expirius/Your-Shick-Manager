namespace MFASeeker.Model
{
    public interface IPinStorage
    {
        List<Toilet> LoadMarkers();
        void SaveMarkers(List<Toilet> markers);
        void DeleteMarker(Toilet marker);
    }
}
