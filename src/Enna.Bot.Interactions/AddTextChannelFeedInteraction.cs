using Discord;
using Discord.Interactions;
using Enna.Discord.Application.Contracts;
using Enna.Streamers.Application.Contracts;
using MediatR;

namespace Enna.Bot.Interactions
{
    public class AddTextChannelFeedInteraction
        : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IMediator _mediator;

        public AddTextChannelFeedInteraction(IMediator mediator)
        {
            ArgumentNullException.ThrowIfNull(mediator);

            _mediator = mediator;
        }

        [SlashCommand(
            name: "add-feed-discord",
            description: "Adds a text channel notifier for a streamer.")]
        public async Task ExecuteInteractionAsync(
            [Summary(
                name: "streamer-id",
                description: "Id of the streamer.")]
                string rawStreamerId,
            [Summary(
                name: "channel" ,
                description: "Channel where the message will be sent.")]
                ITextChannel channel)
        {
            await DeferAsync(true);

            if (!Guid.TryParse(rawStreamerId, out var streamerId))
            {
                await FollowupAsync(
                    ephemeral: true,
                    embed: new EmbedBuilder()
                        .WithTitle("Feed Not Added")
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
                        .WithTitle("Feed Not Added")
                        .WithDescription(
                            $"Streamer id '{rawStreamerId}' does not exist.")
                        .WithColor(Color.Red)
                        .Build());

                return;
            }

            var feedId = Guid.NewGuid();

            try
            {
                await _mediator.Send(
                    new AddFeedRequest(feedId, streamerId, "discord"));
            }
            catch (Exception ex)
            {
                await FollowupAsync(
                   ephemeral: true,
                   embed: new EmbedBuilder()
                       .WithTitle("Feed Not Added")
                       .WithDescription(ex.Message)
                       .WithColor(Color.Red)
                       .Build());
            }

            await _mediator.Send(
                new AddTextChannelFeedRequest(
                    feedId, channel.GuildId, channel.Id));

            await FollowupAsync(
                ephemeral: true,
                embed: new EmbedBuilder()
                    .WithTitle("Feed Added")
                    .WithDescription(
                        $"Successfully added feed for streamer {streamer.Name}.\r\nId: {feedId}")
                    .WithColor(Color.Green)
                    .Build());
        }
    }
}
