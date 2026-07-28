using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PulContent.Application.Features.GetJobResult.DTOs;
using PulContent.Application.Features.GetJobResult.Query;

namespace PulContent.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobsController(IMediator mediator) : ControllerBase
{

    [HttpGet("id:guid/result")]
    public async Task<IActionResult> GetResult(Guid id, CancellationToken cancellation)
    {
        var query = new GetJobResultQuery(id);
        var result = await mediator.Send(query, cancellation);
        
        if(result is null)
            return NotFound(new { Messege = "Job Not found!!" });

        return Ok(result);
    }
}
