using Discord.Interactions;
using Discord;
using Enna.Streamers.Application.Contracts;
using MediatR;
using Enna.Core.Domain;
using Enna.Discord.Application.Contracts;

namespace Enna.Bot.Interactions
{
    public class RemoveStreamerInteraction
            : TenantBaseInteraction
    {
        public RemoveStreamerInteraction(
            IMediator mediator,
            IUnitOfWork unitOfWork) 
            : base(mediator, unitOfWork)
        {
        }

        [SlashCommand(
            name: "remove-streamer",
            description: "Removes a streamer.")]
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
                        .WithTitle("Streamer Not Removed")
                        .WithDescription(
                            $"Streamer id '{rawStreamerId}' is malformed.")
                        .WithColor(Color.Red)
                        .Build());

                return;
            }

            try
            {
                var streamer =
                    await SendToTenantAsync(
                        new GetStreamerRequest(streamerId));

                if (streamer.Feed.Type == "Discord")
                {
                    await SendToTenantAsync(
                        new RemoveTextChannelFeedRequest(streamer.Feed.Id));
                }

                await SendToTenantAsync(
                    new RemoveStreamerRequest(streamerId));

                await FollowupAsync(
                    ephemeral: true,
                    embed: new EmbedBuilder()
                        .WithTitle("Streamer Removed")
                        .WithDescription(
                            $"Successfully removed {streamer.Name}.\r\nId: {streamerId}")
                        .WithColor(Color.Green)
                        .Build());

                await UnitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await FollowupAsync(
                    ephemeral: true,
                    embed: new EmbedBuilder()
                        .WithTitle("Streamer Not Removed")
                        .WithDescription(
                            ex.Message)
                        .WithColor(Color.Red)
                        .Build());
            }
        }
    }
}
