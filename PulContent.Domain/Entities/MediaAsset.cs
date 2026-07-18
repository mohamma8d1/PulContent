using System;

namespace PulContent.Domain.Entities;

public class MediaAsset : BaseEntity 
{
    public Guid UserId { get; private set; }
    public string OriginalFileName { get; private set; } = string.Empty;
    public string StoredFilePath { get; private set; } = string.Empty;
    public long FileSizeBytes { get; private set; }
    public int? DurationSeconds { get; private set; }

    // Navigation Properties
    public User User { get; private set; } = null!;
    public ICollection<ProcessingJob> ProcessingJobs { get; set; } = new List<ProcessingJob>();
}
