using Entities;
using Microsoft.EntityFrameworkCore;

namespace MFASeekerServer.Core.Interfaces
{
    public interface IUserDbContext
    {
        DbSet<User> Users { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
