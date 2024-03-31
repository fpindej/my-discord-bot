using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAi.Client.Interfaces;

namespace OpenAi.Client.Services;

public sealed class AiTextService : IAiTextService
{
    private readonly IOpenAIAPI _openAiApi;
    private readonly IMemoryCache _cache;
    private readonly OpenAiConfiguration _config;
    private readonly ILogger<AiTextService> _logger;

    public AiTextService(IOpenAIAPI openAiApi, IMemoryCache cache, IOptionsSnapshot<OpenAiConfiguration> config, ILogger<AiTextService> logger)
    {
        _openAiApi = openAiApi ?? throw new ArgumentNullException(nameof(openAiApi));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _config = config.Value ?? throw new ArgumentNullException(nameof(config));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<string> ChatAsync(string prompt, ulong userId, bool isNewConversationRequested = false)
    {
        if (isNewConversationRequested)
        {
            RemoveConversationFromCache(userId);
        }
        var conversation = GetOrCreateConversationForUser(userId);
        conversation.AppendUserInput(prompt);
        var response = await conversation.GetResponseFromChatbotAsync();

        return response;
    }

    public Conversation CreateConversation(string? systemMessage = null)
    {
        var config = new ChatRequest
        {
            Model = _config.AiModelType
        };
        var conversation = _openAiApi.Chat.CreateConversation(config);
        conversation.AppendSystemMessage(systemMessage);

        return conversation;
    }

    private Conversation GetOrCreateConversationForUser(ulong userId)
    {
        const string systemMessage = "You are friendly, sometimes you use emojis, sometimes you don't, depends on how you feel. You like to be supportive and helpful, and you are a good listener. You are a good friend.";

        return _cache.GetOrCreate(userId, entry =>
        {
            entry.SlidingExpiration = _config.CacheSlidingExpiration;
            entry.RegisterPostEvictionCallback((key, _, reason, _) =>
            {
                var r = reason switch
                {
                    EvictionReason.Capacity => "Cache reached capacity limit.",
                    EvictionReason.Expired => $"Cache entry expired after {_config.CacheSlidingExpiration}.",
                    EvictionReason.Removed => "Cache entry manually removed.",
                    _ => "Unknown reason."
                };
                _logger.LogInformation("Conversation for user {UserId} was evicted from the cache. Reason: {Reason}.", key, r);
            });
            
            _logger.LogInformation("Creating new conversation for user {UserId}.", userId);
            return CreateConversation(systemMessage);
        }) ?? CreateConversation(systemMessage);
    }
    
    private void RemoveConversationFromCache(ulong userId)
    {
        _cache.Remove(userId);
    }
}