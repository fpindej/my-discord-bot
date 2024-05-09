using Discord.Interactions;
using OpenAi.Client.Interfaces;

namespace Discord.Commands.Modules;

public sealed class ImageModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IAiImageService _aiImageService;

    public ImageModule(IAiImageService aiImageService)
    {
        _aiImageService = aiImageService;
    }
    
    [SlashCommand("image", "Paint something with words!")]
    public async Task Chat(string prompt)
    {
        await DeferAsync();
        await using var image = await _aiImageService.CreateImageAsync(prompt);
        var result = new FileAttachment(image, "image.png");
        await FollowupWithFileAsync(result);
    }
}