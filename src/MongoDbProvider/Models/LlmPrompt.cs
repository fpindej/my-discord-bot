using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDbProvider.Models;

public class LlmPrompt
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public ulong UserId { get; set; }

    public string Username { get; set; } = default!;

    public string Prompt { get; set; } = default!;

    public string? Response { get; set; }
}