using Enna.Core.Application;
using Enna.Core.Domain;
using Enna.Streamers.Application.Contracts;
using MediatR;

namespace Enna.Streamers.Application.Tenant
{
    public class ListStreamersByTenantRequestHandler
        : MultitenantQueryHandler<IEnumerable<StreamerDto>>
    {
        public ListStreamersByTenantRequestHandler(
            IMediator mediator,
            ITenantProvider tenantProvider)
            : base(mediator, tenantProvider)
        {
        }
    }
}
