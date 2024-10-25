using MFASeekerServer.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace MFASeekerServer.Core.Interfaces
{
    public interface IToiletDbContext
    {
        DbSet<Toilet> Toilets { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
