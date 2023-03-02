using Discord.Interactions;
using Enna.Core.Domain;
using Enna.Discord.Application.Contracts;
using MediatR;

namespace Enna.Bot.Interactions
{
    public class TenantBaseInteraction 
        : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IMediator _mediator;

        public TenantBaseInteraction(IMediator mediator)
        {
            ArgumentNullException.ThrowIfNull(mediator);

            _mediator = mediator;
        }

        protected async Task SendToTenantAsync(IRequest request)
        {
            var tenantId = await GetOrCreateTenantId(Context.Guild.Id);

            var wrapper = 
                new TenantRequest(
                    tenantId, 
                    request);

            await _mediator.Send(wrapper);
        }

        protected async Task<TResponse> SendToTenantAsync<TResponse>(IRequest<TResponse> request)
        {
            var tenantId = await GetOrCreateTenantId(Context.Guild.Id);

            var wrapper =
                new TenantRequest<TResponse>(
                    tenantId,
                    request);

            return await _mediator.Send(wrapper);
        }

        private async Task<Guid> GetOrCreateTenantId(ulong guildId)
        {
            try
            {
                var tenant =
                    await _mediator.Send(
                        new GetGuildTenantByGuildRequest(
                            Context.Guild.Id));

                return tenant.Id;
            }
            catch
            {
                var tenantId = Guid.NewGuid();

                await _mediator.Send(
                    new AddGuildTenantRequest(
                        tenantId,
                        Context.Guild.Id));

                return tenantId;
            }
        }
    }
}
