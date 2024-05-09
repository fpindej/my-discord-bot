namespace OpenAi.Client.Interfaces;

public interface IAiImageService
{
    Task<Stream> CreateImageAsync(string prompt);
}