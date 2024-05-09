using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenAI_API;
using OpenAI_API.Audio;
using OpenAi.Client.Interfaces;

namespace OpenAi.Client.Services;

public class AiAudioService : IAiAudioService
{
    private readonly IOpenAIAPI _openAiApi;
    private readonly AudioAiConfiguration _config;
    private readonly ILogger<AiAudioService> _logger;

    public AiAudioService(IOpenAIAPI openAiApi, IOptions<OpenAiConfiguration> config, ILogger<AiAudioService> logger)
    {
        _openAiApi = openAiApi ?? throw new ArgumentNullException(nameof(openAiApi));
        _config = config.Value.AudioAiConfiguration ?? throw new ArgumentNullException(nameof(config));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async Task<Stream> CreateAudioAsync(string prompt)
    {
        var request = new TextToSpeechRequest
        {
            Model = _config.AudioModelType,
            Input = prompt,
            Voice = TextToSpeechRequest.Voices.Alloy
        };
        
        return await _openAiApi.TextToSpeech.GetSpeechAsStreamAsync(request);
    }
}