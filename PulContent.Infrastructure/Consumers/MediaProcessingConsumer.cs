using MassTransit;
using Microsoft.EntityFrameworkCore;
using PulContent.Application.Events;
using PulContent.Application.Interfaces;
using PulContent.Domain.Entities;
using PulContent.Domain.Enums;

namespace PulContent.Infrastructure.Consumers;

public class MediaProcessingConsumer(IAppDbContext dbContext, IAiService aiService, ICacheService cacheService) : IConsumer<MediaUploadedEvent>
{
    public async Task Consume(ConsumeContext<MediaUploadedEvent> context)
    {
        var message = context.Message;

        var job = await dbContext.ProcessingJobs
            .Include(x => x.MediaAsset)
            .ThenInclude(x => x.User)
            .FirstOrDefaultAsync(j => j.Id == message.JobId, context.CancellationToken);

        if (job is null)
        {
            Console.WriteLine($"[Consumer] Job {message.JobId} not found.");
            return;
        }

        try
        {
            var user = job.MediaAsset.User;

            if (user.CreditBalance <= 0)
            {
                job.Status = JobStatus.Failed;
                job.ErrorMessage = "Insufficient credit balance.";
                job.CompletedAt = DateTime.UtcNow;
                await dbContext.SaveChangesAsync(context.CancellationToken);
                Console.WriteLine($"[Consumer] Job {job.Id} failed: insufficient credit for user {user.Id}");
                return;
            }

            user.CreditBalance--;
            var transaction = new CreditTransaction
            {
                UserId = user.Id,
                Type = TransactionType.Consume,
                Amount = 1,
                Description = $"Deducted for job {job.Id}"
            };
            dbContext.CreditTransactions.Add(transaction);

            job.Status = JobStatus.Processing;
            job.StartedAt = DateTime.UtcNow;
            await dbContext.SaveChangesAsync(context.CancellationToken);

            var redisKey = $"user:{user.Id}:credits";
            var currentCached = await cacheService.GetAsync<int>(redisKey, context.CancellationToken);
            if (currentCached.HasValue)
                await cacheService.SetAsync(redisKey, currentCached.Value - 1, context.CancellationToken);
            else
                await cacheService.SetAsync(redisKey, user.CreditBalance, context.CancellationToken);

            string extractText = await aiService.TranscribeAudioAsync(job.MediaAsset.StoredFilePath, context.CancellationToken);

            var transcriptContent = new GeneratedContent
            {
                JobId = job.Id,
                Type = ContentType.FullTranscript,
                ContentBody = extractText
            };
            dbContext.GeneratedContents.Add(transcriptContent);

            string blogPost = await aiService.GenerateTextAsync(extractText, "Write a detailed Blog Post based on this transcript", context.CancellationToken);
            var blogContent = new GeneratedContent
            {
                JobId = job.Id,
                Type = ContentType.BlogPost,
                ContentBody = blogPost
            };
            dbContext.GeneratedContents.Add(blogContent);

            string tweetThread = await aiService.GenerateTextAsync(extractText, "Write a Twitter Thread summarizing this transcript", context.CancellationToken);
            var tweetContent = new GeneratedContent
            {
                JobId = job.Id,
                Type = ContentType.TweetThread,
                ContentBody = tweetThread
            };
            dbContext.GeneratedContents.Add(tweetContent);

            job.Status = JobStatus.Completed;
            job.CompletedAt = DateTime.UtcNow;
            await dbContext.SaveChangesAsync(context.CancellationToken);

            Console.WriteLine($"[Consumer] Job {job.Id} completed successfully!");
        }
        catch (DbUpdateConcurrencyException)
        {
            Console.WriteLine($"[Consumer] Concurrency conflict for job {job.Id}. The message will be retried.");
            throw;
        }
        catch (Exception ex)
        {
            var innerEx = ex.InnerException;
            Console.WriteLine($"[Consumer] Error processing job {job.Id}: {ex.Message}");

            if (innerEx != null)
            {
                Console.WriteLine($"[Consumer] Inner Error: {innerEx.Message}");
            }

            try
            {
                job.Status = JobStatus.Failed;
                job.ErrorMessage = ex.Message.Length > 1000 ? ex.Message[..1000] : ex.Message;
                job.CompletedAt = DateTime.UtcNow;
                await dbContext.SaveChangesAsync(context.CancellationToken);
            }
            catch (Exception dbEx)
            {
                Console.WriteLine($"[Consumer] Failed to update job status: {dbEx.Message}");
            }
        }
    }
}
