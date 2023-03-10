﻿using Discord;
using Discord.Interactions;
using Enna.Core.Domain;
using Enna.Discord.Application.Contracts;
using Enna.Streamers.Application.Contracts;
using MediatR;

namespace Enna.Bot.Interactions
{
    public class AddStreamerInteraction : TenantBaseInteraction
    {
        public AddStreamerInteraction(
            IMediator mediator,
            IUnitOfWork unitOfWork) 
            : base(mediator, unitOfWork)
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
                string link,
            [Summary(
                name: "channel",
                description: "Channel where the message will be sent.")]
                ITextChannel? textChannel = null,
            [Summary(
                name: "template",
                description: "Message that will be sent when streamer goes live.")]
                string? template = null)
        {
            await DeferAsync(true);

            var streamerId = Guid.NewGuid();

            await SendToTenantAsync(
                new AddStreamerRequest(streamerId, name, link));

            var feedId = Guid.NewGuid();

            try
            {
                await SendToTenantAsync(
                    new AddFeedRequest(
                        feedId, 
                        streamerId, 
                        "discord", 
                        template));
            }
            catch (Exception ex)
            {
                await FollowupAsync(
                   ephemeral: true,
                   embed: new EmbedBuilder()
                       .WithTitle("Streamer Not Added")
                       .WithDescription(ex.Message)
                       .WithColor(Color.Red)
                       .Build());
            }

            var textChannelId = Context.Channel.Id;
            if (textChannel != null)
            {
               textChannelId = textChannel.Id;
            }

            await SendToTenantAsync(
                new AddTextChannelFeedRequest(
                    Guid.NewGuid(),
                    Context.Guild.Id,
                    textChannelId));

            await FollowupAsync(
                ephemeral: true,
                embed: new EmbedBuilder()
                    .WithTitle("Streamer Added")
                    .WithDescription(
                        $"Successfully added {name}.\r\n{streamerId}")
                    .WithColor(Color.Green)
                    .Build());

            await UnitOfWork.CommitAsync();
        }
    }
}
