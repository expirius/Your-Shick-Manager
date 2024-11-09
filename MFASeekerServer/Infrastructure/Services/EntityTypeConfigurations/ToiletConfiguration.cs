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
            builder.Property(toilet => toilet.Id).ValueGeneratedOnAdd();
            // уник индекс для guid
            builder.HasIndex(toilet => toilet.Guid).IsUnique();

            // настройка связи с user
            builder.HasOne(toilet => toilet.User) // (1) один создатель (user)
               .WithMany(user => user.Toilets) // (2) юзер с коллекцией туалетов
               .HasForeignKey(toilet => toilet.UserID) // (3) foreign на создателя
               .OnDelete(DeleteBehavior.SetNull); // (4) каскадное удаление не нужно

        }
    }
}
