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
}
