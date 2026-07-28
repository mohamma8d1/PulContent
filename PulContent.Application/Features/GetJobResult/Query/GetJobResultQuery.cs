using MediatR;
using PulContent.Application.Features.GetJobResult.DTOs;
using System;

namespace PulContent.Application.Features.GetJobResult.Query;

public record GetJobResultQuery(Guid JobId): IRequest<JobResultDto>;

