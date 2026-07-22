using Microsoft.EntityFrameworkCore;
using PulContent.Application.Interfaces;
using PulContent.Domain.Entities;
using PulContent.Infrastructure.Data.Configurations;
using System;

namespace PulContent.Infrastructure.Data;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    public DbSet<User> Users { get; set; }

    public DbSet<MediaAsset> MediaAssets { get; set; }

    public DbSet<ProcessingJob> ProcessingJobs { get; set; }

    public DbSet<GeneratedContent> GeneratedContents { get; set; }

    public DbSet<CreditTransaction> CreditTransactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
    }
}
