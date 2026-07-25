using PulContent.Application.Interfaces;
using System.Diagnostics;
using System.Text;
using Whisper.net;
using Whisper.net.Ggml;

namespace PulContent.Infrastructure.Services;

public class LocalWhisperService : IAiService
{
    private readonly string _modelPath;
    private readonly string _modelFileName = "ggml-base.bin";

    public LocalWhisperService()
    {
        var modelDir = Path.Combine(Directory.GetCurrentDirectory(), "Models");
        _modelPath = Path.Combine(modelDir, _modelFileName);

        if (!File.Exists(_modelPath))
        {
            throw new FileNotFoundException($"Model Not Found!");
        }
    }

    public async Task<string> TranscribeAudioAsync(string filePath, CancellationToken cancellationToken)
    {
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), filePath);

        if (!File.Exists(fullPath))
            throw new FileNotFoundException($"Audio file not found: {fullPath}");

        Console.WriteLine($"[Local AI] Loading model from {_modelPath}...");

        var sw = Stopwatch.StartNew();

        using var factory = WhisperFactory.FromPath(_modelPath);

        using var processor = factory.CreateBuilder()
            .WithLanguage("fa")
            .Build();

        Console.WriteLine($"[Local AI] Model loaded in {sw.ElapsedMilliseconds} ms. Processing file...");

        using var fileStream = File.OpenRead(fullPath);

        var textBuilder = new StringBuilder();

        await foreach (var result in processor.ProcessAsync(fileStream, cancellationToken))
        {
            textBuilder.Append(result.Text);
        }

        Console.WriteLine($"[Local AI] Transcription completed in {sw.ElapsedMilliseconds} ms.");

        return textBuilder.ToString();
    }
}