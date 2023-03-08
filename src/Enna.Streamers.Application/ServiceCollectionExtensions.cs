using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enna.Streamers.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStreamerApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services;
        }
    }
}
