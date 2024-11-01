using MFASeekerServer.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MFASeekerServer.Infrastructure.Services.EntityTypeConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(user => user.Id);
            builder.Property(user => user.Id).ValueGeneratedOnAdd();
            builder.HasIndex(user => user.Guid).IsUnique();

            builder.Property(user => user.UserName).HasMaxLength(100).HasColumnName("Username");
        }
    }
}
