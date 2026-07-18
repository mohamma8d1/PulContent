using PulContent.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulContent.Domain.Entities;

public class GeneratedContent : BaseEntity
{
    public Guid JobId { get; private set; }
    public ContentType Type { get; private set; }
    public string ContentBody { get; private set; } = string.Empty;

    // Navigation Property
    public ProcessingJob ProcessingJob { get; private set; } = null!;
}
