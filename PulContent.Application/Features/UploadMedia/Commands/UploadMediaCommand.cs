using MediatR;
using PulContent.Application.Features.UploadMedia.DTOs;
using System;

namespace PulContent.Application.Features.UploadMedia.Commands;

public class UploadMediaCommand : IRequest<UploadMediaResponseDto>
{
    public string OrginalFileName { get; set; } = string.Empty;
    public long FileSizeBytes { get; set; }
    public string StoredFilePath { get; set; } = string.Empty;
    public int? DurationSeconds { get; set; }
}
