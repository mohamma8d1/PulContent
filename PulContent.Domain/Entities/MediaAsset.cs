using System;

namespace PulContent.Domain.Entities;

public class MediaAsset : BaseEntity 
{
    public Guid UserId { get; private set; }
    public string OriginalFileName { get; private set; } = string.Empty;
    public string StoredFilePath { get; private set; } = string.Empty;
    public long FileSizeBytes { get; private set; }
    public int? DurationSeconds { get; private set; }

    // Constructor to set required properties
    public MediaAsset(string originalFileName, string storedFilePath, long fileSizeBytes, int? durationSeconds)
    {
        OriginalFileName = originalFileName;
        StoredFilePath = storedFilePath;
        FileSizeBytes = fileSizeBytes;
        DurationSeconds = durationSeconds;
    }

    // EF Core requires a private parameterless constructor for materialization
    private MediaAsset() { }

    // Navigation Properties
    public User User { get; private set; } = null!;
    public ICollection<ProcessingJob> ProcessingJobs { get; set; } = new List<ProcessingJob>();
}
