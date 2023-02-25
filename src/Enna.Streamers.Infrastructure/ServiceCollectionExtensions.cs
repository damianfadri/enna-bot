using Enna.Streamers.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Enna.Streamers.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStreamerInfrastructureServices(
            this IServiceCollection services)
        {
            services
                .AddTransient<ILinkFetcher, YoutubeLivestreamFetcher>();

            services.AddHttpClient<YoutubeLivestreamFetcher>();

            return services;
        }
    }
}
