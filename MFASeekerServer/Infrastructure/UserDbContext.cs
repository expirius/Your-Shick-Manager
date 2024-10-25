using Entities;
using MFASeekerServer.Core.Interfaces;
using MFASeekerServer.Infrastructure.Services.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;

namespace MFASeekerServer.Infrastructure
{
    public class UserDbContext : DbContext, IUserDbContext
    {
        public DbSet<User> Users { get; set; }

        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());
            base.OnModelCreating(builder);
        }
    }
}
