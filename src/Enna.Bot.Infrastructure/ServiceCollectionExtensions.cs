using Enna.Streamers.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Enna.Bot.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddTransient<ILinkFetcher, YoutubeLivestreamFetcher>();
            services.AddHttpClient<YoutubeLivestreamFetcher>();

            services.AddTransient<ILinkFetcher, TwitchLivestreamFetcher>();
            services.AddHttpClient<TwitchLivestreamFetcher>();

            return services;
        }
    }
}
