using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenAI_API;
using OpenAI_API.Images;
using OpenAi.Client.Interfaces;

namespace OpenAi.Client.Services;

public class AiImageService : IAiImageService
{
    private readonly IOpenAIAPI _openAiApi;
    private readonly ImageAiConfiguration _config;
    private readonly ILogger<AiImageService> _logger;

    public AiImageService(IOpenAIAPI openAiApi, IOptions<OpenAiConfiguration> config, ILogger<AiImageService> logger)
    {
        _openAiApi = openAiApi ?? throw new ArgumentNullException(nameof(openAiApi));
        _config = config.Value.ImageAiConfiguration ?? throw new ArgumentNullException(nameof(config));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Stream> CreateImageAsync(string prompt)
    {
        var request = new ImageGenerationRequest
        {
            NumOfImages = 1,
            Prompt = prompt,
            Model = _config.ImageModelType,
            ResponseFormat = ImageResponseFormat.B64_json,
        };
        var response = await _openAiApi.ImageGenerations.CreateImageAsync(request);
        var result = Convert.FromBase64String(response.Data.First().Base64Data);
        
        return new MemoryStream(result);
    }
}