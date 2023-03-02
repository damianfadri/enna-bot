using Enna.Core.Domain;
using Enna.Discord.Application.Contracts;
using Enna.Discord.Domain;
using Enna.Streamers.Domain;
using MediatR;

namespace Enna.Discord.Application.Handlers
{
    public class AddGuildTenantRequestHandler
        : IRequestHandler<AddGuildTenantRequest>
    {
        private readonly IGuildTenantRepository _tenantRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddGuildTenantRequestHandler(
            IGuildTenantRepository tenantRepository,
            IUnitOfWork unitOfWork)
        {
            ArgumentNullException.ThrowIfNull(tenantRepository);

            _tenantRepository = tenantRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(
            AddGuildTenantRequest request, 
            CancellationToken cancellationToken)
        {
            var tenant = new Tenant<ulong>(
                request.Id,
                request.GuildId);

            await _tenantRepository.Add(tenant);
            await _unitOfWork.CommitAsync();
        }
    }
}
