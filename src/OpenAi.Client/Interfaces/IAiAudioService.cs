namespace OpenAi.Client.Interfaces;

public interface IAiAudioService
{
    Task<Stream> CreateAudioAsync(string prompt);
}