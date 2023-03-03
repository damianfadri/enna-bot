using Enna.Core.Application;
using Enna.Core.Domain;
using Enna.Streamers.Application.Contracts;
using MediatR;

namespace Enna.Streamers.Application.Tenant
{
    public class GetStreamerByTenantRequestHandler
        : MultitenantQueryHandler<StreamerDto>
    {
        public GetStreamerByTenantRequestHandler(
            IMediator mediator, 
            ITenantProvider tenantProvider) 
            : base(mediator, tenantProvider)
        {
        }
    }
}
