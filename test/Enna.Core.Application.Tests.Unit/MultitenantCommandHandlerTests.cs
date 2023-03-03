using Enna.Core.Domain;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace Enna.Core.Application.Tests.Unit
{
    public class MultitenantCommandHandlerTests
    {
        public class Constructor_Should
        {
            [Fact]
            public void ThrowException_When_MediatorIsNull()
            {
                var sut = () =>
                    new MultitenantCommandHandlerSutBuilder()
                        .WithNullMediator()
                        .Build();

                sut.Should().Throw<ArgumentNullException>();
            }

            [Fact]
            public void ThrowException_When_TenantProviderIsNull()
            {
                var sut = () =>
                    new MultitenantCommandHandlerSutBuilder()
                        .WithNullTenantProvider()
                        .Build();

                sut.Should().Throw<ArgumentNullException>();
            }
        }

        public class Handle_Should
        {
            [Fact]
            public async Task SetTenantId()
            {
                var tenantId = Guid.NewGuid();

                var handler =
                    new MultitenantCommandHandlerSutBuilder()
                        .WithExpectedTenant(tenantId, out var tenantProvider)
                        .Build();

                await handler.Handle(
                    new TenantRequest(
                        tenantId,
                        new Mock<IRequest>().Object),
                    CancellationToken.None);

                tenantProvider.VerifySet(provider => provider.TenantId = tenantId);
            }
        }
    }
}
