namespace MFASeeker.Model
{
    public interface IPinStorage
    {
        Task<List<Toilet>> GetMarkers();
        Task SaveMarker(Toilet marker);
        Task DeleteMarker(Toilet marker);
    }
}
