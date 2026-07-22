using System;

namespace PulContent.Application.Features.UploadMedia.DTOs;

public record UploadMediaResponseDto(Guid JobId, string Messege);
