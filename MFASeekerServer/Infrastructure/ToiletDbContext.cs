using MFASeekerServer.Core.Entities;
using MFASeekerServer.Core.Interfaces;
using MFASeekerServer.Infrastructure.Services.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;

namespace MFASeekerServer.Infrastructure
{
    public class ToiletDbContext : DbContext, IToiletDbContext
    {
        public DbSet<Toilet> Toilets { get; set; }

        public ToiletDbContext(DbContextOptions<ToiletDbContext> options)
            : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ToiletConfiguration());
            base.OnModelCreating(builder);
        }
    }
}
