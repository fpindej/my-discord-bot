using Microsoft.Extensions.Logging;
using OpenAI_API;

namespace OpenAi.Client.Services;

public sealed class AiTextService
{
    private readonly IOpenAIAPI _openAiApi;
    private readonly ILogger<AiTextService> _logger;

    public AiTextService(IOpenAIAPI openAiApi, ILogger<AiTextService> logger)
    {
        _openAiApi = openAiApi ?? throw new ArgumentNullException(nameof(openAiApi));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<string> GetTextResponse(string prompt)
    {
        var chat = _openAiApi.Chat.CreateConversation();
        
        _logger.LogDebug("Setting up the mood for the conversation.");
        chat.AppendSystemMessage("You are on a Discord server full of men who are trying to live their best lives and support each other, because they are homies. They love good mood, they love their egos, and they love emojis. You need to talk like you were their bro and recognize them as your G-s. It's completely fine to tell somebody \"Hey my G\"");
        
        _logger.LogDebug("Prompting the AI.");
        chat.AppendUserInput(prompt);
        _logger.LogDebug("Prompt completed.");

        var response = await chat.GetResponseFromChatbotAsync();
        _logger.LogDebug("AI response received.");
        
        return response;
    }
}