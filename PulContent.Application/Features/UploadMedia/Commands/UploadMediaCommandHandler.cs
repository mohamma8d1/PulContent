using MediatR;
using PulContent.Application.Features.UploadMedia.DTOs;
using PulContent.Application.Interfaces;
using PulContent.Domain.Entities;
using PulContent.Domain.Enums;
using System;

namespace PulContent.Application.Features.UploadMedia.Commands;

public class UploadMediaCommandHandler(IAppDbContext dbContext) : IRequestHandler<UploadMediaCommand, UploadMediaResponseDto>
{
    public async Task<UploadMediaResponseDto> Handle(UploadMediaCommand request, CancellationToken cancellationToken)
    {
        var mediaAsset = new MediaAsset
        {
            OriginalFileName = request.OrginalFileName,
            StoredFilePath = request.StoredFilePath,
            FileSizeBytes = request.FileSizeBytes,
            DurationSeconds = request.DurationSeconds,
        };

        var job = new ProcessingJob
        {
            MediaAsset = mediaAsset,
            Status = JobStatus.Pending,
        };

        dbContext.MediaAssets.Add(mediaAsset);
        dbContext.ProcessingJobs.Add(job);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new UploadMediaResponseDto(job.Id, "File created successfuly.Pending.....");
    }
}
