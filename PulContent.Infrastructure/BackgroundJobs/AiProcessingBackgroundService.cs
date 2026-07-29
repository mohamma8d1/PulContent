using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PulContent.Application.Interfaces;
using PulContent.Domain.Entities;
using PulContent.Domain.Enums;

namespace PulContent.Infrastructure.BackgroundJobs;

public class AiProcessingBackgroundService(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            ProcessingJob? pendingJob = null;
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
            var aiService = scope.ServiceProvider.GetRequiredService<IAiService>();

            try
            {
                pendingJob = await dbContext.ProcessingJobs
                    .Include(x => x.MediaAsset)
                    .FirstOrDefaultAsync(j => j.Status == JobStatus.Pending, cancellationToken);

                if (pendingJob != null)
                {
                    Console.WriteLine($"[Worker] job number founded: {pendingJob.Id} for file: {pendingJob.MediaAsset.OriginalFileName}");

                    pendingJob.Status = JobStatus.Processing;
                    pendingJob.StartedAt = DateTime.UtcNow;
                    await dbContext.SaveChangesAsync(cancellationToken);

                    string extractText = await aiService.TranscribeAudioAsync(pendingJob.MediaAsset.StoredFilePath, cancellationToken);

                    var transcriptContent = new GeneratedContent
                    {
                        JobId = pendingJob.Id,
                        Type = ContentType.FullTranscript,
                        ContentBody = extractText
                    };
                    dbContext.GeneratedContents.Add(transcriptContent);

                    string blogPost = await aiService.GenerateTextAsync(extractText, "Write a detailed Blog Post based on this transcript", cancellationToken);
                    var blogContent = new GeneratedContent
                    {
                        JobId = pendingJob.Id,
                        Type = ContentType.BlogPost,
                        ContentBody = blogPost
                    };
                    dbContext.GeneratedContents.Add(blogContent);

                    string tweetThread = await aiService.GenerateTextAsync(extractText, "Write a Twitter Thread summarizing this transcript", cancellationToken);
                    var tweetContent = new GeneratedContent
                    {
                        JobId = pendingJob.Id,
                        Type = ContentType.TweetThread,
                        ContentBody = tweetThread
                    };
                    dbContext.GeneratedContents.Add(tweetContent);

                    pendingJob.Status = JobStatus.Completed;
                    pendingJob.CompletedAt = DateTime.UtcNow;
                    await dbContext.SaveChangesAsync(cancellationToken);

                    Console.WriteLine($"[Worker] job Number {pendingJob.Id} Completed successfully!");
                }
            }
                        catch (Exception ex)
            {
                var innerEx = ex.InnerException;
                Console.WriteLine($"[Worker] Main Error: {ex.Message}");
                
                if (innerEx != null)
                {
                    Console.WriteLine($"[Worker] SQL Inner Error: {innerEx.Message}");
                }

                if (pendingJob != null)
                {
                    try
                    {
                        pendingJob.Status = JobStatus.Failed;
                        pendingJob.ErrorMessage = ex.Message.Length > 1000 ? ex.Message.Substring(0, 1000) : ex.Message;
                        pendingJob.CompletedAt = DateTime.UtcNow;
                        await dbContext.SaveChangesAsync(cancellationToken);
                    }
                    catch (Exception dbEx)
                    {
                        Console.WriteLine($"[Worker] Failed to update job status: {dbEx.Message}");
                    }
                }
            }

            await Task.Delay(10000, cancellationToken);
        }
    }
}