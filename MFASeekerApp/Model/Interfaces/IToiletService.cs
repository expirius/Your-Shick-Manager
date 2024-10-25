using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace MFASeekerApp.Model.Interfaces
{
    public interface IToiletService
    {
        // CRUD
        Task<List<Toilet>> GetAllToilets();
        Task AddToilet(Toilet toilet);
        Task UpdateToilet(Toilet toilet);
        Task DeleteToilet(Guid guid);
        Task<Toilet> GetToiletByGuid(Guid guid);

    }
}
