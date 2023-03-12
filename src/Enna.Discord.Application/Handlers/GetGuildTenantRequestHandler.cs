using Enna.Discord.Application.Contracts;
using Enna.Discord.Domain;
using MediatR;

namespace Enna.Discord.Application.Handlers
{
    public class GetGuildTenantRequestHandler
        : IRequestHandler<GetGuildTenantRequest, GuildTenantDto>
    {
        private readonly IGuildTenantRepository _tenantRepository;

        public GetGuildTenantRequestHandler(
            IGuildTenantRepository tenantRepository) 
        {
            ArgumentNullException.ThrowIfNull(tenantRepository);

            _tenantRepository = tenantRepository;
        }

        public async Task<GuildTenantDto> Handle(
            GetGuildTenantRequest request, 
            CancellationToken cancellationToken)
        {
            var tenant = await _tenantRepository.FindByGuildId(request.GuildId);
            if (tenant == null)
            {
                throw new InvalidOperationException(
                    $"Guild id '{request.GuildId}` has no associated tenant.");
            }

            return new GuildTenantDto(
                tenant.Id,
                tenant.KeyId);
        }
    }
}
