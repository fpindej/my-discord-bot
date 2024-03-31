using System.ComponentModel.DataAnnotations;

namespace CoinApi.Client;

public sealed class CoinApiConfiguration
{
    public const string SectionName = "CoinApiConfiguration";

    [Required]
    public Uri? BaseUrl { get; init; } = null!;
    
    [Required]
    public string ApiKey { get; init; } = null!;

    public int RetryCount { get; init; } = 3;
    
    public TimeSpan HandlerLifetime { get; init; } = TimeSpan.FromMinutes(5);
}