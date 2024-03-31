using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace CoinApi.Client.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoinApiClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<CoinApiConfiguration>()
            .BindConfiguration(CoinApiConfiguration.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        var config = configuration.GetSection(CoinApiConfiguration.SectionName).Get<CoinApiConfiguration>() ??
                     throw new Exception("Configuration was not found.");

        services.AddHttpClient<ICryptocurrencyExchangeRateProvider, CryptocurrencyExchangeRateProvider>(opt =>
        {
            opt.BaseAddress = config.BaseUrl;
            opt.DefaultRequestHeaders.Add("X-CoinAPI-Key", config.ApiKey);
        })
        .SetHandlerLifetime(TimeSpan.FromMinutes(5))
        .AddPolicyHandler(GetRetryPolicy(config.RetryCount));

        return services;
    }
    
    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int retryCount)
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode is HttpStatusCode.NotFound)
            .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}