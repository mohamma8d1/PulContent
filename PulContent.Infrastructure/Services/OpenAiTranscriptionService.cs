using OpenAI.Audio;
using PulContent.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulContent.Infrastructure.Services;

public class OpenAiTranscriptionService : IAiService
{
    private readonly AudioClient _audioClient;

    public OpenAiTranscriptionService(string apiKey) => _audioClient = new AudioClient("whisper-1", apiKey);


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
}
