using OpenAI_API.Chat;

namespace OpenAi.Client.Interfaces;

public interface IAiTextService
{
    Task<string> ChatAsync(string prompt, string? systemMessage = null);
}