using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDbProvider.DataAccess;
using MongoDbProvider.Repositories;

namespace MongoDbProvider.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMongoDbProvider(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<MongoDbConfiguration>()
            .BindConfiguration(MongoDbConfiguration.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.Configure<MongoDbConfiguration>(configuration.GetSection(MongoDbConfiguration.SectionName));
        services.AddTransient<LlmPromptContext>();
        services.AddSingleton<LlmPromptRepository>();

        return services;
    }
}