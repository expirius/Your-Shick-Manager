using MFASeeker.Model;

namespace MFASeeker.Services
{
    interface IToiletService
    {
        // CRUD
        Task<Toilet> GetToiletById(int id);
        Task<IEnumerable<Toilet>> GetAllToilets();
        Task AddToilet(Toilet toilet);
        Task UpdateToilet(Toilet toilet);
        Task DeleteToilet(int id);

    }
    class ToiletService : IToiletService
    {
        public Task AddToilet(Toilet toilet)
        {
            throw new NotImplementedException();
        }

        public Task DeleteToilet(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Toilet>> GetAllToilets()
        {
            throw new NotImplementedException();
        }

        public Task<Toilet> GetToiletById(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateToilet(Toilet toilet)
        {
            throw new NotImplementedException();
        }
    }
}
