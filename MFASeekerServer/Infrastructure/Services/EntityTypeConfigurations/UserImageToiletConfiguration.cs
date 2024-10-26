using MFASeekerServer.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MFASeekerServer.Infrastructure.Services.EntityTypeConfigurations
{
    public class UserImageToiletConfiguration : IEntityTypeConfiguration<UserImageToilet>
    {
        public void Configure(EntityTypeBuilder<UserImageToilet> builder)
        {
            builder.HasKey(ust => ust.Id);
            builder.Property(ust => ust.Id).ValueGeneratedOnAdd();

            // foreign user
            builder.HasOne(ust => ust.User)
                .WithMany(user => user.UserImageToilets)
                .HasForeignKey(ust => ust.UserID)
                .OnDelete(DeleteBehavior.Restrict);
            // foreign toilet
            builder.HasOne(ust => ust.Toilet)
                .WithMany(toilet => toilet.UserImageToilets)
                .HasForeignKey(ust => ust.ToiletID)
                .OnDelete(DeleteBehavior.Cascade);
            // foreign image
            builder.HasOne(ust => ust.ImageFile)
                .WithOne() // Т.к. одна фотка ТОЛЬКО к одному туалету.
                .HasForeignKey<UserImageToilet>(ust => ust.ImageID)
                .OnDelete(DeleteBehavior.Cascade); // что тут происходит 
        }
    }
}
