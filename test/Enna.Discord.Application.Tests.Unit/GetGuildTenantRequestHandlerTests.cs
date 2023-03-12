using Enna.Discord.Application.Contracts;
using Enna.Streamers.Domain;
using FluentAssertions;
using Xunit;

namespace Enna.Discord.Application.Tests.Unit
{
    public class GetGuildTenantRequestHandlerTests
    {
        public class Constructor_Should
        {
            [Fact]
            public void ThrowException_When_TenantRepositoryIsNull()
            {
                var sut = () =>
                    new GetGuildTenantRequestHandlerSutBuilder()
                        .WithNullGuildTenantRepository()
                        .Build();

                sut.Should().Throw<ArgumentNullException>();
            }
        }

        public class Handle_Should 
        {
            [Fact]
            public async Task ThrowException_When_TenantIsNotFound()
            {
                var handler
                    = new GetGuildTenantRequestHandlerSutBuilder()
                        .WithMissingTenant(1L)
                        .Build();

                var sut = () =>
                    handler.Handle(
                        new GetGuildTenantRequest(1L),
                        CancellationToken.None);

                await sut.Should().ThrowAsync<InvalidOperationException>();
            }

            [Fact]
            public async Task ReturnTenantDto()
            {
                var tenant
                    = new Tenant<ulong>(
                        Guid.NewGuid(), 
                        1L);

                var handler
                    = new GetGuildTenantRequestHandlerSutBuilder()
                        .WithExistingTenant(tenant)
                        .Build();

                var dto
                    = await handler.Handle(
                        new GetGuildTenantRequest(1L),
                        CancellationToken.None);

                dto.Id.Should().Be(tenant.Id);
                dto.GuildId.Should().Be(tenant.KeyId);
            }
        }
    }
}
