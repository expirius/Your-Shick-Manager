namespace MFASeekerApp.Model.Interfaces
{
    public interface IPinStorage
    {
        Task<List<Toilet>> GetMarkersAsync();
        Task SaveMarkerAsync(Toilet marker);
        Task DeleteMarkerAsync(Toilet marker);
    }
}
