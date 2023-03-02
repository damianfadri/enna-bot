using Discord;
using Discord.Interactions;
using Enna.Streamers.Application.Contracts;
using MediatR;

namespace Enna.Bot.Interactions
{
    public class AddStreamerInteraction : TenantBaseInteraction
    {
        public AddStreamerInteraction(IMediator mediator) : base(mediator)
        {
        }

        [SlashCommand(
            name: "add-streamer",
            description: "Adds a streamer.")]
        public async Task ExecuteInteractionAsync(
            [Summary(
                name: "name",
                description: "Friendly name of the streamer.")]
                string name,
            [Summary(
                name: "link",
                description: "URL to the streamer's channel.")]
                string link)
        {
            await DeferAsync(true);

            var streamerId = Guid.NewGuid();

            await SendToTenantAsync(
                new AddStreamerRequest(streamerId, name, link));

            await FollowupAsync(
                ephemeral: true,
                embed: new EmbedBuilder()
                    .WithTitle("Streamer Added")
                    .WithDescription(
                        $"Successfully added {name}.\r\nId: {streamerId}")
                    .WithColor(Color.Green)
                    .Build());
        }
    }
}
