using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PulContent.Application.Features.UploadMedia.Commands;

namespace PulContent.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController(IMediator mediator, IWebHostEnvironment env) : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> UploadMedia(IFormFile file, CancellationToken cancellationToken)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Please Choose File.");

            var uploadsFolder = Path.Combine(env.WebRootPath, "Uploads"); // Uses wwwroot/Uploads
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            var relativePath = Path.Combine("Uploads", uniqueFileName).Replace('\\', '/');

            var fullPath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream, cancellationToken);
            }

            var command = new UploadMediaCommand
            {
                OrginalFileName = file.FileName,
                FileSizeBytes = file.Length,
                StoredFilePath = relativePath,
                DurationSeconds = null
            };

            var result = await mediator.Send(command, cancellationToken);

            return Ok(result);

        }
    }
}
