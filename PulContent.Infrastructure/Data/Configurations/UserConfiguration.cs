using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PulContent.Domain.Entities;
using System;

namespace PulContent.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(u => u.Email).IsRequired().HasMaxLength(255);
        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.FullName).IsRequired().HasMaxLength(100);

        builder.Property(u => u.RowVersion).IsRowVersion();

        builder.HasMany(u => u.CreditTransactions)
               .WithOne(t => t.User)
               .HasForeignKey(t => t.UserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
