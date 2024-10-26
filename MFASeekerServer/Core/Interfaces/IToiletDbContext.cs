using MFASeekerServer.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace MFASeekerServer.Core.Interfaces
{
    public interface ISeekerDbContext
    {
        DbSet<Toilet> Toilets { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<ImageFile> ImageFiles { get; set; }
        DbSet<UserImageToilet> ToiletImages { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
