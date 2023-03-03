using Enna.Core.Domain;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enna.Core.Application.Tests.Unit
{
    public class MultitenantCommandHandlerSutBuilder
    {
        private IMediator _mediator;
        private ITenantProvider _tenantProvider;

        public MultitenantCommandHandlerSutBuilder() 
        {
            _mediator = new Mock<IMediator>().Object;
            _tenantProvider = new Mock<ITenantProvider>().Object;
        }

        public MultitenantCommandHandlerSutBuilder WithNullMediator()
        {
            _mediator = null!;
            return this;
        }

        public MultitenantCommandHandlerSutBuilder WithNullTenantProvider()
        {
            _tenantProvider = null!;
            return this;
        }
        public MultitenantCommandHandlerSutBuilder WithExpectedTenant(Guid tenantId, out Mock<ITenantProvider> mock)
        {
            mock = Mock.Get(_tenantProvider);

            mock.SetupSet(provider => provider.TenantId = tenantId)
                .Verifiable();

            return this;
        }

        public MultitenantCommandHandler Build()
        {
            return new MultitenantCommandHandler(
                _mediator, _tenantProvider);
        }
    }
}
