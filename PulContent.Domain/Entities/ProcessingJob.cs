using PulContent.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulContent.Domain.Entities;

public class ProcessingJob : BaseEntity
{
    public Guid MediaAssetId { get; init; }
    public JobStatus Status { get; init; } = JobStatus.Pending;
    public string? ErrorMessage { get; private set; }
    public DateTime? StartedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }



    // Navigation Properties
    public MediaAsset MediaAsset { get; init; } = null!;
    public ICollection<GeneratedContent> GeneratedContents { get; set; } = new List<GeneratedContent>();
}
