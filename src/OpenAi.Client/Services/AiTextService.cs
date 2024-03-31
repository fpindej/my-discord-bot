using Microsoft.Extensions.Logging;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAi.Client.Interfaces;

namespace OpenAi.Client.Services;

public sealed class AiTextService : IAiTextService
{
    private readonly IOpenAIAPI _openAiApi;
    private readonly Dictionary<ulong, Conversation?> _conversations = new();
    private readonly ILogger<AiTextService> _logger;

    public AiTextService(IOpenAIAPI openAiApi, ILogger<AiTextService> logger)
    {
        _openAiApi = openAiApi ?? throw new ArgumentNullException(nameof(openAiApi));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<string> ChatAsync(string prompt, ulong userId)
    {
        var conversation = GetOrCreateConversationForUser(userId);
        conversation.AppendUserInput(prompt);
        var response = await conversation.GetResponseFromChatbotAsync();

        return response;
    }

    public Conversation CreateConversation(string? systemMessage = null)
    {
        var conversation = _openAiApi.Chat.CreateConversation();
        conversation.AppendSystemMessage(systemMessage);

        return conversation;
    }

    private Conversation GetOrCreateConversationForUser(ulong userdId)
    {
        var conversation = _conversations.GetValueOrDefault(userdId);
        
        if (conversation is null)
        {
            _logger.LogInformation("Creating new conversation");
            conversation = CreateConversation();
            _conversations[userdId] = conversation;
        }

        return conversation;
    }
}