using Microsoft.EntityFrameworkCore;
using PulContent.Domain.Entities;
using System;

namespace PulContent.Application.Interfaces;

public interface IAppDbContext
{
    DbSet<User> Users { get; }
    DbSet<MediaAsset> MediaAssets { get; }
    DbSet<ProcessingJob> ProcessingJobs { get; }
    DbSet<GeneratedContent> GeneratedContents { get; }
    DbSet<CreditTransaction> CreditTransactions { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
