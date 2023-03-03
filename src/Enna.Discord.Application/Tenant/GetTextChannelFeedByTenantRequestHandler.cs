using Enna.Core.Application;
using Enna.Core.Domain;
using Enna.Discord.Application.Contracts;
using MediatR;

namespace Enna.Discord.Application.Tenant
{
    public class GetTextChannelFeedByTenantRequestHandler
        : MultitenantQueryHandler<TextChannelFeedDto>
    {
        public GetTextChannelFeedByTenantRequestHandler(
            IMediator mediator, 
            ITenantProvider tenantProvider) 
            : base(mediator, tenantProvider)
        {
        }
    }
}
