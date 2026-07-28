using MediatR;
using PulContent.Application.Features.GetJobResult.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulContent.Application.Features.GetJobResult.Query;

public record GetJobResultQuery(Guid JobId): IRequest<JobResultDto>;

