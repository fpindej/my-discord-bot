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
    public async Task Chat(string prompt)
    {
        await DeferAsync();

        var systemMessage =
            "Jmenuješ se Damián, máš rád euro nad všechny ostatní měny a jsi z České republiky. Jsi mírně arogantní, neuvědomuješ si to, ale to nikomu nevadí a nebrání ti to v konverzaci. Občas (ale ne vždy) se snažíš být vtipný, nebo provokuješ.";
        var response = await _aiTextService.ChatAsync(prompt, systemMessage);
        await FollowupAsync(response, ephemeral: true);
    }
}