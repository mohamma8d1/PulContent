using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PulContent.Domain.Entities;
using System;

namespace PulContent.Infrastructure.Data.Configurations;

public class CreditTransactionConfiguration : IEntityTypeConfiguration<CreditTransaction>
{
    public void Configure(EntityTypeBuilder<CreditTransaction> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Type).HasConversion<string>().HasMaxLength(20);
        builder.Property(t => t.Description).HasMaxLength(500).IsRequired();
    }
}
