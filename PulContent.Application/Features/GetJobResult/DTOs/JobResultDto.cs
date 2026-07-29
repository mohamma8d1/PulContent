using System;

namespace PulContent.Application.Features.GetJobResult.DTOs;

public record JobResultDto(
    Guid JobId,
    string Status,
    string? ErrorMassage,
    string? TranscribeText,
    string? BlogPost,
    string? TweetThread
    );
