using Entities;
namespace MFASeeker.Model
{
    public interface IPinStorage
    {
        Task<List<Toilet>> GetMarkersAsync();
        Task SaveMarkerAsync(Toilet marker);
        Task DeleteMarkerAsync(Toilet marker);
    }
}
