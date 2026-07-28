using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulContent.Application.Features.GetJobResult.DTOs;

public record JobResultDto(
    Guid JobId,
    string Status,
    string? ErrorMassage,
    string? TranscribeText
    );
