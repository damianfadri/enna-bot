﻿using Discord;
using Discord.Interactions;
using Enna.Core.Domain;
using Enna.Discord.Application.Contracts;
using Enna.Streamers.Application.Contracts;
using MediatR;
using System.Text;

namespace Enna.Bot.Interactions
{
    public class ListStreamersInteraction
        : TenantBaseInteraction
    {
        public ListStreamersInteraction(
            IMediator mediator,
            IUnitOfWork unitOfWork) 
            : base(mediator, unitOfWork)
        {
        }

        [SlashCommand(
            name: "list-streamers",
            description: "Lists all registered streamers.")]
        public async Task ExecuteInteractionAsync()
        {
            await DeferAsync(true);

            var streamers =
                await SendToTenantAsync(
                    new ListStreamersRequest());

            if (!streamers.Any())
            {
                await FollowupAsync(
                    ephemeral: true,
                    embed: new EmbedBuilder()
                        .WithTitle("Streamer List")
                        .WithDescription(
                            "Nothing here but us chickens.\r\nAdd a streamer by invoking `/add-streamer`.")
                        .WithColor(Color.Purple)
                        .Build());

                return;
            }

            await FollowupAsync(
                ephemeral: true,
                embed: new EmbedBuilder()
                    .WithTitle("Streamer List")
                    .WithDescription(
                        await BuildStreamerListMessage(streamers))
                    .WithColor(Color.Purple)
                    .Build());
        }

        private async Task<string> BuildStreamerListMessage(
            IEnumerable<StreamerDto> streamers)
        {
            var builder = new StringBuilder();

            foreach (var streamer in streamers)
            {
                builder.AppendLine(streamer.Name);
                builder.AppendLine(streamer.Id.ToString());
                builder.AppendLine();

                builder.AppendLine("Channels:");
                foreach (var channel in streamer.Channels)
                {
                    builder.AppendLine(channel.Link);
                }

                builder.AppendLine($"Feeds: ");
                foreach (var feed in streamer.Feeds)
                {
                    builder.AppendLine(feed.MessageTemplate ?? "@link");
                    if (feed.Type == "Discord")
                    {
                        var textChannel =
                            await SendToTenantAsync(
                                new GetTextChannelFeedRequest(feed.Id));

                        builder.AppendLine($"<@{textChannel.ChannelId}>");
                    }
                }

                builder.AppendLine();
            }

            return builder.ToString();
        }
    }
}