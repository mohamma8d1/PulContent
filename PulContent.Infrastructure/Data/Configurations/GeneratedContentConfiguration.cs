using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PulContent.Domain.Entities;
using System;

namespace PulContent.Infrastructure.Data.Configurations;

public class GeneratedContentConfiguration : IEntityTypeConfiguration<GeneratedContent>
{
    public void Configure(EntityTypeBuilder<GeneratedContent> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Type).HasConversion<string>().HasMaxLength(30);
        builder.Property(c => c.ContentBody).IsRequired();
    }
}
