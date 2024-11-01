using MFASeekerServer.Core.Entities;
using MFASeekerServer.Core.Interfaces;
using MFASeekerServer.Infrastructure.Services.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;

namespace MFASeekerServer.Infrastructure
{
    public class SeekerDbContext : DbContext, ISeekerDbContext
    {
        public DbSet<Toilet> Toilets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ImageFile> ImageFiles { get; set; }
        public DbSet<UserImageToilet> ToiletImages { get; set; }

        public SeekerDbContext(DbContextOptions<SeekerDbContext> options)
            : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ToiletConfiguration()); // toilets by user
            builder.ApplyConfiguration(new UserConfiguration()); // users
            builder.ApplyConfiguration(new ImageFileConfiguration()); // pics
            builder.ApplyConfiguration(new UserImageToiletConfiguration()); // pics from toilet and users

            base.OnModelCreating(builder);
        }
    }
}
