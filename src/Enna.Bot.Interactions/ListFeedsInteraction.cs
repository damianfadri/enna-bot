using Discord;
using Discord.Interactions;
using Enna.Streamers.Application.Contracts;
using MediatR;

namespace Enna.Bot.Interactions
{
    public class ListFeedsInteraction
        : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IMediator _mediator;

        public ListFeedsInteraction(IMediator mediator)
        {
            ArgumentNullException.ThrowIfNull(mediator);

            _mediator = mediator;
        }

        [SlashCommand(
            name: "list-feeds",
            description: "Lists all registered feeds of a streamer.")]
        public async Task ExecuteInteractionAsync(
            [Summary(
                name: "streamer-id",
                description: "Id of the streamer.")]
            string rawStreamerId)
        {
            if (!Guid.TryParse(rawStreamerId, out var streamerId))
            {
                await FollowupAsync(
                    ephemeral: true,
                    embed: new EmbedBuilder()
                        .WithTitle("Streamer Not Removed")
                        .WithDescription(
                            $"Streamer id '{rawStreamerId}' is malformed.")
                        .WithColor(Color.Red)
                        .Build());

                return;
            }

            var streamer = await _mediator.Send(
                new GetStreamerRequest(streamerId));

            if (streamer == null)
            {
                await FollowupAsync(
                    ephemeral: true,
                    embed: new EmbedBuilder()
                        .WithTitle("Streamer Not Removed")
                        .WithDescription(
                            $"Streamer id '{rawStreamerId}' does not exist.")
                        .WithColor(Color.Red)
                        .Build());

                return;
            }

            var feeds = await _mediator.Send(
                new ListFeedsRequest(streamerId));
        }
    }
}
