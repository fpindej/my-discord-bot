using System.ComponentModel.DataAnnotations;
using OpenAI_API.Models;

namespace OpenAi.Client;

public sealed class OpenAiConfiguration
{
    public const string SectionName = "OpenAiConfiguration";
    
    [Required]
    public string ApiKey { get; set; } = null!;
    
    public Model AiModelType { get; set; } = Model.GPT4_Turbo;
    
    public TimeSpan CacheSlidingExpiration { get; set; } = TimeSpan.FromSeconds(30);
}