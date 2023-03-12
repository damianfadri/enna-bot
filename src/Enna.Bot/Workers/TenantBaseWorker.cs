using Enna.Core.Domain;
using Enna.Discord.Application.Contracts;
using MediatR;

namespace Enna.Bot.Workers
{
    public abstract class TenantBaseWorker : IWorker
    {
        protected Guid? TenantId { get; private set; }
        protected IMediator Mediator { get; private set; }
        protected IUnitOfWork UnitOfWork { get; private set; }

        public TenantBaseWorker(
            IMediator mediator,
            IUnitOfWork unitOfWork)
        {
            ArgumentNullException.ThrowIfNull(mediator);
            ArgumentNullException.ThrowIfNull(unitOfWork);

            Mediator = mediator;
            UnitOfWork = unitOfWork;
        }

        public virtual async Task DoWork(params object[] args)
        {
            TenantId = await GetOrCreateTenantId((ulong)args[0]);
        }

        protected async Task SendToTenantAsync(IRequest request)
        {
            if (!TenantId.HasValue)
            {
                throw new InvalidOperationException(
                    $"{nameof(TenantId)} is not set.");
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
                throw new InvalidOperationException(
                    $"{nameof(TenantId)} is not set.");
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
                        new GetGuildTenantRequest(guildId));

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
