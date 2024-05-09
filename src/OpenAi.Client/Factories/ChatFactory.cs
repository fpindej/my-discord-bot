using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using OpenAI_API;
using OpenAI_API.Chat;

namespace OpenAi.Client.Factories;

public sealed class ChatFactory
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<ChatFactory> _logger;
    
    public ChatFactory(IMemoryCache cache, ILogger<ChatFactory> logger)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _logger = logger;
    }
    
    public Conversation CreateConversation(ulong userId, IOpenAIAPI openAiapi, TextAiConfiguration config, string? systemMessage = null)
    {
        return _cache.GetOrCreate(userId, entry =>
        {
            entry.SlidingExpiration = config.CacheSlidingExpiration;
            entry.RegisterPostEvictionCallback((key, _, reason, _) =>
            {
                var r = reason switch
                {
                    EvictionReason.Capacity => "Cache reached capacity limit.",
                    EvictionReason.Expired => $"Cache entry expired after {config.CacheSlidingExpiration}.",
                    EvictionReason.Removed => "Cache entry manually removed.",
                    _ => "Unknown reason."
                };
                _logger.LogInformation("Conversation for user {UserId} was evicted from the cache. Reason: {Reason}", key, r);
            });
            
            _logger.LogInformation("Creating new conversation for user {UserId}.", userId);
            return CreateConversation(openAiapi, config, systemMessage);
        }) ?? CreateConversation(openAiapi, config, systemMessage);
    }
    
    public void RemoveConversation(ulong userId)
    {
        _cache.Remove(userId);
    }
    
    private Conversation CreateConversation(IOpenAIAPI openAiApi, TextAiConfiguration config, string? systemMessage = null)
    {
        var chatRequest = new ChatRequest
        {
            Model = config.TextModelType
        };
        var conversation = openAiApi.Chat.CreateConversation(chatRequest);
        conversation.AppendSystemMessage(systemMessage);

        return conversation;
    }
}