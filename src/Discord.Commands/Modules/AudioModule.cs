using Discord.Audio;
using Discord.Interactions;
using OpenAi.Client.Interfaces;

namespace Discord.Commands.Modules;

public class AudioModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IAiAudioService _aiAudioService;

    public AudioModule(IAiAudioService aiAudioService)
    {
        _aiAudioService = aiAudioService;
    }

    [SlashCommand("join", "Join a voice channel and play audio")]
    public async Task JoinChannel(string prompt)
    {
        // Get the audio channel
        var channel = (Context.User as IGuildUser)?.VoiceChannel;

        if (channel is null)
        {
            await FollowupAsync("User must be in a voice channel", ephemeral: true);
            return;
        }

        await using var audioStream = await _aiAudioService.CreateAudioAsync(prompt);

        // For the next step with transmitting audio, you would want to pass this Audio Client in to a service.
        var audioClient = await channel.ConnectAsync();
        await using var discord = audioClient.CreatePCMStream(AudioApplication.Mixed);

        try
        {
            await audioStream.CopyToAsync(discord);
        }
        finally
        {
            await discord.FlushAsync();
        }
    }
}