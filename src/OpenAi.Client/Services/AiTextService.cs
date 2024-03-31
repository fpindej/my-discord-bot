using Microsoft.Extensions.Logging;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAi.Client.Interfaces;

namespace OpenAi.Client.Services;

public sealed class AiTextService : IAiTextService
{
    private readonly IOpenAIAPI _openAiApi;
    private readonly ILogger<AiTextService> _logger;

    private Conversation? _conversation;

    public AiTextService(IOpenAIAPI openAiApi, ILogger<AiTextService> logger)
    {
        _openAiApi = openAiApi ?? throw new ArgumentNullException(nameof(openAiApi));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<string> ChatAsync(string prompt, string? systemMessage = null)
    {
        _conversation ??= CreateConversation(systemMessage);
        _conversation.AppendUserInput(prompt);
        var response = await _conversation.GetResponseFromChatbotAsync();

        return response;
    }

    private Conversation CreateConversation(string? systemMessage = null)
    {
        _logger.LogInformation("Creating new conversation");
        var conversation = _openAiApi.Chat.CreateConversation();
        conversation.AppendSystemMessage(systemMessage);

        return conversation;
    }
}