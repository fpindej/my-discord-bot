using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDbProvider.Models;

namespace MongoDbProvider.DataAccess;

public class LlmPromptContext
{
    private const string CollectionName = "discord-bot-llm-prompts";
    private readonly IMongoDatabase _db;

    public LlmPromptContext(IOptions<MongoDbConfiguration> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        _db = client.GetDatabase(options.Value.DatabaseName);

        LlmPromptsContext.Indexes.CreateOne(new CreateIndexModel<LlmPrompt>(Builders<LlmPrompt>.IndexKeys.Wildcard(p => p.UserId)));
    }

    public IMongoCollection<LlmPrompt> LlmPromptsContext => _db.GetCollection<LlmPrompt>(CollectionName);
}