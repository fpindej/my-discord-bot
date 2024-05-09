using System.ComponentModel.DataAnnotations;
using OpenAI_API.Models;

namespace OpenAi.Client;

public sealed class OpenAiConfiguration
{
    public const string SectionName = "OpenAiConfiguration";

    [Required]
    public string ApiKey { get; init; } = null!;

    public TextAiConfiguration? TextAi { get; init; }

    public ImageAiConfiguration? ImageAiConfiguration { get; init; }

    public AudioAiConfiguration? AudioAiConfiguration { get; init; }
}

public sealed class TextAiConfiguration
{
    public const string SectionName = "TextAiConfiguration";

    public Model? TextModelType { get; init; } = Model.GPT4_Turbo;

    public string? DefaultChatContext { get; init; }

    public TimeSpan CacheSlidingExpiration { get; init; } = TimeSpan.FromSeconds(30);
}

public sealed class AudioAiConfiguration
{
    public const string SectionName = "AudioAiConfiguration";

    public Model? AudioModelType { get; init; } = Model.TTS_HD;
}

public sealed class ImageAiConfiguration
{
    public const string SectionName = "ImageAiConfiguration";

    public Model? ImageModelType { get; init; } = Model.DALLE3;
}