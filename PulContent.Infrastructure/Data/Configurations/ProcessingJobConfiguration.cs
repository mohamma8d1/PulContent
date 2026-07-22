using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PulContent.Domain.Entities;
using System;

namespace PulContent.Infrastructure.Data.Configurations;

public class ProcessingJobConfiguration : IEntityTypeConfiguration<ProcessingJob>
{
    public void Configure(EntityTypeBuilder<ProcessingJob> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(20).IsRequired();

        builder.Property(x => x.ErrorMessage).HasMaxLength(1000);

        builder.HasOne(x => x.MediaAsset).WithMany(m => m.ProcessingJobs).HasForeignKey(j => j.MediaAssetId).OnDelete(DeleteBehavior.Cascade);
    }
}
