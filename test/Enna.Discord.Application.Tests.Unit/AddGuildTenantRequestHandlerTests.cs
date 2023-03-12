using Enna.Discord.Application.Contracts;
using Enna.Streamers.Domain;
using FluentAssertions;
using Xunit;

namespace Enna.Discord.Application.Tests.Unit
{
    public class AddGuildTenantRequestHandlerTests
    {
        public class Constructor_Should 
        {
            [Fact]
            public void ThrowException_When_TenantRepositoryIsNull()
            {
                var sut = () =>
                    new AddGuildTenantRequestHandlerSutBuilder()
                        .WithNullGuildTenantRepository()
                        .Build();

                sut.Should().Throw<ArgumentNullException>();
            }
        }

        public class Handle_Should 
        {
            [Fact]
            public async Task ThrowException_When_GuildAlreadyExists()
            {
                var tenant
                    = new Tenant<ulong>(
                        Guid.NewGuid(),
                        1L);

                var handler
                    = new AddGuildTenantRequestHandlerSutBuilder()
                        .WithExistingTenant(tenant)
                        .Build();

                var sut = () =>
                    handler.Handle(
                        new AddGuildTenantRequest(
                            Guid.NewGuid(),
                            tenant.KeyId),
                        CancellationToken.None);

                await sut.Should().ThrowAsync<InvalidOperationException>();
            }

            [Fact]
            public async Task SaveToDatabase()
            {
                var handler
                    = new AddGuildTenantRequestHandlerSutBuilder()
                        .WithMissingTenant(1L)
                        .WithVerifiableGuildTenantRepository(out var tenantRepository)
                        .Build();

                await handler.Handle(
                    new AddGuildTenantRequest(
                        Guid.NewGuid(),
                        1L),
                    CancellationToken.None);

                tenantRepository.Verify();
            }
        }
    }
}
