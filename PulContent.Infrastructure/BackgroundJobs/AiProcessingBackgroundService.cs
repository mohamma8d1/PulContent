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
                    .ThenInclude(x => x.User)
                    .FirstOrDefaultAsync(j => j.Status == JobStatus.Pending, cancellationToken);

                if (pendingJob != null)
                {
                    var user = pendingJob.MediaAsset.User;

                    if (user.CreditBalance <= 0)
                    {
                        pendingJob.Status = JobStatus.Failed;
                        pendingJob.ErrorMessage = "Insufficient credit balance.";
                        pendingJob.CompletedAt = DateTime.UtcNow;
                        await dbContext.SaveChangesAsync(cancellationToken);
                        Console.WriteLine($"[Worker] Job {pendingJob.Id} failed: insufficient credit for user {user.Id}");
                        continue;
                    }

                    user.CreditBalance--;
                    var transaction = new CreditTransaction
                    {
                        UserId = user.Id,
                        Type = TransactionType.Consume,
                        Amount = 1,
                        Description = $"Deducted for job {pendingJob.Id}"
                    };
                    dbContext.CreditTransactions.Add(transaction);

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

                    Console.WriteLine($"[Worker] Job {pendingJob.Id} completed successfully!");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                Console.WriteLine($"[Worker] Concurrency conflict for job {pendingJob?.Id}. Will retry on next poll.");
            }
            catch (Exception ex)
            {
                var innerEx = ex.InnerException;
                Console.WriteLine($"[Worker] Main Error: {ex.Message}");
                
                if (innerEx != null)
                {
                    Console.WriteLine($"[Worker] Inner Error: {innerEx.Message}");
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