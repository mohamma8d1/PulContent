using MediatR;
using Microsoft.EntityFrameworkCore;
using PulContent.Application.Features.GetJobResult.DTOs;
using PulContent.Application.Interfaces;
using PulContent.Domain.Enums;
using System;

namespace PulContent.Application.Features.GetJobResult.Query;

public class GetJobResultQueryHandler(IAppDbContext dbContext) : IRequestHandler<GetJobResultQuery, JobResultDto>
{
    public async Task<JobResultDto> Handle(GetJobResultQuery request, CancellationToken cancellationToken)
    {
        var job = await dbContext.ProcessingJobs
            .Include(i => i.GeneratedContents)
            .FirstOrDefaultAsync(i => i.Id == request.JobId, cancellationToken);
        if (job == null)
            return null;

        var transcript = job.GeneratedContents.FirstOrDefault(c => c.Type == ContentType.FullTranscript)?.ContentBody;
        var blogPost = job.GeneratedContents.FirstOrDefault(c => c.Type == ContentType.BlogPost)?.ContentBody;
        var tweetThread = job.GeneratedContents.FirstOrDefault(c => c.Type == ContentType.TweetThread)?.ContentBody;

        return new JobResultDto(
            job.Id,
            job.Status.ToString(),
            job.ErrorMessage,
            transcript,
            blogPost,
            tweetThread
            );
    }
}
