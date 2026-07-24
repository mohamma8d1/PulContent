using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulContent.Application.Interfaces;

public interface IAiService
{
    Task<string> TranscribeAudioAsync(string filePath, CancellationToken cancellationToken);
}
