﻿using Discord;
using Discord.Interactions;
using Enna.Discord.Application.Contracts;
using Enna.Streamers.Application.Contracts;
using MediatR;
using System.Text;

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
            await DeferAsync(true);

            if (!Guid.TryParse(rawStreamerId, out var streamerId))
            {
                await FollowupAsync(
                    ephemeral: true,
                    embed: new EmbedBuilder()
                        .WithTitle("Feed List")
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
                        .WithTitle("Feed List")
                        .WithDescription(
                            $"Streamer id '{rawStreamerId}' does not exist.")
                        .WithColor(Color.Red)
                        .Build());

                return;
            }

            var feeds = await _mediator.Send(
                new ListFeedsRequest(streamerId));

            var feedDetailRequests =
                feeds
                    .Where(feed => feed.Type == "Discord")
                    .Select(feed =>
                        _mediator.Send(
                            new GetTextChannelFeedRequest(feed.Id)));

            var feedDetails = await Task.WhenAll(feedDetailRequests);

            await FollowupAsync(
                ephemeral: true,
                embed: new EmbedBuilder()
                    .WithTitle("Feed List")
                    .WithDescription(
                        BuildFeedListMessage(feedDetails))
                    .WithColor(Color.Purple)
                    .Build());
        }

        private string BuildFeedListMessage(
            IEnumerable<TextChannelFeedDto> feeds)
        {
            var builder = new StringBuilder();

            foreach (var feed in feeds)
            {
                builder.AppendLine($"Id: {feed.Id}");
                builder.AppendLine($"<#{feed.ChannelId}>");
                builder.AppendLine("Template: @link");
            }

            return builder.ToString();
        }
    }
}