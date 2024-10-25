using MFASeekerServer.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MFASeekerServer.Infrastructure.Services.EntityTypeConfigurations
{
    public class ToiletConfiguration : IEntityTypeConfiguration<Toilet>
    {
        public void Configure(EntityTypeBuilder<Toilet> builder)
        {
            builder.HasKey(toilet => toilet.Id);
            builder.HasIndex(toilet => toilet.Id).IsUnique();
        }
    }
}
