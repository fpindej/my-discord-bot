using Discord.Interactions;
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
    public async Task Chat(string prompt, bool startNewConversation = false)
    {
        await DeferAsync();
        var userId = Context.User.Id;
        var response = await _aiTextService.ChatAsync(prompt, userId, startNewConversation);
        await FollowupAsync(response); 
    }
}