using Enna.Core.Domain;
using MediatR;

namespace Enna.Core.Application
{
    public class MultitenantQueryHandler<TResponse>
        : IRequestHandler<TenantRequest<TResponse>, TResponse>
    {
        private readonly IMediator _mediator;
        private readonly ITenantProvider _tenantProvider;

        public MultitenantQueryHandler(
            IMediator mediator,
            ITenantProvider tenantProvider)
        {
            ArgumentNullException.ThrowIfNull(mediator);
            ArgumentNullException.ThrowIfNull(tenantProvider);

            _mediator = mediator;
            _tenantProvider = tenantProvider;
        }

        public async Task<TResponse> Handle(
            TenantRequest<TResponse> request, 
            CancellationToken cancellationToken)
        {
            _tenantProvider.TenantId = request.TenantId;
            return await _mediator.Send(request.OriginalRequest, cancellationToken);
        }
    }
}
