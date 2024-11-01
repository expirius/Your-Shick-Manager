using MFASeekerServer.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MFASeekerServer.Infrastructure.Services.EntityTypeConfigurations
{
    public class ImageFileConfiguration : IEntityTypeConfiguration<ImageFile>
    {
        public void Configure(EntityTypeBuilder<ImageFile> builder)
        {
            builder.HasKey(imagefile => imagefile.Id);
            builder.Property(im => im.Id).ValueGeneratedOnAdd();
            builder.Property(im => im.FileName).HasMaxLength(100);
        }
    }
}
