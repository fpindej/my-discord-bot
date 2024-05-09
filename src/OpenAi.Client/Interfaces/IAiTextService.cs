namespace OpenAi.Client.Interfaces;

public interface IAiTextService
{
    Task<string> ChatAsync(string prompt, ulong userId, string username, bool isNewConversationRequested = false);
}