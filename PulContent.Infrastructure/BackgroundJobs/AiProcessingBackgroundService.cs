using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PulContent.Application.Interfaces;
using PulContent.Domain.Entities;
using PulContent.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulContent.Infrastructure.BackgroundJobs;

public class AiProcessingBackgroundService(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
            var aiService = scope.ServiceProvider.GetRequiredService<IAiService>();

            try
            {
                var pendingJob = await dbContext.ProcessingJobs
                    .Include(x => x.MediaAsset)
                    .FirstOrDefaultAsync(j => j.Status == JobStatus.Pending, cancellationToken);

                if (pendingJob != null)
                {
                    Console.WriteLine($"[Worker] job number founded: {pendingJob.Id} for file: {pendingJob.MediaAsset.OriginalFileName}");

                    pendingJob.Status = JobStatus.Processing;
                    pendingJob.StartedAt = DateTime.UtcNow;
                    await dbContext.SaveChangesAsync(cancellationToken);

                    string extractText = await aiService.TranscribeAudioAsync(pendingJob.MediaAsset.StoredFilePath, cancellationToken);

                    var generatedContent = new GeneratedContent
                    {
                        JobId = pendingJob.Id,
                        Type = ContentType.FullTranscript,
                        ContentBody = extractText
                    };
                    dbContext.GeneratedContents.Add(generatedContent);

                    pendingJob.Status = JobStatus.Completed;
                    pendingJob.CompletedAt = DateTime.UtcNow;
                    await dbContext.SaveChangesAsync(cancellationToken);

                    Console.WriteLine($"[Worker] job Number {pendingJob.Id} Completed successfully!");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Worker] Error while proccess: {ex.Message}");
            }
            await Task.Delay(10000, cancellationToken);
        }
    }
}
