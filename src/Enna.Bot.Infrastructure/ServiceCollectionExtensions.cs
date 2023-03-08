using Enna.Streamers.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Enna.Bot.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStreamerInfrastructureServices(
            this IServiceCollection services)
        {
            return services;
        }
    }
}
