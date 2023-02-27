using Microsoft.Extensions.DependencyInjection;

namespace Enna.Discord.Application
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
