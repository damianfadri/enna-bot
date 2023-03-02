using MediatR;

namespace Enna.Core.Domain
{
    public record TenantRequest(
        Guid TenantId,
        IRequest OriginalRequest)
        : IRequest;

    public record TenantRequest<TResponse>(
        Guid TenantId,
        IRequest<TResponse> OriginalRequest)
        : IRequest<TResponse>;
}
