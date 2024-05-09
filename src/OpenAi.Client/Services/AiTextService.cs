using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAi.Client.Factories;
using OpenAi.Client.Interfaces;

namespace OpenAi.Client.Services;

public sealed class AiTextService : IAiTextService
{
    private readonly IOpenAIAPI _openAiApi;
    private readonly TextAiConfiguration _config;
    private readonly ChatFactory _chatFactory;
    private readonly ILogger<AiTextService> _logger;
    private readonly IPromptService _promptService;

    public AiTextService(IOpenAIAPI openAiApi, IOptions<OpenAiConfiguration> config, ChatFactory chatFactory, ILogger<AiTextService> logger, IPromptService promptService)
    {
        _openAiApi = openAiApi ?? throw new ArgumentNullException(nameof(openAiApi));
        _config = config.Value.TextAi ?? throw new ArgumentNullException(nameof(config));
        _chatFactory = chatFactory;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _promptService = promptService;
    }

    public async Task<string> ChatAsync(string prompt, ulong userId, string username, bool isNewConversationRequested = false)
    {
        try
        {
            var conversation = GetOrCreateConversation(userId, isNewConversationRequested);
            var response = await GetResponseFromChatbotAsync(conversation, prompt);
            await _promptService.SavePromptAsync(userId, username, prompt, response);
            return response;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error in ChatAsync.");
            throw;
        }
    }

    private Conversation GetOrCreateConversation(ulong userId, bool isNewConversationRequested)
    {
        if (isNewConversationRequested)
        {
            _chatFactory.RemoveConversation(userId);
        }

        var systemMessage = _config.DefaultChatContext;
        return _chatFactory.CreateConversation(userId, _openAiApi, _config, systemMessage);
    }

    private async Task<string> GetResponseFromChatbotAsync(Conversation conversation, string prompt)
    {
        conversation.AppendUserInput(prompt);
        return await conversation.GetResponseFromChatbotAsync();
    }
}