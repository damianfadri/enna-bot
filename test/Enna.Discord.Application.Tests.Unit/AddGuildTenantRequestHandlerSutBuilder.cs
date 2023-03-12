using Enna.Discord.Application.Handlers;
using Enna.Discord.Domain;
using Enna.Streamers.Domain;
using Moq;

namespace Enna.Discord.Application.Tests.Unit
{
    public class AddGuildTenantRequestHandlerSutBuilder
    {
        private IGuildTenantRepository _guildTenantRepository;

        public AddGuildTenantRequestHandlerSutBuilder()
        {
            _guildTenantRepository = new Mock<IGuildTenantRepository>().Object;    
        }

        public AddGuildTenantRequestHandlerSutBuilder WithNullGuildTenantRepository()
        {
            _guildTenantRepository = null!;
            return this;
        }

        public AddGuildTenantRequestHandlerSutBuilder WithExistingTenant(Tenant<ulong> tenant)
        {
            Mock.Get(_guildTenantRepository)
                .Setup(repository => repository.FindByGuildId(tenant.KeyId))
                .ReturnsAsync(tenant);

            return this;
        }

        public AddGuildTenantRequestHandlerSutBuilder WithMissingTenant(ulong guildId)
        {
            Mock.Get(_guildTenantRepository)
                .Setup(repository => repository.FindByGuildId(guildId))
                .ReturnsAsync((Tenant<ulong>)null!);

            return this;
        }

        public AddGuildTenantRequestHandlerSutBuilder WithVerifiableGuildTenantRepository(
            out Mock<IGuildTenantRepository> tenantRepository)
        {
            tenantRepository = Mock.Get(_guildTenantRepository);

            tenantRepository
                .Setup(repository => repository.Add(It.IsAny<Tenant<ulong>>()))
                .Returns(Task.CompletedTask);

            return this;
        }

        public AddGuildTenantRequestHandler Build()
        {
            return new AddGuildTenantRequestHandler(_guildTenantRepository);
        }
    }
}
