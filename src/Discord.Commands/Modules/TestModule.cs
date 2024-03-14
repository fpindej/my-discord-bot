using Discord.Interactions;
using OpenAi.Client.Services;

namespace Discord.Commands.Modules;

public sealed class TestModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly AiTextService _aiTextService;

    public TestModule(AiTextService aiTextService)
    {
        _aiTextService = aiTextService;
    }

    [SlashCommand("test", "Test command")]
    public async Task Test(string input)
    {
        var response = await _aiTextService.GetTextResponse(prompt: input);
        await RespondAsync(response);
    }

    [SlashCommand("say", "Say something, don't be shy!")]
    public async Task Say(string input)
    {
        await RespondAsync($"You said **{input}**");
    }
}