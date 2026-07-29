using OpenAI;
using OpenAI.Audio;
using PulContent.Application.Interfaces;
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulContent.Infrastructure.Services;

public class OpenAiTranscriptionService : IAiService
{
    private readonly AudioClient _audioClient;

    public OpenAiTranscriptionService(string apiKey)
    {
        var clientOptions = new OpenAIClientOptions()
        {
            Endpoint = new Uri("https://api.bazaarlink.ai/v1")
        };

        var openAIClient = new OpenAIClient(new ApiKeyCredential(apiKey), clientOptions);

        _audioClient = openAIClient.GetAudioClient("whisper-1");
    }


    public async Task<string> TranscribeAudioAsync(string filePath, CancellationToken cancellationToken)
    {
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), filePath);

        if (!File.Exists(fullPath))
            throw new FileNotFoundException($"Audio file not found: {fullPath}");

        using var audioStream = File.OpenRead(fullPath);

        var fileName = Path.GetFileName(fullPath);

        var transcriptionOptions = new AudioTranscriptionOptions
        {
            Language = "fa"
        };

        var result = await _audioClient.TranscribeAudioAsync(audioStream, fileName, transcriptionOptions, cancellationToken);

        return result.Value.Text;
    }

    public Task<string> GenerateTextAsync(string inputText, string prompt, CancellationToken cancellationToken)
    {
        throw new NotSupportedException("OpenAiTranscriptionService does not support text generation.");
    }
}
