using MassTransit;
using MediatR;
using PulContent.Application.Events;
using PulContent.Application.Features.UploadMedia.DTOs;
using PulContent.Application.Interfaces;
using PulContent.Domain.Entities;
using PulContent.Domain.Enums;
using System;

namespace PulContent.Application.Features.UploadMedia.Commands;

public class UploadMediaCommandHandler(IAppDbContext dbContext, IPublishEndpoint publishEndpoint) : IRequestHandler<UploadMediaCommand, UploadMediaResponseDto>
{
    public async Task<UploadMediaResponseDto> Handle(UploadMediaCommand request, CancellationToken cancellationToken)
    {
        var mediaAsset = new MediaAsset
        {
            UserId = request.UserId,
            OriginalFileName = request.OrginalFileName,
            StoredFilePath = request.StoredFilePath,
            FileSizeBytes = request.FileSizeBytes,
            DurationSeconds = request.DurationSeconds,
        };

        var job = new ProcessingJob
        {
            MediaAssetId = mediaAsset.Id,
            Status = JobStatus.Pending,
        };

        dbContext.MediaAssets.Add(mediaAsset);
        dbContext.ProcessingJobs.Add(job);

        await dbContext.SaveChangesAsync(cancellationToken);

        await publishEndpoint.Publish(new MediaUploadedEvent(job.Id, mediaAsset.Id), cancellationToken);

        return new UploadMediaResponseDto(job.Id, "File created successfuly.Pending.....");
    }
}
