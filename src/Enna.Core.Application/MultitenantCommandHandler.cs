using Enna.Core.Domain;
using MediatR;

namespace Enna.Core.Application
{
    public class MultitenantCommandHandler
        : IRequestHandler<TenantRequest>
    {
        private readonly IMediator _mediator;
        private readonly ITenantProvider _tenantProvider;

        public MultitenantCommandHandler(
            IMediator mediator,
            ITenantProvider tenantProvider)
        {
            ArgumentNullException.ThrowIfNull(mediator);
            ArgumentNullException.ThrowIfNull(tenantProvider);

            _mediator = mediator;
            _tenantProvider = tenantProvider;
        }

        public async Task Handle(
            TenantRequest request, 
            CancellationToken cancellationToken)
        {
            _tenantProvider.TenantId = request.TenantId;
            await _mediator.Send(request.OriginalRequest);
        }
    }
}
