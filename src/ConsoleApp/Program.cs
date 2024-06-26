﻿using Discord.Commands.Extensions;
using ConsoleApp.Extensions;
using Discord.BotConfiguration.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MongoDbProvider.Extensions;
using OpenAi.Client.Extensions;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureSerilog();
builder.ConfigureAppConfiguration(config =>
{
    var environmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

    config.AddJsonFile("appsettings.json", false, true);
    config.AddJsonFile($"appsettings.{environmentName}.json", true, true);
});
builder.ConfigureServices((hostBuilder, services) =>
{
    services.AddAiClient();
    services.AddMongoDbProvider(hostBuilder.Configuration);

    // The sequence of these calls is crucial due to assembly scanning, and they should be placed at the end.
    services.AddDiscordBotConfiguration(hostBuilder.Configuration);
    services.AddDiscordCommands(hostBuilder.Configuration);
});

await builder.Build().RunAsync();