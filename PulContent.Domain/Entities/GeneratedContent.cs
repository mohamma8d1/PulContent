using PulContent.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulContent.Domain.Entities;

public class GeneratedContent : BaseEntity
{
    public Guid JobId { get; set; }
    public ContentType Type { get; set; }
    public string ContentBody { get; set; } = string.Empty;

    // Navigation Property
    public ProcessingJob ProcessingJob { get; set; } = null!;
}
