using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PulContent.Domain.Entities;
using System;

namespace PulContent.Infrastructure.Data.Configurations;

public class MediaAssetConfiguration : IEntityTypeConfiguration<MediaAsset>
{
    public void Configure(EntityTypeBuilder<MediaAsset> builder)
    {
        builder.HasKey(m =>  m.Id);
        builder.Property(m => m.OriginalFileName).HasMaxLength(300).IsRequired();
        builder.Property(m => m.StoredFilePath).HasMaxLength(500).IsRequired();
    }
}
