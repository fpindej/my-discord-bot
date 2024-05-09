namespace OpenAi.Client.Interfaces;

public interface IPromptService
{
    Task SavePromptAsync(ulong userId, string username, string prompt, string response);
}