using MFASeekerServer.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFASeekerServer.Core.Interfaces
{
    public interface IToiletService
    {
        // CRUD
        Task<List<Toilet>> GetAllToilets();
        Task AddToilet(Toilet toilet);
        Task UpdateToilet(Toilet toilet);
        Task DeleteToilet(string guid);
        Task<Toilet> GetToiletByGuid(string guid);

    }
}
