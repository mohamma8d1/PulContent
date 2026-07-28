using MediatR;
using Microsoft.EntityFrameworkCore;
using PulContent.Application.Features.GetJobResult.DTOs;
using PulContent.Application.Interfaces;
using PulContent.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        var TranscribeText = job.GeneratedContents.FirstOrDefault(c => c.Type == ContentType.FullTranscript)?.ContentBody;

        return new JobResultDto(
            job.Id,
            job.Status.ToString(),
            job.ErrorMessage,
            TranscribeText
            );
    }
}
