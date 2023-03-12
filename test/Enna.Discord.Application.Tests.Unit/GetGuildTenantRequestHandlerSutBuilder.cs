using Enna.Discord.Application.Handlers;
using Enna.Discord.Domain;
using Enna.Streamers.Domain;
using Moq;

namespace Enna.Discord.Application.Tests.Unit
{
    public class GetGuildTenantRequestHandlerSutBuilder
    {
        private IGuildTenantRepository _guildTenantRepository;

        public GetGuildTenantRequestHandlerSutBuilder()
        {
            _guildTenantRepository = new Mock<IGuildTenantRepository>().Object;
        }

        public GetGuildTenantRequestHandlerSutBuilder WithNullGuildTenantRepository()
        {
            _guildTenantRepository = null!;
            return this;
        }

        public GetGuildTenantRequestHandlerSutBuilder WithMissingTenant(ulong guildId)
        {
            Mock.Get(_guildTenantRepository)
                .Setup(repository => repository.FindByGuildId(guildId))
                .ReturnsAsync((Tenant<ulong>)null!);

            return this;
        }

        public GetGuildTenantRequestHandlerSutBuilder WithExistingTenant(Tenant<ulong> tenant)
        {
            Mock.Get(_guildTenantRepository)
                .Setup(repository => repository.FindByGuildId(tenant.KeyId))
                .ReturnsAsync(tenant);

            return this;
        }

        public GetGuildTenantRequestHandler Build()
        {
            return new GetGuildTenantRequestHandler(_guildTenantRepository);
        }
    }
}