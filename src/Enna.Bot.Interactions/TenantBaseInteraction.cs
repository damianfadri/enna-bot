using Discord.Interactions;
using Enna.Core.Domain;
using Enna.Discord.Application.Contracts;
using MediatR;

namespace Enna.Bot.Interactions
{
    public class TenantBaseInteraction 
        : InteractionModuleBase<SocketInteractionContext>
    {
        protected IMediator Mediator { get; private set; }
        protected IUnitOfWork UnitOfWork { get; private set; }

        protected Guid? TenantId { get; private set; }

        public TenantBaseInteraction(
            IMediator mediator,
            IUnitOfWork unitOfWork)
        {
            ArgumentNullException.ThrowIfNull(mediator);
            ArgumentNullException.ThrowIfNull(unitOfWork);

            Mediator = mediator;
            UnitOfWork = unitOfWork;
        }

        protected async Task SendToTenantAsync(IRequest request)
        {
            if (!TenantId.HasValue)
            {
                TenantId = await GetOrCreateTenantId(Context.Guild.Id);
            }
            
            var wrapper = 
                new TenantRequest(
                    TenantId.Value,
                    request);

            await Mediator.Send(wrapper);
        }

        protected async Task<TResponse> SendToTenantAsync<TResponse>(IRequest<TResponse> request)
        {
            if (!TenantId.HasValue)
            {
                TenantId = await GetOrCreateTenantId(Context.Guild.Id);
            }

            var wrapper =
                new TenantRequest<TResponse>(
                    TenantId.Value,
                    request);

            return await Mediator.Send(wrapper);
        }

        private async Task<Guid> GetOrCreateTenantId(ulong guildId)
        {
            try
            {
                var tenant =
                    await Mediator.Send(
                        new GetGuildTenantByGuildRequest(guildId));

                return tenant.Id;
            }
            catch
            {
                var tenantId = Guid.NewGuid();

                await Mediator.Send(
                    new AddGuildTenantRequest(tenantId, guildId));

                return tenantId;
            }
        }
    }
}
