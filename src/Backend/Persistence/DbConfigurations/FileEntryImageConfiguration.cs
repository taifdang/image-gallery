using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.DbConfigurations;

public class FileEntryImageConfiguration : IEntityTypeConfiguration<FileEntryImage>
{
    public void Configure(EntityTypeBuilder<FileEntryImage> builder)
    {
        builder.ToTable("FileEntryImages");
        builder.Property(x => x.Id).HasDefaultValueSql("newsequentialid()");
    }
}