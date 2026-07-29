using PulContent.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulContent.Infrastructure.Services;

public class MockAiService : IAiService
{
    public async Task<string> TranscribeAudioAsync(string filePath, CancellationToken cancellationToken)
    {
        await Task.Delay(3000, cancellationToken);

        return "این یک متن نمونه است که توسط سرویس شبیه‌ساز (Mock) تولید شده است. " +
               "سیستم ما با موفقیت فایل صوتی را دریافت کرد، پردازش کرد و این متن را در دیتابیس ذخیره نمود. " +
               "این پیام ثابت می‌کند که معماری Clean Architecture ما به درستی کار می‌کند.";
    }

    public async Task<string> GenerateTextAsync(string inputText, string prompt, CancellationToken cancellationToken)
    {
        await Task.Delay(2000, cancellationToken);

        if (prompt.Contains("Blog", StringComparison.OrdinalIgnoreCase))
        {
            return $"# Blog Post\n\nBased on the transcript, here is a comprehensive blog post. " +
                   $"The key points discussed include various important topics that were covered in the original content. " +
                   $"This blog post expands on those ideas and presents them in a readable format for your audience.\n\n" +
                   $"> Original transcript excerpt: {inputText[..Math.Min(150, inputText.Length)]}";
        }

        if (prompt.Contains("Tweet", StringComparison.OrdinalIgnoreCase))
        {
            return "🧵 **Twitter Thread**\n\n" +
                   "1/ The main takeaway from today's content is incredibly insightful.\n\n" +
                   "2/ Let's break down the key points:\n" +
                   "   - First important concept discussed\n" +
                   "   - Second crucial insight shared\n" +
                   "   - Third actionable takeaway\n\n" +
                   "3/ This is something every content creator should know about.\n\n" +
                   "4/ What are your thoughts on this? Share below! 👇";
        }

        return $"Generated content for prompt: {prompt}\n\nBased on: {inputText[..Math.Min(100, inputText.Length)]}...";
    }
}
