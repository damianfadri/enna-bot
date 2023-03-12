using Enna.Discord.Application.Contracts;
using Enna.Discord.Domain;
using MediatR;

namespace Enna.Discord.Application.Handlers
{
    public class GetGuildTenantByGuildRequestHandler
        : IRequestHandler<GetGuildTenantByGuildRequest, GuildTenantDto>
    {
        private readonly IGuildTenantRepository _tenantRepository;

        public GetGuildTenantByGuildRequestHandler(
            IGuildTenantRepository tenantRepository) 
        {
            ArgumentNullException.ThrowIfNull(tenantRepository);

            _tenantRepository = tenantRepository;
        }

        public async Task<GuildTenantDto> Handle(
            GetGuildTenantByGuildRequest request, 
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
