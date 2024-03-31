using OpenAI_API.Chat;

namespace OpenAi.Client.Interfaces;

public interface IAiTextService
{
    Conversation CreateConversation(string? systemMessage = null);
    
    Task<string> ChatAsync(string prompt, ulong userId, bool isNewConversationRequested = false);
}