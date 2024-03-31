using Discord.Interactions;
using OpenAI_API.Chat;
using OpenAi.Client.Interfaces;

namespace Discord.Commands.Modules;

public sealed class ChatModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IAiTextService _aiTextService;

    public ChatModule(IAiTextService aiTextService)
    {
        _aiTextService = aiTextService;
    }
    
    [SlashCommand("chat", "Chat with AI")]
    public async Task Chat(string prompt)
    {
        await DeferAsync();
        var userId = Context.User.Id;
        var response = await _aiTextService.ChatAsync(prompt, userId);
        await FollowupAsync(response);
    }
}