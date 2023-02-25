using Discord;
using Discord.Interactions;
using Enna.Streamers.Application.Contracts;
using MediatR;

namespace Enna.Streamers.Application.Discord.Interactions
{
    public class AddStreamerInteraction
        : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IMediator _mediator;

        public AddStreamerInteraction(IMediator mediator)
        {
            ArgumentNullException.ThrowIfNull(mediator);

            _mediator = mediator;
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

            await _mediator.Send(
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
