using System;

namespace PulContent.Domain.Entities;

public class MediaAsset : BaseEntity 
{
    public Guid UserId { get; init; }
    public string OriginalFileName { get; init; } = string.Empty;
    public string StoredFilePath { get; init; } = string.Empty;
    public long FileSizeBytes { get; init; }
    public int? DurationSeconds { get; init; }

    // Navigation Properties
    public User User { get; private set; } = null!;
    public ICollection<ProcessingJob> ProcessingJobs { get; set; } = new List<ProcessingJob>();
}
