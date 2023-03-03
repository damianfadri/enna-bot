using Enna.Core.Domain;
using Enna.Discord.Application.Contracts;
using Enna.Discord.Domain;
using Enna.Streamers.Domain;
using MediatR;

namespace Enna.Discord.Application.Handlers
{
    public class AddGuildTenantRequestHandler
        : IRequestHandler<AddGuildTenantRequest>
    {
        private readonly IGuildTenantRepository _tenantRepository;

        public AddGuildTenantRequestHandler(
            IGuildTenantRepository tenantRepository)
        {
            ArgumentNullException.ThrowIfNull(tenantRepository);

            _tenantRepository = tenantRepository;
        }

        public async Task Handle(
            AddGuildTenantRequest request, 
            CancellationToken cancellationToken)
        {
            var tenant = new Tenant<ulong>(
                request.Id,
                request.GuildId);

            await _tenantRepository.Add(tenant);
        }
    }
}
