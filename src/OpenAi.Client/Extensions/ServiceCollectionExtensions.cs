using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenAI_API;
using OpenAi.Client.Factories;
using OpenAi.Client.Interfaces;
using OpenAi.Client.Services;

namespace OpenAi.Client.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAiClient(this IServiceCollection services)
    {
        services.AddOptions<OpenAiConfiguration>()
            .BindConfiguration(OpenAiConfiguration.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOpenAiClient();
        services.AddAiServices();

        return services;
    }

    private static IServiceCollection AddOpenAiClient(this IServiceCollection services)
    {
        services.AddTransient<IOpenAIAPI, OpenAIAPI>(p =>
        {
            var config = p.GetRequiredService<IOptions<OpenAiConfiguration>>().Value;
            return new OpenAIAPI(config.ApiKey);
        });

        return services;
    }

    private static IServiceCollection AddAiServices(this IServiceCollection services)
    {
        services.AddAiTextService();
        services.AddSingleton<IAiImageService, AiImageService>();
        services.AddSingleton<IAiAudioService, AiAudioService>();

        return services;
    }

    private static IServiceCollection AddAiTextService(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<IPromptService, PromptService>();
        services.AddSingleton<ChatFactory>();
        services.AddSingleton<IAiTextService, AiTextService>();

        return services;
    }
}