using Discord.Interactions;
using Discord;
using Enna.Streamers.Application.Contracts;
using MediatR;
using Enna.Core.Domain;

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

            var streamer = 
                await SendToTenantAsync(
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
    }
}
