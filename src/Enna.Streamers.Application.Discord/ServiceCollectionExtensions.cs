using Microsoft.Extensions.DependencyInjection;

namespace Enna.Streamers.Application.Discord
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTextChannelFeedServices(
            this IServiceCollection services)
        {
            return services;
        }
    }
}
